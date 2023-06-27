using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feirapp.Tests.Helpers;

public class DockerComposeTestBase : IDisposable
{
    private readonly IContainerService _container;
    private readonly IList<IVolumeService>? _volumes;

    public DockerComposeTestBase()
    {
        var hosts = new Hosts().Discover();
        _volumes = hosts.FirstOrDefault()!.GetVolumes();

        _container = new Builder()
            .UseContainer()
            .UseImage("mongo:latest")
            .ExposePort(27017, 27017)
            .WaitForPort("27017/tcp", 3000)
            .WithName("integrationTestsContainer")
            .ReuseIfExists()
            .Build()
            .Start();
    }

    public void Dispose()
    {
        _container.Dispose();
        foreach (var volumeService in _container.GetVolumes())
        {
            volumeService.Remove();
        }
    }
}