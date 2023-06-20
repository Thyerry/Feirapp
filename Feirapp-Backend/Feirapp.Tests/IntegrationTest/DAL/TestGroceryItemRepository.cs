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
public class TestGroceryItemRepository : IDisposable
{
    private readonly IMongoFeirappContext _context;
    private const int GROCERY_ITEMS_COUNT = 5;

    public TestGroceryItemRepository()
    {
        _context ??= new MongoDbContextMock().Context;
    }

    [Fact]
    public async Task NewGroceryItem_InsertItOnDatabase_ShouldBeInsertedOnDatabase()
    {
        //Arrange
        var _repository = new GroceryItemRepository(_context);
        var expected = GroceryItemFixture.CreateRandomGroceryItem();

        //Act
        var actual = await _repository.CreateGroceryItem(expected);

        //Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task CreateGroceryItemBatch_ValidBatch_ShouldInsertAllGroceryItems()
    {
        //Arrange
        var _repository = new GroceryItemRepository(_context);
        var groceryItems = GroceryItemFixture.CreateListGroceryItem(GROCERY_ITEMS_COUNT);

        //Act
        await _repository.CreateGroceryItemBatch(groceryItems);

        //Assert
        var actual = await _repository.GetAllGroceryItems();
        actual.Count.Should().Be(GROCERY_ITEMS_COUNT);
    }

    [Fact]
    public async Task GetAllGroceryItems_IntegrationTest()
    {
        //Arrange
        var _repository = new GroceryItemRepository(_context);
        var groceryItems = GroceryItemFixture.CreateListGroceryItem(GROCERY_ITEMS_COUNT);
        await _repository.CreateGroceryItemBatch(groceryItems);

        //Act
        var actual = await _repository.GetAllGroceryItems();

        //Assert
        actual.Count.Should().Be(GROCERY_ITEMS_COUNT);
    }

    public void Dispose()
    {
        _context.DropCollection(nameof(GroceryItem));
    }
}