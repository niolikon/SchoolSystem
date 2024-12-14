using System.Linq.Expressions;

namespace SchoolSystem.Core.Common.BaseInterfaces;

public interface IBaseService<DtoType> where DtoType : class
{
    Task<IEnumerable<DtoType>> GetAll();

    Task<PaginatedData<DtoType>> GetAllPaginated(int pageNumber, int pageSize);

    Task<DtoType> GetSingle(int id);

    Task<DtoType> Create(DtoType model);

    Task Update(int id, DtoType model);

    Task Delete(int id);
}
