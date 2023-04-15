using System.Runtime.Serialization;

namespace MonitorPet.Core.Exceptions;

/// <summary>
/// Exceptions in Core
/// </summary>
public abstract class CoreException : Exception, ICoreException
{
    /// <summary>
    /// Status code
    /// </summary>
    public abstract int StatusCode { get; }

    /// <summary>
    /// Source Core
    /// </summary>
    public override string? Source => "Smartec.Web.Core";

    /// <summary>
    /// Status code + message
    /// </summary>
    public override string Message => $"{base.Message}";

    protected CoreException()
    {
    }

    protected CoreException(string? message) : base(message)
    {
    }

    protected CoreException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    protected CoreException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public override string ToString()
    {
        return $"{StatusCode}|{Message}";
    }

    /// <summary>
    /// Check if obj is a <see cref="ICoreException"/>
    /// </summary>
    /// <param name="obj">object to check</param>
    /// <returns>true : is a <see cref="ICoreException"/>, otherwise don't</returns>
    public static bool IsCoreException(object? obj)
    {
        return TryGetCoreException(obj, out ICoreException? coreException);
    }

    /// <summary>
    /// Check if obj is a <see cref="ICoreException"/> and return in <paramref name="coreException"/> the value
    /// </summary>
    /// <param name="obj">object to check</param>
    /// <returns>true : is a <see cref="ICoreException"/>, otherwise don't</returns>
    public static bool TryGetCoreException(object? obj, out ICoreException? coreException)
    {
        coreException = null;

        if (obj is null)
            return false;

        coreException = obj as ICoreException;

        if (coreException is null)
            return false;

        return true;
    }

    /// <summary>
    /// Creates new CoreException
    /// </summary>
    /// <param name="statusCode">400 to 499</param>
    /// <param name="message">error message</param>
    /// <returns>new CoreException</returns>
    /// <inheritdoc cref="HttpCoreExpception.HttpCoreExpception(System.Net.HttpStatusCode, string?)" path="/exception"/>
    public static CoreException CreateWithHttpStatusCode(System.Net.HttpStatusCode statusCode, string? message = null)
    {
        return new HttpCoreExpception(statusCode, message);
    }

    /// <summary>
    /// private use
    /// </summary>
    protected class HttpCoreExpception : CoreException
    {
        private System.Net.HttpStatusCode _httpStatusCode { get; }
        private int _statusCode => (int)_httpStatusCode;
        public System.Net.HttpStatusCode HttpStatusCode => _httpStatusCode;
        public override int StatusCode => _statusCode;

        /// <summary>
        /// Creates new instance with http status code
        /// </summary>
        /// <param name="statusCode">400 to 499</param>
        /// <param name="message">error message</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public HttpCoreExpception(System.Net.HttpStatusCode statusCode, string? message) : base(message)
        {
            ThrowExceptionIfIsInvalidHttpStatusCode(statusCode);
            _httpStatusCode = statusCode;
        }

        /// <summary>
        /// Check if is a valid code
        /// </summary>
        /// <param name="statusCode">400 to 499</param>
        /// <returns>true : valid, false: invalid</returns>
        public static bool IsValidHttpStatusCode(System.Net.HttpStatusCode statusCode)
        {
            var intStatusCode = (int)statusCode;
            if (intStatusCode > 499 || intStatusCode < 400)
                return false;

            return true;
        }

        /// <summary>
        /// Throw exception if is invalid http status code
        /// </summary>
        /// <param name="statusCode">400 to 499</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static void ThrowExceptionIfIsInvalidHttpStatusCode(System.Net.HttpStatusCode statusCode)
        {
            if (!IsValidHttpStatusCode(statusCode))
                throw new IndexOutOfRangeException("statusCode must have range of 400 to 499.");
        }
    }
}
