using SchoolSystem.Core.Common.BaseClasses;

namespace SchoolSystem.Core.Exceptions.Domain;

public class EmailAlreadyExistsDomainException : BaseDomainException
{
    public EmailAlreadyExistsDomainException(string message) : base(message) { }

    public EmailAlreadyExistsDomainException(string message, Exception? innerException) : base(message, innerException) { }
}
