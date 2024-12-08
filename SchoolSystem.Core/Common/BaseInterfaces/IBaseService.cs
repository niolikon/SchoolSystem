namespace SchoolSystem.Core.Common.BaseInterfaces;

public interface IBaseService<ViewModelType> where ViewModelType : class
{
    Task<IEnumerable<ViewModelType>> GetAll();
    Task<PaginatedData<ViewModelType>> GetAllPaginated(int pageNumber, int pageSize);
    Task<ViewModelType> GetSingle(int id);
    Task<bool> IsExists(string key, string value);
    Task<bool> IsExistsForUpdate(int id, string key, string value);
    Task<ViewModelType> Create(ViewModelType model);
    Task Update(ViewModelType model);
    Task Delete(int id);
}
