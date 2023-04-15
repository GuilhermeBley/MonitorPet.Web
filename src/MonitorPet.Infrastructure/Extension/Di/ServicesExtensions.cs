using Microsoft.Extensions.DependencyInjection;

namespace MonitorPet.Infrastructure.Extension.Di;

public static class ServicesExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
        => serviceCollection
        .AddEmails()
        .AddMapper()
        .AddRepositories()
        .AddServices()
        .AddMySql();

    private static IServiceCollection AddEmails(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<Email.Client.EmailClient>()
            .AddScoped<Application.Email.IUserEmail, Infrastructure.Email.Emails.UserEmail>();

    private static IServiceCollection AddMapper(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddAutoMapper(cfg => 
            {
            });

    private static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<Application.Repositories.IDosadorRepository, Infrastructure.Repositories.DosadorRepository>()
            .AddScoped<Application.Repositories.IUserRepository, Infrastructure.Repositories.UserRepository>()
            .AddScoped<Application.Repositories.IUsuarioDosadorRepository, Infrastructure.Repositories.UsuarioDosadorRepository>();

    private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<Application.Services.Interfaces.IUserService, Application.Services.Implementation.UserService>()
            .AddScoped<Application.Services.Interfaces.IDosadorService, Application.Services.Implementation.DosadorService>();


    private static IServiceCollection AddMySql(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<Infrastructure.UoW.MySqlSession>()
            .AddScoped<Infrastructure.UoW.IDbSession>(x => x.GetRequiredService<Infrastructure.UoW.MySqlSession>())
            .AddScoped<Application.UoW.IUnitOfWork>(x => x.GetRequiredService<Infrastructure.UoW.MySqlSession>());
}