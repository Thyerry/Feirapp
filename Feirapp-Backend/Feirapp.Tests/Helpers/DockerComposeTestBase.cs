using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using System;
using System.Diagnostics;

namespace Feirapp.Tests.Helpers;

public class DockerComposeTestBase : IDisposable
{
    private readonly IContainerService _container;

    public DockerComposeTestBase()
    {
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
        Debug.WriteLine("YEAH, I'M THINKING I'M DISPOSED 😡");
        Debug.Write(@"
        ⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣀⣀⣀⣀⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀
        ⠀⠀⠀⠀⠀⢀⣴⣾⣿⣿⣿⣿⣿⣿⣿⣿⣷⣦⡀⠀⠀⠀⠀⠀
        ⠀⠀⠀⢀⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣦⠀⠀⠀⠀
        ⠀⠀⢀⣾⣿⣿⣿⣿⡿⠿⠿⣿⣿⠿⠿⢿⣿⣿⣿⣿⣧⡀⠀⠀
        ⠀⠀⣾⣿⣿⣿⣿⠏⠀⠀⠀⠈⠁⠀⠀⠀⠹⣿⣿⣿⣿⣧⠀⠀
        ⠀⣸⣿⣿⣿⣿⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⣿⣿⣿⣿⡆⠀
        ⠀⣿⣿⣿⣿⣿⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⣿⣿⣿⣿⣿⠀
        ⢠⣿⣿⣿⣿⡇⠈⠛⣿⡟⠀⠀⠀⠀⢻⣿⠛⠁⢸⣿⣿⣿⣿⡀
        ⢸⣿⣿⣿⣿⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣸⣿⣿⣿⣿⡇
        ⠸⣿⣿⣿⣿⣿⣧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣼⣿⣿⣿⣿⣿⠁
        ⠀⣿⣿⣿⣿⣿⣿⠀⢠⣶⣿⣿⣿⣿⣶⡄⠀⣿⣿⣿⣿⣿⣿⠀
        ⠀⠘⣿⣿⣿⣿⣿⡄⢸⡟⠋⠉⠉⠙⢻⡇⢠⣿⣿⣿⣿⣿⠃⠀
        ⠀⠀⠈⠛⠿⣿⣿⣿⣾⣇⢀⣿⣿⡀⣸⣷⣿⣿⣿⠿⠛⠁⠀⠀
        ⠀⠀⠀⠀⠀⠀⠀⠙⢿⣿⣿⣿⣿⣿⣿⡿⠋⠀⠀⠀⠀⠀⠀⠀
        ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠙⠛⠛⠋⠉
        ");

        _container.Dispose();
    }
}