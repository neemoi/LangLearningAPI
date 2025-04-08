using System.Net;

namespace LangLearningAPI.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(
            string message = "Not Found",
            string? errorCode = "NOT_FOUND",
            Exception? innerException = null)
            : base((int)HttpStatusCode.NotFound, message, errorCode, innerException)
        {
        }
    }
}