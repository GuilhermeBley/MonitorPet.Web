namespace MonitorPet.Core.Exceptions;

/// <summary>
/// Represets a exception with status code <see cref="System.Net.HttpStatusCode.BadRequest"/>
/// </summary>
public sealed class CommonCoreException : CoreException
{
    public const string DefaultMessage = "BadRequest";
    public override int StatusCode => (int)System.Net.HttpStatusCode.BadRequest;
    public CommonCoreException(string? message = DefaultMessage) : base(message)
    {
    }
}