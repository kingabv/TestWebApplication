using System.Runtime.Serialization;
using TestWebApplication.Models;

namespace TestWebApplication.Exceptions;

/// <summary>
/// Error specific exceptions
/// </summary>
[Serializable]
public class PictureItemException : Exception
{
    /// <summary>
    /// Error codes that map to handled error use-cases inside API
    /// </summary>
    public ErrorCode ErrorCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PictureItemException"/> class with error code
    /// </summary>
    /// <param name="errorCode">Error codes that map to handled error use-cases inside API</param>
    public PictureItemException(ErrorCode errorCode)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    ///  Initializes a new instance of the <see cref="PictureItemException"/> class with error code and message
    /// </summary>
    /// <param name="errorCode">Error codes that map to handled error use-cases inside API</param>
    /// <param name="message">The massage that describe the error</param>
    public PictureItemException(ErrorCode errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    ///  Initializes a new instance of the <see cref="PictureItemException"/> class with error code, message and the inner exception
    /// </summary>
    /// <param name="errorCode">Error codes that map to handled error use-cases inside API</param>
    /// <param name="message">The massage that describe the error</param>
    /// <param name="innerException">The inner exception</param>
    public PictureItemException(ErrorCode errorCode, string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }

    /// <inheritdoc />
    public PictureItemException(string message) : base(message)
    {
    }

    /// <inheritdoc />
    public PictureItemException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <inheritdoc />
    protected PictureItemException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}