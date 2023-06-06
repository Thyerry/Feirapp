using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Extensions;
using Ductus.FluentDocker.Services;
using System;
using Xunit;

namespace Feirapp.UnitTests.Fixtures
{
    public class DockerComposeTestBase : IDisposable
    {
        [Fact]
        public void docker()
        {
            var ok = "ok";
            using (var container = new Builder()
                .UseContainer()
                .UseImage("mongo:latest")
                .ExposePort(27019)
                .WaitForPort("27019/tcp", 3000)
                .Build()
                .Start())
            {
                var config = container.GetConfiguration(true);
                Assert.Equal(ServiceRunningState.Running, config.State.ToServiceState());
            }
        }

        public void Dispose()
        {
            Console.WriteLine("YEAH, I'M THINKING DISPOSED 🤬🤬🤬");
        }
    }
}