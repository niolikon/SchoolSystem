using SchoolSystem.Core.Common.BaseClasses;

namespace SchoolSystem.Core.Exceptions.Domain;


public class EntityNotFoundDomainException : BaseDomainException
{
    public EntityNotFoundDomainException(string entityName, object key) : base($"{entityName} with key {key} was not found.") { }
}
