using SchoolSystem.IntegrationTests.Common;

namespace SchoolSystem.IntegrationTests.Infrastructure;

[CollectionDefinition("RepositoryIntegrationTest")]
public class RepositoryIntegrationTestCollection : ICollectionFixture<ContainerizedDatabaseFixture>
{
}
