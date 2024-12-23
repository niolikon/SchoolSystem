using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Infrastracture.Common;
using SchoolSystem.IntegrationTests.Common.BaseInterface;
using Testcontainers.MsSql;

namespace SchoolSystem.IntegrationTests.Common;

public class ContainerizedDatabaseFixture : IAsyncLifetime, IDatabaseClassFixture<ApplicationDbContext>
{
    private readonly MsSqlContainer container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-CU10-ubuntu-22.04")
            .Build();

    public IDatabaseContainer Container => container;

    public string ConnectionString => container.GetConnectionString();

    public ApplicationDbContext Context { get; private set; }


    public async Task InitializeAsync()
    {
        await Container.StartAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        Context = new ApplicationDbContext(options);

        await Context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
       await Container.DisposeAsync();
    }
}
