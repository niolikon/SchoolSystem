using SchoolSystem.IntegrationTests.Common;

namespace SchoolSystem.IntegrationTests.Api;

[CollectionDefinition("ControllerIntegrationTest")]
public class RepositoryIntegrationTestCollection : ICollectionFixture<ContainerizedDatabaseFixture>
{
}
