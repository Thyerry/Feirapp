using Feirapp.Entities;
using Feirapp.Infrastructure.DataContext;
using Feirapp.Infrastructure.Repository;
using Feirapp.Tests.Fixtures;
using Feirapp.Tests.Helpers;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Feirapp.Tests.IntegrationTest.Infrastructure;

[Collection("Database Integration Test")]
public class TestGroceryItemRepository : IDisposable
{
    private readonly IMongoFeirappContext _context;
    private const int GroceryItemsCount = 5;

    public TestGroceryItemRepository()
    {
        _context ??= new MongoDbContextMock().Context;
    }

    [Fact]
    public async Task NewGroceryItem_InsertItOnDatabase_ShouldBeInsertedOnDatabase()
    {
        //Arrange
        var repository = new GroceryItemRepository(_context);
        var expected = GroceryItemFixture.CreateRandomGroceryItem();

        //Act
        var actual = await repository.InsertAsync(expected);

        //Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task CreateGroceryItemBatch_ValidBatch_ShouldInsertAllGroceryItems()
    {
        //Arrange
        var repository = new GroceryItemRepository(_context);
        var groceryItems = GroceryItemFixture.CreateListGroceryItem(GroceryItemsCount);

        //Act
        await repository.InsertGroceryItemBatch(groceryItems);

        //Assert
        var actual = await repository.GetAllAsync();
        actual.Count.Should().Be(GroceryItemsCount);
    }

    [Fact]
    public async Task GetAllGroceryItems_IntegrationTest()
    {
        //Arrange
        var repository = new GroceryItemRepository(_context);
        var groceryItems = GroceryItemFixture.CreateListGroceryItem(GroceryItemsCount);
        await repository.InsertGroceryItemBatch(groceryItems);

        //Act
        var actual = await repository.GetAllAsync();

        //Assert
        actual.Count.Should().Be(GroceryItemsCount);
    }

    public void Dispose()
    {
        _context.DropCollection(nameof(GroceryItem));
    }
}