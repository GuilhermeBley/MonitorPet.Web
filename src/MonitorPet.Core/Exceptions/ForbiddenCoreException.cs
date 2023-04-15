namespace MonitorPet.Core.Exceptions;

/// <summary>
/// Represets a exception with status code <see cref="System.Net.HttpStatusCode.Forbidden"/>
/// </summary>
public sealed class ForbiddenCoreException : CoreException
{
    public const string DefaultMessage = "Forbidden";
    public override int StatusCode => (int)System.Net.HttpStatusCode.Forbidden;
    public ForbiddenCoreException(string? message = DefaultMessage) : base(message)
    {
    }
}