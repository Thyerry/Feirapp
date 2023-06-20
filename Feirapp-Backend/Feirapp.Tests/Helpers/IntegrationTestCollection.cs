using Xunit;

namespace Feirapp.Tests.Helpers;

[CollectionDefinition("Database Integration Test")]
public class IntegrationTestCollection : ICollectionFixture<DockerComposeTestBase>
{
}