using System.Net;

namespace LangLearningAPI.Exceptions
{
    public class ConflictException : ApiException
    {
        public ConflictException(
            string message = "Conflict",
            string? errorCode = "CONFLICT",
            Exception? innerException = null)
            : base((int)HttpStatusCode.Conflict, message, errorCode, innerException)
        {
        }
    }
}