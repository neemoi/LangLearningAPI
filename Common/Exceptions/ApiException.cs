using System;
using System.Net;

namespace LangLearningAPI.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public string? ErrorCode { get; }

        public ApiException(int statusCode, string message, string? errorCode = null, Exception? innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
}