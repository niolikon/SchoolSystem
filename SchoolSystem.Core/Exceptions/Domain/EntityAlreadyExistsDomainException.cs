using SchoolSystem.Core.Common.BaseClasses;

namespace SchoolSystem.Core.Exceptions.Domain;


public class EntityAlreadyExistsDomainException : BaseDomainException
{
    public EntityAlreadyExistsDomainException(string entityName, object key) : base($"{entityName} with key {key} already exists.") { }
}
