using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastracture.Common;

namespace SchoolSystem.IntegrationTests.Common;

public class DatabasePreSeederPostCleaner : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly object[] _dataToPreload;

    public DatabasePreSeederPostCleaner(ApplicationDbContext context, object[] data) 
    {
        _context = context;
        _dataToPreload = data;

        PopulateDatabase();
    }

    private void DisableForeignKeys()
    {
        var entityTypes = _context.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            var tableName = entityType.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
            {
                var schema = entityType.GetSchema() ?? "dbo";
                _context.Database.ExecuteSqlRaw($"ALTER TABLE [{schema}].[{tableName}] NOCHECK CONSTRAINT ALL;");
            }
        }
    }

    private void EnableForeignKeys()
    {
        var entityTypes = _context.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            var tableName = entityType.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
            {
                var schema = entityType.GetSchema() ?? "dbo";
                _context.Database.ExecuteSqlRaw($"ALTER TABLE [{schema}].[{tableName}] WITH CHECK CHECK CONSTRAINT ALL;");
            }
        }
    }

    private void PopulateDatabase()
    {
        DisableForeignKeys();

        foreach (var entity in _dataToPreload)
        {
            _context.Add(entity);
        }
        _context.SaveChanges();

        EnableForeignKeys();
    }

    private void ClearDatabase()
    {
        var entityTypes = _context.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            var tableName = entityType.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
            {
                var schema = entityType.GetSchema() ?? "dbo";
                _context.Database.ExecuteSqlRaw($"DELETE FROM [{schema}].[{tableName}];");
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        DisableForeignKeys();

        ClearDatabase();

        EnableForeignKeys();
    }
}
