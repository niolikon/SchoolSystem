using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common;
using SchoolSystem.Core.Exceptions.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SchoolSystem.Infrastracture.Common.BaseClasses;


public abstract class BaseRepository<TModel, Tid> : IBaseRepository<TModel, Tid> where TModel : BaseModel<Tid>
{
    protected readonly ApplicationDbContext _dbContext;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TModel>> GetAll()
    {
        try
        {
            var data = await _dbContext.Set<TModel>().ToListAsync();
            return data;
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException("Failed to retrieve all records.", ex);
        }
    }


    public abstract Task<IEnumerable<TModel>> GetAllWithDetails();

    public virtual async Task<PaginatedData<TModel>> GetPaginatedData(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            throw new InvalidQueryDomainException("Page number and page size must be greater than zero.");

        try
        {
            var query = _dbContext.Set<TModel>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

            var data = await query.ToListAsync();
            var totalCount = await _dbContext.Set<TModel>().CountAsync();

            return new PaginatedData<TModel>(data, totalCount);
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException("Failed to retrieve paginated data.", ex);
        }
        
    }

    public async Task<TModel> GetById(Tid id)
    {
        try
        {
            return await _dbContext.Set<TModel>().FindAsync(id) ??
                throw new EntityNotFoundDomainException(typeof(TModel).ToString(), id);
        }
        catch (EntityNotFoundDomainException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException($"Failed to retrieve {typeof(TModel).Name} by ID.", ex);
        }
    }

    public abstract Task<TModel> GetByIdWithDetails(Tid id);

    public async Task<IEnumerable<TModel>> Find(Expression<Func<TModel, bool>> predicate)
    {
        try
        {
            return await _dbContext.Set<TModel>().Where(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidQueryDomainException("Failed to execute the query with the given predicate.", ex);
        }
    }

    public async Task<TModel> Create(TModel model)
    {
        try
        {
            await _dbContext.Set<TModel>().AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("unique constraint") ?? false)
        {
            throw new EntityAlreadyExistsDomainException(typeof(TModel).Name, "Unique constraint violation.");
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException($"Failed to create {typeof(TModel).Name}.", ex);
        }
    }

    public async Task CreateRange(List<TModel> model)
    {
        try
        {
            await _dbContext.Set<TModel>().AddRangeAsync(model);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException("Failed to create multiple records.", ex);
        }
    }

    public async Task Update(TModel model)
    {
        try
        {
            _dbContext.Set<TModel>().Update(model);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException($"Failed to update {typeof(TModel).Name}.", ex);
        }
    }

    public async Task Delete(TModel model)
    {
        try
        {
            _dbContext.Set<TModel>().Remove(model);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("foreign key constraint") ?? false)
        {
            throw new ForeignKeyViolationDomainException(typeof(TModel).Name);
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException($"Failed to delete {typeof(TModel).Name}.", ex);
        }
    }

    public async Task SaveChangeAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException("Failed to save changes to the database.", ex);
        }
    }
}
