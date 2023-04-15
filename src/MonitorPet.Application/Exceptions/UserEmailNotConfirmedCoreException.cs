using MonitorPet.Core.Exceptions;

namespace MonitorPet.Application.Exceptions;

public class UserEmailNotConfirmedCoreException : CoreException
{
    public const string DefaultMessageUserEmailNotConfirmed = "Usuário com e-mail não confirmado.";

    public override int StatusCode => (int)System.Net.HttpStatusCode.Locked;
    public string Login { get; }

    public UserEmailNotConfirmedCoreException(string login)
        : base(DefaultMessageUserEmailNotConfirmed)
        => Login = login;
}