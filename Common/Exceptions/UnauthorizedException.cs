using LangLearningAPI.Exceptions;
using System.Net;

public class UnauthorizedException : ApiException
{
    public UnauthorizedException(
        string message = "Unauthorized",
        string? errorCode = "UNAUTHORIZED")
        : base((int)HttpStatusCode.Unauthorized, message, errorCode)
    {
    }
}