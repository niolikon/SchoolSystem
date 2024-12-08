using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common;
using SchoolSystem.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace SchoolSystem.Infrastracture.Common.BaseClasses;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        var data = await _dbContext.Set<T>()
            .AsNoTracking()
            .ToListAsync();

        return data;
    }

    public virtual async Task<PaginatedData<T>> GetPaginatedData(int pageNumber, int pageSize)
    {
        var query = _dbContext.Set<T>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();

        var data = await query.ToListAsync();
        var totalCount = await _dbContext.Set<T>().CountAsync();

        return new PaginatedData<T>(data, totalCount);
    }

    public async Task<T> GetById<Tid>(Tid id)
    {
        var data = await _dbContext.Set<T>().FindAsync(id);
        if (data == null)
            throw new NotFoundException("No data found");
        return data;
    }

    public async Task<bool> IsExists<Tvalue>(string key, Tvalue value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, key);
        var constant = Expression.Constant(value);
        var equality = Expression.Equal(property, constant);
        var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

        return await _dbContext.Set<T>().AnyAsync(lambda);
    }

    public async Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, key);
        var constant = Expression.Constant(value);
        var equality = Expression.Equal(property, constant);

        var idProperty = Expression.Property(parameter, "Id");
        var idEquality = Expression.NotEqual(idProperty, Expression.Constant(id));

        var combinedExpression = Expression.AndAlso(equality, idEquality);
        var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);

        return await _dbContext.Set<T>().AnyAsync(lambda);
    }

    public async Task<T> Create(T model)
    {
        await _dbContext.Set<T>().AddAsync(model);
        await _dbContext.SaveChangesAsync();
        return model;
    }

    public async Task CreateRange(List<T> model)
    {
        await _dbContext.Set<T>().AddRangeAsync(model);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(T model)
    {
        _dbContext.Set<T>().Update(model);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(T model)
    {
        _dbContext.Set<T>().Remove(model);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveChangeAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>().Where(predicate).ToListAsync();
    }
}
