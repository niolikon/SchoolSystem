using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolSystem.Infrastracture.Common;
using SchoolSystem.IntegrationTests.Common.BaseInterface;

namespace SchoolSystem.IntegrationTests.Common;

public class ContainerizedWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private readonly IDatabaseClassFixture<ApplicationDbContext> _databaseClassFixture;

    public ContainerizedWebApplicationFactory(IDatabaseClassFixture<ApplicationDbContext> databaseClassFixture)
    {
        _databaseClassFixture = databaseClassFixture;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer(_databaseClassFixture.ConnectionString)
                    .Options;

            services.AddSingleton(options);
            services.AddSingleton<ApplicationDbContext>();
        });
    } 
}
