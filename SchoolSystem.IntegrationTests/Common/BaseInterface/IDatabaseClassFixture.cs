using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;

namespace SchoolSystem.IntegrationTests.Common.BaseInterface;

public interface IDatabaseClassFixture<TDbContext> where TDbContext : DbContext
{
    IDatabaseContainer Container { get; }
    string ConnectionString { get; }
    TDbContext Context { get; }
}
