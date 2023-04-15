namespace MonitorPet.Core.Exceptions;

/// <summary>
/// Represets a exception with status code <see cref="System.Net.HttpStatusCode.Conflict"/>
/// </summary>
public sealed class ConflictCoreException : CoreException
{
    public const string DefaultMessage = "Conflict";
    public override int StatusCode => (int)System.Net.HttpStatusCode.Conflict;
    public ConflictCoreException(string? message = DefaultMessage) : base(message)
    {
    }
}