using Feirapp.DAL.DataContext;
using Feirapp.DAL.Repositories;
using Feirapp.Domain.Models;
using Feirapp.UnitTests.Fixtures;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Feirapp.UnitTests.IntegrationTest;

public class TestGroceryItemRepository
{
    private readonly IMongoFeirappContext _context;
    private readonly DockerComposeTestBase _docker;

    public TestGroceryItemRepository()
    {
        _context = new MongoDbFixture().Context;
        _docker = new DockerComposeTestBase();
    }

    [Fact]
    public async Task NewGroceryItem_InsertItOnDatabase_ShouldBeInsertedOnDatabase()
    {
        //Arrange
        var _repository = new GroceryItemRepository(_context);
        var expected = new GroceryItem();

        //Act
        var actual = await _repository.CreateGroceryItem(expected);

        //Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task n()
    {
        _docker.Dispose();
    }
}