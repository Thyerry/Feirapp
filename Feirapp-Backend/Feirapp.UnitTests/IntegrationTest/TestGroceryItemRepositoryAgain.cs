using Feirapp.DAL.DataContext;
using Feirapp.DAL.Repositories;
using Feirapp.Domain.Models;
using Feirapp.UnitTests.Fixtures;
using Feirapp.UnitTests.Helpers;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Feirapp.UnitTests.IntegrationTest;

[Collection("Database Integration Test")]
public class TestGroceryItemRepositoryAgain : IDisposable
{
    private readonly IMongoFeirappContext _context;

    private const string NEW_GROCERY_ITEM_NAME = "New Name";

    public TestGroceryItemRepositoryAgain(DockerComposeTestBase docker)
    {
        _context ??= new MongoDbContextMock().Context;
    }

    [Fact]
    public async Task OneMoreTestJustToKnowWhatHappensIfIRunTwoTestClassesWithTheSameDockerComposeSource()
    {
        //Arrange
        var _repository = new GroceryItemRepository(_context);
        var groceryItem = GroceryItemFixture.CreateRandomGroceryItem();
        await _repository.CreateGroceryItem(groceryItem);
        groceryItem.Name = NEW_GROCERY_ITEM_NAME;
        //Act
        await _repository.UpdateGroceryItem(groceryItem);
        //Assert
        var actual = await _repository.GetGroceryItemById(groceryItem.Id);
        actual.Name.Should().Be(NEW_GROCERY_ITEM_NAME);
    }

    public void Dispose()
    {
        _context.DropCollection(nameof(GroceryItem));
    }
}