using SchoolSystem.Core.Common.BaseClasses;
using System.Linq.Expressions;

namespace SchoolSystem.Core.Common.BaseInterfaces;

public interface IBaseRepository<TModel, Tid> where TModel : BaseModel<Tid>
{
    Task<IEnumerable<TModel>> GetAll();

    Task<IEnumerable<TModel>> GetAllWithDetails();

    Task<PaginatedData<TModel>> GetPaginatedData(int pageNumber, int pageSize);

    Task<TModel> GetById(Tid id);

    Task<TModel> GetByIdWithDetails(Tid id);

    Task<IEnumerable<TModel>> Find(Expression<Func<TModel, bool>> predicate);

    Task<TModel> Create(TModel model);

    Task CreateRange(List<TModel> model);

    Task Update(TModel model);

    Task Delete(TModel model);

    Task SaveChangeAsync();
}