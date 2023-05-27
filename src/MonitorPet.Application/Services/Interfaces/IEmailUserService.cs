using MonitorPet.Application.Model.Email;

namespace MonitorPet.Application.Services.Interfaces;

public interface IEmailUserService
{
    Task<IEnumerable<QueryRoleEmailUserModel>> CreateOrUpdate(CreateOrUpdateRoleEmailUserModel config, CancellationToken cancellationToken = default);
    Task<IEnumerable<QueryRoleEmailUserModel>> GetByIdUser(int idUser, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmailTypeModel>> GetAvailables(CancellationToken cancellationToken = default);
}
