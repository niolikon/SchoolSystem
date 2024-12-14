using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace SchoolSystem.IntegrationTests.Common;

public class DatabaseRollbackTransactional : IDisposable
{
    private readonly DbContext _dbContext;
    private readonly IDbContextTransaction _transaction;

    public DatabaseRollbackTransactional(DbContext dbContext)
    {
        _dbContext = dbContext;
        _transaction = _dbContext.Database.BeginTransaction();
    }

    public void Dispose()
    {
        _transaction.Rollback();
        _transaction.Dispose();
    }
}
