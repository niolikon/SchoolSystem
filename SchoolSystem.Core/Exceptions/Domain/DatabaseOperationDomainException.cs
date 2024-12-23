using SchoolSystem.Core.Common.BaseClasses;

namespace SchoolSystem.Core.Exceptions.Domain;

public class DatabaseOperationDomainException : BaseDomainException
{
    public DatabaseOperationDomainException(string message) : base(message) { }

    public DatabaseOperationDomainException(string message, Exception? innerException) : base(message, innerException) { }
}
