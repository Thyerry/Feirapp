using Xunit;

namespace Feirapp.Tests.Helpers;

[CollectionDefinition("integration-tests-collection")]
public class IntegrationTestCollection : ICollectionFixture<DockerComposeTestBase>
{
}