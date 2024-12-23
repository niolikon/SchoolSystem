using SchoolSystem.Core.Common.BaseClasses;

namespace SchoolSystem.Core.Exceptions.Domain;


public class ForeignKeyViolationDomainException : BaseDomainException
{
    public ForeignKeyViolationDomainException(string entityName) : base($"{entityName} foreign key constraint violation.") { }
}
