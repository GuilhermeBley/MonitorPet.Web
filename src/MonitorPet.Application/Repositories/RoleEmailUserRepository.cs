using MonitorPet.Application.Model.Email;
using MonitorPet.Core.Entity;

namespace MonitorPet.Application.Repositories;

public interface RoleEmailUserRepository : IRepositoryBase<RoleEmailUser, RoleEmailUserModel, int>
{
    IEnumerable<QueryRoleEmailUserModel> GetByIdUser(int idUser);
}
