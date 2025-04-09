using LangLearningAPI.Exceptions;
using System.Net;

public class IdentityException : Exception
{
    public string ErrorCode { get; }
    public string[] ErrorDetails { get; }

    public IdentityException(string message, string errorCode, params string[] errorDetails)
        : base(message)
    {
        ErrorCode = errorCode;
        ErrorDetails = errorDetails;
    }
}