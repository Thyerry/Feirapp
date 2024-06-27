//using Feirapp.Infrastructure.DataContext;
//using Feirapp.Infrastructure.Repository;
//using Feirapp.Tests.Fixtures;
//using Feirapp.Tests.Helpers;
//using FluentAssertions;
//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Feirapp.Entities.Entities;
//using Xunit;

//namespace Feirapp.Tests.IntegrationTest.Infrastructure;

//[Collection("Database Integration Test")]
//public class GroceryItemRepositoryTest : IDisposable
//{
//    private readonly IMongoFeirappContext _context;
//    private const int GroceryItemsCount = 5;

//    public GroceryItemRepositoryTest()
//    {
//        _context ??= new MongoDbContextMock().Context;
//    }

//    [Fact]
//    public async Task GetAllAsync_ReturnAllGroceryItemsInserted()
//    {
//        //Arrange
//        var repository = new GroceryItemRepository(_context);
//        var groceryItems = GroceryItemFixture.CreateList(GroceryItemsCount);
//        await repository.InsertBatchAsync(groceryItems, CancellationToken.None);

//        //Act
//        var actual = await repository.GetAllAsync(CancellationToken.None);

//        //Assert
//        actual.Count.Should().Be(GroceryItemsCount);
//    }

//    [Fact]
//    public async Task GetByIdAsync_ReturnGroceryItemWithThatId()
//    {
//        //Arrange
//        var repository = new GroceryItemRepository(_context);
//        var groceryCategories = GroceryItemFixture.CreateList(10);
//        await repository.InsertBatchAsync(groceryCategories, CancellationToken.None);
//        var expected = (await repository.GetAllAsync(CancellationToken.None)).MinBy(x => Guid.NewGuid());

//        //Act
//        var actual = await repository.GetByIdAsync(expected.Id, CancellationToken.None);

//        //Assert
//        actual.Should().BeEquivalentTo(expected);
//    }

//    [Fact]
//    public async Task InsertAsync_InsertOnDatabaseSuccessfully()
//    {
//        //Arrange
//        var repository = new GroceryItemRepository(_context);
//        var expected = GroceryItemFixture.CreateRandomGroceryItem();

//        //Act
//        var actual = await repository.InsertAsync(expected, CancellationToken.None);

//        //Assert
//        actual.Should().BeEquivalentTo(expected);
//    }

//    [Fact]
//    public async Task InsertBatchAsync_InsertOnDatabaseSuccessfully()
//    {
//        //Arrange
//        var repository = new GroceryItemRepository(_context);
//        var groceryItems = GroceryItemFixture.CreateList(GroceryItemsCount);

//        //Act
//        await repository.InsertBatchAsync(groceryItems, CancellationToken.None);

//        //Assert
//        var actual = await repository.GetAllAsync(CancellationToken.None);
//        actual.Count.Should().Be(GroceryItemsCount);
//    }

//    [Fact]
//    public async Task UpdateAsync_UpdateDocumentOnDatabaseSuccessfully()
//    {
//        //Arrange
//        var repository = new GroceryItemRepository(_context);
//        var insertGroceryItem = GroceryItemFixture.CreateRandomGroceryItem();
//        await repository.InsertAsync(insertGroceryItem, CancellationToken.None);
//        var expected = GroceryItemFixture.CreateRandomGroceryItem();
//        expected.Id = insertGroceryItem.Id;

//        //Act
//        await repository.UpdateAsync(expected, CancellationToken.None);

//        //Assert
//        var actual = await repository.GetByIdAsync(insertGroceryItem.Id!, CancellationToken.None);
//        actual.Should().BeEquivalentTo(expected);
//    }

//    [Fact]
//    public async Task DeleteAsync_DeleteFromDatabaseSuccessfully()
//    {
//        //Arrange
//        var repository = new GroceryItemRepository(_context);
//        var groceryItem = GroceryItemFixture.CreateRandomGroceryItem();
//        await repository.InsertAsync(groceryItem, CancellationToken.None);
//        var deleteId = groceryItem.Id;

//        //Act
//        await repository.DeleteAsync(deleteId, CancellationToken.None);

//        //Assert
//        var actual = await repository.GetByIdAsync(deleteId, CancellationToken.None);
//        actual.Should().BeNull();
//    }

//    public void Dispose()
//    {
//        _context.DropCollection(nameof(GroceryItem));
//    }
//}