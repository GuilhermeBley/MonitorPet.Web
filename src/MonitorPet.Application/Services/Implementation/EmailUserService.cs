using MonitorPet.Application.Model.Email;
using MonitorPet.Application.Repositories;
using MonitorPet.Application.Security;
using MonitorPet.Application.Services.Interfaces;
using MonitorPet.Application.UoW;
using MonitorPet.Core.Entity;

namespace MonitorPet.Application.Services.Implementation;

public class EmailUserService : IEmailUserService
{
    private readonly ContextClaim _contextClaim;
    private readonly IEmailTypeRepository _emailTypeRepository;
    private readonly IRoleEmailUserRepository _roleEmailUserRepository;
    private readonly IUnitOfWork _uoW;

    public EmailUserService(
        ContextClaim contextClaim, 
        IRoleEmailUserRepository roleEmailUserRepository,
        IEmailTypeRepository emailTypeRepository,
        IUnitOfWork uoW)
    {
        _contextClaim = contextClaim;
        _emailTypeRepository = emailTypeRepository;
        _roleEmailUserRepository = roleEmailUserRepository;
        _uoW = uoW;
    }

    public async Task<IEnumerable<QueryRoleEmailUserModel>> CreateOrUpdate(CreateOrUpdateRoleEmailUserModel config, CancellationToken cancellationToken = default)
    {
        var ctx = await _contextClaim.GetRequiredCurrentClaim();

        ctx.ThrowIfIsntLogged();

        var idUser = ctx.RequiredIdUser;

        using var transaction = await _uoW.BeginTransactionAsync();

        await _roleEmailUserRepository.DeleteByIdUser(idUser);

        var availablesTypes = await _emailTypeRepository.GetAll();

        foreach (var role in config.Roles)
        {
            var availableFound = availablesTypes.FirstOrDefault(
                a => string.Equals(role, a.Type, StringComparison.OrdinalIgnoreCase));

            if (availableFound is null)
                continue;

            var emailType = EmailConfigType.Create(availableFound.Id, availableFound.Type, availableFound.Description);
            var entityToCreate = RoleEmailUser.CreateWithDefaultId(ctx.RequiredIdUser, emailType);

            await _roleEmailUserRepository.Create(entityToCreate);
        }

        var roles = await _roleEmailUserRepository.GetByIdUser(idUser);

        await transaction.SaveChangesAsync();

        return roles;
    }

    public async Task<IEnumerable<EmailTypeModel>> GetAvailables(CancellationToken cancellationToken = default)
    {
        var ctx = await _contextClaim.GetRequiredCurrentClaim();

        ctx.ThrowIfIsntLogged();

        using var connection = await _uoW.OpenConnectionAsync();

        return await _emailTypeRepository.GetAll();
    }

    public async Task<IEnumerable<QueryRoleEmailUserModel>> GetByIdUser(int idUser, CancellationToken cancellationToken = default)
    {
        var ctx = await _contextClaim.GetRequiredCurrentClaim();

        ctx.ThrowIfIsntLogged();

        if (idUser != ctx.IdUser)
            throw new Core.Exceptions.ForbiddenCoreException();

        using var connection = await _uoW.OpenConnectionAsync();

        return await _roleEmailUserRepository.GetByIdUser(idUser);
    }
}
