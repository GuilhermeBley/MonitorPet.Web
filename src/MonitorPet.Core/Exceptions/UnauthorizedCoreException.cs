namespace MonitorPet.Core.Exceptions;

/// <summary>
/// Represets a exception with status code <see cref="System.Net.HttpStatusCode.Unauthorized"/>
/// </summary>
public class UnauthorizedCoreException : CoreException
{
    public const string DefaultMessage = "Unauthorized";
    public override int StatusCode => (int)System.Net.HttpStatusCode.Unauthorized;
    public UnauthorizedCoreException(string? message = DefaultMessage) : base(message)
    {
    }
}