using System.Security.Claims;
using MonitorPet.Application.Email;
using MonitorPet.Application.Model.User;

namespace MonitorPet.Application.Tests.Email;

internal class UserEmail : IUserEmail
{
    private readonly FakeInbox _fakeInbox;

    public UserEmail(FakeInbox fakeInbox)
    {
        _fakeInbox = fakeInbox;
    }

    public async Task SendEmailChangePassword(UserModel userToConfirm, Claim[] claims)
    {
        _fakeInbox.AddToLastInbox(userToConfirm, claims);
        await Task.CompletedTask;
    }

    public async Task SendEmailConfirmPassword(UserModel email, Claim[] claims)
    {
        _fakeInbox.AddToLastInbox(email, claims);
        await Task.CompletedTask;
    }
}