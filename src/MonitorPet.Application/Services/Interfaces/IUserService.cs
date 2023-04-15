using System.Security.Claims;
using MonitorPet.Application.Model.Dosador;
using MonitorPet.Application.Model.User;

namespace MonitorPet.Application.Services.Interfaces;

/// <summary>
/// Gerenciamento de usuários, necessita de autenticação os métodos, exceto Login
/// </summary>
public interface IUserService
{
    Task<ResultUserLoginModel> ConfirmEmail(string email);
    Task<ResultUserLoginModel> ConfirmChangePassoword(string email, string newPassword);
    Task Create(CreateUserModel createUserModel);
    Task<QueryUserModel> GetByIdUser(int idUser);
    Task<ResultUserLoginModel> Login(string email, string password);
    Task<Claim[]> LoginDosador(int idUser, Guid idDosador);
    Task SendAgainEmailConfirmAccount(string email);
    Task SendEmailChangePassword(string email);
    Task<QueryUserModel> Update(int idUser, UpdateUserModel updateUserModel);
    Task<QueryUserModel> ChangePassword(int idUser, string oldPassword, string newPassword);
}