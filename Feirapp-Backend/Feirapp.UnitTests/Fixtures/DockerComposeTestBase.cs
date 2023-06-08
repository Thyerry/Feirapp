using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Extensions;
using Ductus.FluentDocker.Services;
using Feirapp.DAL.DataContext;
using Feirapp.Domain.Models;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using MongoDB.Driver;
using Xunit;
using FluentAssertions;

namespace Feirapp.UnitTests.Fixtures;

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
            .Build()
            .Start();
    }

    [Fact]
    public async void mini_integration_test()
    {
        var mongoSettings = new MongoSettings()
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "Feirapp",
        };
        var mongoContext = new MongoFeirappContext(new OptionsConfigurationMock<MongoSettings>(mongoSettings));

        var collection = mongoContext.GetCollection<GroceryItem>(nameof(GroceryItem));
        var expected = new GroceryItem { Name = "thyerry" };

        await collection.InsertOneAsync(expected);

        var actual = (await collection.FindAsync(g => g.Name.Contains(expected.Name))).FirstOrDefault();

        actual.Should().BeEquivalentTo(expected);
    }

    public void Dispose()
    {
        Debug.WriteLine("YEAH, I'M THINKING I'M DISPOSED 😡");
        _container.Dispose();
    }
}

public class OptionsConfigurationMock<T> : IOptions<T> where T : class
{
    public T Value { get; }

    public OptionsConfigurationMock(T value)
    {
        Value = value;
    }
}