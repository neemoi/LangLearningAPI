using LangLearningAPI.Exceptions;
using System.Net;

public class ForbiddenException : ApiException
{
    public ForbiddenException(
        string message = "Forbidden",
        string? errorCode = "FORBIDDEN")
        : base((int)HttpStatusCode.Forbidden, message, errorCode)
    {
    }
}