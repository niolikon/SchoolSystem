namespace SchoolSystem.Core.Common.BaseClasses;

public class BaseRestException : Exception
{
    public int StatusCode { get; }
    public object Error { get; }

    public BaseRestException(int statusCode, object error)
    {
        StatusCode = statusCode;
        Error = error;
    }
}
