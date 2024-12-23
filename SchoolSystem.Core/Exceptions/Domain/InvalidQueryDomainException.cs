using SchoolSystem.Core.Common.BaseClasses;

namespace SchoolSystem.Core.Exceptions.Domain;


public class InvalidQueryDomainException : BaseDomainException
{
    public InvalidQueryDomainException(string message) : base(message) { }

    public InvalidQueryDomainException(string message, Exception? innerException) : base(message, innerException) { }
}