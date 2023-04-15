namespace MonitorPet.Infrastructure.Email.Templates;

internal class UserEmailTemplate
{
    public static string MakeTemaplateConfirmAccount(string nickName, string url)
        => @"<h1>Confirmação de acesso MonitorPet</h1>
        <p>Bem Vindo {nickName}! Para confirmar seu e-mail acesse o Link de <a href='{url}'>confirmação de cadastro</a>.</p>"
        .Replace("{nickName}", nickName)
        .Replace("{url}", url);
    public static string MakeTemaplatChangePassword(string url)
        => @"<h1>Alteração de senha MonitorPet</h1>
        <p>Para alterar sua senha acesse o Link de <a href='{url}'>alteração de senha</a>.</p>"
        .Replace("{url}", url);
}