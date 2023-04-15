using System.Security.Claims;
using MonitorPet.Application.Email;
using MonitorPet.Application.Model.User;
using MonitorPet.Infrastructure.Email.Client;
using MonitorPet.Infrastructure.Factories;

namespace MonitorPet.Infrastructure.Email.Emails;

internal class UserEmail : IUserEmail
{
    private readonly EmailClient _client;
    private readonly IUrlUserEmailFactory _urlFactory;
    private readonly IUrlChangePasswordFactory _urlChangePassword;

    public UserEmail(Client.EmailClient client, 
        Factories.IUrlUserEmailFactory urlFactory, IUrlChangePasswordFactory urlChangePassword)
    {
        _client = client;
        _urlFactory = urlFactory;
        _urlChangePassword = urlChangePassword;
    }

    public async Task SendEmailChangePassword(UserModel userToConfirm, Claim[] claims)
    {
        var urlConfirmAccess = await _urlChangePassword.Create(userToConfirm, claims);
        var bodyHtml =
            Templates.UserEmailTemplate.MakeTemaplatChangePassword(urlConfirmAccess.AbsoluteUri);

        await _client.SendHtmlMessageWithDefaultFrom(
            "Alteração de senha MonitorPet.",
            bodyHtml,
            new string[] { userToConfirm.Email });
    }

    public async Task SendEmailConfirmPassword(UserModel userToConfirm, Claim[] claims)
    {
        var urlConfirmAccess = await _urlFactory.Create(userToConfirm, claims);
        var bodyHtml = 
            Templates.UserEmailTemplate.MakeTemaplateConfirmAccount(userToConfirm.Email, urlConfirmAccess.AbsoluteUri);

        await _client.SendHtmlMessageWithDefaultFrom(
            "Confirmação de conta MonitorPet.",
            bodyHtml,
            new string[] { userToConfirm.Email });
    }
}