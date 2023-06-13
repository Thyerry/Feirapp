using Xunit;

namespace Feirapp.UnitTests.Helpers;

[CollectionDefinition("Database Integration Test")]
public class IntegrationTestCollection : ICollectionFixture<DockerComposeTestBase>
{
}