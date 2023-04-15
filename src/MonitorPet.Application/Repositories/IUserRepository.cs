using MonitorPet.Application.Model.User;
using MonitorPet.Core.Entity;

namespace MonitorPet.Application.Repositories;

public interface IUserRepository : IRepositoryBase<User, UserModel, int>
{
    Task<UserModel?> GetByEmailOrDefault(string email);
    Task<UserModel?> UpdatePasswordByIdOrDefault(int id, User entity);
    Task<UserModel?> UpdateAccessAccountFailed(int id, User entity);
}