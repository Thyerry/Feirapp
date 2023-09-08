using Feirapp.Entities;
using Feirapp.Infrastructure.DataContext;
using Feirapp.Infrastructure.Repository;
using Feirapp.Tests.Fixtures;
using Feirapp.Tests.Helpers;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Feirapp.Tests.IntegrationTest.Infrastructure;

[Collection("Database Integration Test")]
public class GroceryItemRepositoryTest : IDisposable
{
    private readonly IMongoFeirappContext _context;
    private const int GroceryItemsCount = 5;

    public GroceryItemRepositoryTest()
    {
        _context ??= new MongoDbContextMock().Context;
    }

    [Fact]
    public async Task InsertAsync_InsertItOnDatabase_ShouldBeInsertedOnDatabase()
    {
        //Arrange
        var repository = new GroceryItemRepository(_context);
        var expected = GroceryItemFixture.CreateRandomGroceryItem();

        //Act
        var actual = await repository.InsertAsync(expected, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task InsertGroceryItemBatchAsync_ValidBatch_ShouldInsertAllGroceryItems()
    {
        //Arrange
        var repository = new GroceryItemRepository(_context);
        var groceryItems = GroceryItemFixture.CreateListGroceryItem(GroceryItemsCount);

        //Act
        await repository.InsertGroceryItemBatchAsync(groceryItems, CancellationToken.None);

        //Assert
        var actual = await repository.GetAllAsync(CancellationToken.None);
        actual.Count.Should().Be(GroceryItemsCount);
    }

    [Fact]
    public async Task GetAllAsync_ReturnAllGroceryItemsInserted()
    {
        //Arrange
        var repository = new GroceryItemRepository(_context);
        var groceryItems = GroceryItemFixture.CreateListGroceryItem(GroceryItemsCount);
        await repository.InsertGroceryItemBatchAsync(groceryItems, CancellationToken.None);

        //Act
        var actual = await repository.GetAllAsync(CancellationToken.None);

        //Assert
        actual.Count.Should().Be(GroceryItemsCount);
    }

    public void Dispose()
    {
        _context.DropCollection(nameof(GroceryItem));
    }
}