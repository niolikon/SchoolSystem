using System.Linq.Expressions;

namespace SchoolSystem.Core.Common.BaseInterfaces;

public interface IBaseRepository<ModelType> where ModelType : class
{
    Task<IEnumerable<ModelType>> GetAll();

    Task<PaginatedData<ModelType>> GetPaginatedData(int pageNumber, int pageSize);

    Task<ModelType> GetById<Tid>(Tid id);

    Task<IEnumerable<ModelType>> Find(Expression<Func<ModelType, bool>> predicate);

    Task<ModelType> Create(ModelType model);

    Task CreateRange(List<ModelType> model);

    Task Update(ModelType model);

    Task Delete(ModelType model);

    Task SaveChangeAsync();
}