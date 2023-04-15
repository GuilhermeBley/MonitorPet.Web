using MonitorPet.Core.Exceptions;

namespace MonitorPet.Application.Exceptions;

public class UserEmailNotConfirmedCoreException : CoreException
{
    public const string DefaultMessageUserEmailNotConfirmed = "Usu�rio com e-mail n�o confirmado.";

    public override int StatusCode => (int)System.Net.HttpStatusCode.Locked;
    public string Login { get; }

    public UserEmailNotConfirmedCoreException(string login)
        : base(DefaultMessageUserEmailNotConfirmed)
        => Login = login;
}