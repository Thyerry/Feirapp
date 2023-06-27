using Feirapp.DAL.DataContext;
using Feirapp.DAL.Repositories;
using Feirapp.Domain.Models;
using Feirapp.Tests.Fixtures;
using Feirapp.Tests.Helpers;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Feirapp.Tests.IntegrationTest;

[Collection("Database Integration Test")]
public class TestGroceryItemRepositoryAgain : IDisposable
{
    private const string NewGroceryItemName = "NEW_GROCERY_ITEM_NAME";
    private readonly IMongoFeirappContext _context;

    public TestGroceryItemRepositoryAgain(DockerComposeTestBase docker)
    {
        _context ??= new MongoDbContextMock().Context;
    }

    [Fact]
    public async Task OneMoreTestJustToKnowWhatHappensIfIRunTwoTestClassesWithTheSameDockerComposeSource()
    {
        //Arrange
        var repository = new GroceryItemRepository(_context);
        var groceryItem = GroceryItemFixture.CreateRandomGroceryItem();
        await repository.CreateGroceryItem(groceryItem);
        groceryItem.Name = NewGroceryItemName;
        //Act
        await repository.UpdateGroceryItem(groceryItem);
        //Assert
        var actual = await repository.GetGroceryItemById(groceryItem.Id!);
        actual.Name.Should().Be(NewGroceryItemName);
    }

    public void Dispose()
    {
        _context.DropCollection(nameof(GroceryItem));
    }
}