using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonitorPet.Application.Model.User;
using MonitorPet.Application.Services.Interfaces;
using System.Security.Claims;

namespace MonitorPet.Application.Tests;

public abstract class TestBase
{
    private static TestHost _host = new();

    /// <summary>
    /// Static provider
    /// </summary>
    protected IServiceProvider ServiceProvider => _host.ServiceProvider;

    public TestBase()
    {
    }

    protected async Task<(CreateUserModel UserCreated, IEnumerable<Claim> Claims, Model.Dosador.DosadorModel Dosador)> CreateUserWithDosador()
    {
        var tupleUserCreated = await CreateAndLoginUser();
        var dosadorCreated = await CreateDosador();

        var dosadorService = ServiceProvider.GetRequiredService<IDosadorService>();

        using var context = CreateContext(tupleUserCreated.Claims);

        await dosadorService.AddDosadorToUser(context.ClaimModel.IdUser, dosadorCreated.IdDosador.ToString());

        return (tupleUserCreated.UserCreated, tupleUserCreated.Claims, dosadorCreated);
    }

    protected InternalContext CreateContext(IEnumerable<Claim> claims)
    {
        return new InternalContext(
            new Security.CurrentClaim(
                claims
            )
        );
    }

    protected internal async Task<(CreateUserModel UserCreated, IEnumerable<Claim> Claims)> CreateAndLoginUser(
        IServiceProvider? scoopedServiceProvider = null)
    {
        var tupleUserCreated = await CreateCompleteUserEmailConfirmed(scoopedServiceProvider);

        if (scoopedServiceProvider is null)
            scoopedServiceProvider = ServiceProvider;

        var userService = scoopedServiceProvider.GetRequiredService<IUserService>();

        var resultLogin =
            await userService.Login(tupleUserCreated.UserCreated.Email, tupleUserCreated.UserCreated.Password);

        return (tupleUserCreated.UserCreated, resultLogin.Claims);
    }

    protected internal async Task<(CreateUserModel UserCreated, Email.FakeInbox Inbox)> CreateCompleteUserEmailConfirmed(
        IServiceProvider? scoopedServiceProvider = null)
    {
        var tupleUserCreated = await CreateCompleteUser(scoopedServiceProvider);

        if (scoopedServiceProvider is null)
            scoopedServiceProvider = ServiceProvider;

        var userService = scoopedServiceProvider.GetRequiredService<IUserService>();

        using var contextClaim =
            CreateContext(tupleUserCreated.Inbox.TryGetObjectLastInbox<Claim[]>() ?? Enumerable.Empty<Claim>());

        await userService.ConfirmEmail(tupleUserCreated.UserCreated.Email);

        return tupleUserCreated;
    }

    protected internal async Task<(CreateUserModel UserCreated, Email.FakeInbox Inbox)> CreateCompleteUser(
        IServiceProvider? scoopedServiceProvider = null)
    {
        if (scoopedServiceProvider is null)
            scoopedServiceProvider = ServiceProvider;
        var inbox = scoopedServiceProvider.GetRequiredService<Email.FakeInbox>();
        var userService =
            scoopedServiceProvider.GetRequiredService<IUserService>();

        var validNewUser =
            Mocks.UserMock.CreateNewValidUser(tokenAccessDosador: Mocks.DosadorMock.DefaultIdDosador1.ToString());

        await userService.Create(validNewUser);

        return (validNewUser, inbox);
    }

    protected internal async Task<Model.Dosador.DosadorModel> CreateDosador(IServiceProvider? scoopedServiceProvider = null)
    {
        if (scoopedServiceProvider is null)
            scoopedServiceProvider = ServiceProvider;

        await using var context = scoopedServiceProvider.GetRequiredService<InMemoryDb.AppDbContext>();

        var dosadorToCreate = Mocks.DosadorMock.CreateNewDosador();

        await context.Dosadores.AddAsync(new ModelDb.DosadorDbModel
        {
            IdDosador = dosadorToCreate.IdDosador,
            Nome = dosadorToCreate.Nome,
            PesoMax = dosadorToCreate.PesoMax
        });

        await context.SaveChangesAsync();

        return dosadorToCreate;
    }

    protected class InternalContext : IDisposable
    {
        private static Context.TestContext _context = new();
        public Application.Security.CurrentClaim ClaimModel { get; }

        public InternalContext(Application.Security.CurrentClaim claim)
        {
            ClaimModel = claim;
            _context.ClaimContext = claim;
        }

        public void Dispose()
        {
            _context.ClaimContext = null;
        }
    }

    private class TestHost : Hosts.DefaultHost
    {
        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection serviceCollection)
            => serviceCollection.AddDbContext<InMemoryDb.AppDbContext>()
                .AddScoped<UoW.MemorySession>()
                .AddScoped<UoW.IMemorySession>(serviceProvider => serviceProvider.GetRequiredService<UoW.MemorySession>())
                .AddScoped<Application.UoW.IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<UoW.MemorySession>())

                .AddAutoMapper(config => config.AddProfile(typeof(Profiles.DbRepositoryProfile)))

                .AddSingleton<Context.TestContext>()
                .AddSingleton<Application.Security.ContextClaim>(serviceProvider => serviceProvider.GetRequiredService<Context.TestContext>())

                .AddScoped<Email.FakeInbox>()
                .AddScoped<Application.Email.IUserEmail, Email.UserEmail>()

                .AddScoped<Application.Repositories.IUsuarioDosadorRepository, Repositories.UsuarioDosadorRepository>()
                .AddScoped<Application.Repositories.IUserRepository, Repositories.UserRepository>()
                .AddScoped<Application.Repositories.IDosadorRepository, Repositories.DosadorRepository>()
                .AddScoped<Application.Repositories.IAgendamentoRepository, Repositories.AgendamentoRepository>()

                .AddScoped<Application.Services.Interfaces.IUserService, Application.Services.Implementation.UserService>()
                .AddScoped<Application.Services.Interfaces.IAgendamentoService, Application.Services.Implementation.AgendamentoService>()
                .AddScoped<Application.Services.Interfaces.IDosadorService, Application.Services.Implementation.DosadorService>();

        protected override void AfterCreated(IServiceProvider provider)
        {
            base.AfterCreated(provider);
            var appDbContext = provider.GetRequiredService<InMemoryDb.AppDbContext>();
            InMemoryDb.SeedAppDbContext.AddDosadores(appDbContext).GetAwaiter().GetResult();
        }
    }
}