using System.Security.Claims;
using MonitorPet.Application.Model.User;

namespace MonitorPet.Application.Email;

public interface IUserEmail
{
    Task SendEmailConfirmPassword(UserModel userToConfirm, Claim[] claims);
    Task SendEmailChangePassword(UserModel userToConfirm, Claim[] claims);
}