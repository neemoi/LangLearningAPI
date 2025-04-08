using System.Net;

namespace LangLearningAPI.Exceptions
{
    public class ValidationException : ApiException
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException(
            string message = "Validation failed",
            string? errorCode = "VALIDATION_ERROR",
            Dictionary<string, string[]>? errors = null)
            : base((int)HttpStatusCode.BadRequest, message, errorCode)
        {
            Errors = errors ?? new Dictionary<string, string[]>();
        }
    }
}
