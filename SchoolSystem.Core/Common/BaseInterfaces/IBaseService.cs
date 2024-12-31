using System.Linq.Expressions;

namespace SchoolSystem.Core.Common.BaseInterfaces;

public interface IBaseService<TDetailsDto, TCreateDto, TUpdateDto> where TDetailsDto : class where TCreateDto : class where TUpdateDto : class
{
    Task<IEnumerable<TDetailsDto>> GetAll();

    Task<PaginatedData<TDetailsDto>> GetAllPaginated(int pageNumber, int pageSize);

    Task<TDetailsDto> GetSingle(int id);

    Task<TDetailsDto> Create(TCreateDto model);

    Task Update(int id, TUpdateDto model);

    Task Delete(int id);
}
