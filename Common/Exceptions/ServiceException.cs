namespace Common.Exceptions
{
    public class ServiceException : Exception
    {
        public string ErrorCode { get; }

        public ServiceException(string message) : base(message) { }

        public ServiceException(string message, Exception innerException)
            : base(message, innerException) { }

        public ServiceException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public ServiceException(string message, string errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
