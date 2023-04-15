namespace MonitorPet.Core.Exceptions;

/// <summary>
/// Core exception
/// </summary>
public interface ICoreException
{
    /// <summary>
    /// Status code
    /// </summary>
    int StatusCode { get; }

    /// <summary>
    /// Message
    /// </summary>
    string Message { get; }
}