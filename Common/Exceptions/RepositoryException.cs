public class RepositoryException : Exception
{
    public string ErrorCode { get; }

    public RepositoryException(string message, string errorCode, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}