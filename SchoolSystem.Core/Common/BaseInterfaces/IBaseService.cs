using System.Linq.Expressions;

namespace SchoolSystem.Core.Common.BaseInterfaces;

public interface IBaseService<TDto> where TDto : class
{
    Task<IEnumerable<TDto>> GetAll();

    Task<PaginatedData<TDto>> GetAllPaginated(int pageNumber, int pageSize);

    Task<TDto> GetSingle(int id);

    Task<TDto> Create(TDto model);

    Task Update(int id, TDto model);

    Task Delete(int id);
}
