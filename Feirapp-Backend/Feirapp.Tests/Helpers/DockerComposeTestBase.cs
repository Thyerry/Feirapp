using System;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Feirapp.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Feirapp.Tests.Helpers;

public class DockerComposeTestBase : IAsyncLifetime
{
    private readonly IContainerService _container;
    private readonly IList<IVolumeService>? _volumes;

    public DockerComposeTestBase()
    {
        var hosts = new Hosts().Discover();
        _volumes = hosts.FirstOrDefault()!.GetVolumes();

        _container = new Builder()
            .UseContainer()
            .UseImage("postgres:16")
            .ExposePort(5433, 5432)
            .WaitForPort("5432/tcp", 3000, "127.0.0.1")
            .WithName("feirapp-integration-tests")
            .WithEnvironment(
                "POSTGRES_DB=feirapp-test-db",
                "POSTGRES_USER=feirapp-test-user",
                "POSTGRES_PASSWORD=feirapp-test-password")
            .Build()
            .Start();
        
        Console.WriteLine("Starting docker container...");
    }

    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<BaseContext>()
            .UseNpgsql("Host=localhost;Port=5433;Database=feirapp-test-db;Username=feirapp-test-user;Password=feirapp-test-password;Timeout=30;CommandTimeout=30;Include Error Detail=true")
            .Options;
        
        if(!await TestConnection(options))
            throw new InvalidOperationException("Could not connect to the database.");

        await using var context = new BaseContext(options);
        await context.Database.MigrateAsync();
    }


    private async Task<bool> TestConnection(DbContextOptions<BaseContext> options)
    {
        const int maxAttempts = 10;
        var connected = false;
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                await using var context = new BaseContext(options);
                if (!await context.Database.CanConnectAsync())
                    throw new Exception("Could not connect to the database.");
                connected = true;
                break;
            }
            catch (Exception)
            {
                await Task.Delay(2000);
            }
        }

        return connected;
    }

    public Task DisposeAsync()
    {
        _container.Dispose();
        foreach (var volumeService in _container.GetVolumes())
            volumeService.Remove();
        
        return Task.CompletedTask;
    }
}