using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common;
using SchoolSystem.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SchoolSystem.Core.Common.BaseClasses;


namespace SchoolSystem.Infrastracture.Common.BaseClasses;

public class BaseRepository<TModel, Tid> : IBaseRepository<TModel, Tid> where TModel : BaseModel<Tid>
{
    protected readonly ApplicationDbContext _dbContext;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TModel>> GetAll()
    {
        var data = await _dbContext.Set<TModel>().ToListAsync();

        return data;
    }

    public virtual async Task<PaginatedData<TModel>> GetPaginatedData(int pageNumber, int pageSize)
    {
        var query = _dbContext.Set<TModel>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var data = await query.ToListAsync();
        var totalCount = await _dbContext.Set<TModel>().CountAsync();

        return new PaginatedData<TModel>(data, totalCount);
    }

    public async Task<TModel> GetById(Tid id)
    {
        var data = await _dbContext.Set<TModel>().FindAsync(id);
        if (data == null)
            throw new NotFoundException("No data found");
        return data;
    }

    public async Task<IEnumerable<TModel>> Find(Expression<Func<TModel, bool>> predicate)
    {
        return await _dbContext.Set<TModel>().Where(predicate).ToListAsync();
    }

    public async Task<TModel> Create(TModel model)
    {
        await _dbContext.Set<TModel>().AddAsync(model);
        await _dbContext.SaveChangesAsync();
        return model;
    }

    public async Task CreateRange(List<TModel> model)
    {
        await _dbContext.Set<TModel>().AddRangeAsync(model);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(TModel model)
    {
        _dbContext.Set<TModel>().Update(model);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(TModel model)
    {
        _dbContext.Set<TModel>().Remove(model);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveChangeAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
