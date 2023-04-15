namespace MonitorPet.Core.Exceptions;

/// <summary>
/// Represets a exception with status code <see cref="System.Net.HttpStatusCode.NotFound"/>
/// </summary>
public class NotFoundCoreException : CoreException
{
    public const string DefaultMessage = "NotFound";
    public override int StatusCode => (int)System.Net.HttpStatusCode.NotFound;
    public NotFoundCoreException(string? message = DefaultMessage) : base(message)
    {
    }
}