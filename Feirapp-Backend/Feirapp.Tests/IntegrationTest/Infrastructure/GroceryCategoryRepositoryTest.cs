using Feirapp.Entities;
using Feirapp.Infrastructure.DataContext;
using Feirapp.Infrastructure.Repository;
using Feirapp.Tests.Fixtures;
using Feirapp.Tests.Helpers;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Feirapp.Tests.IntegrationTest.Infrastructure;

[Collection("Database Integration Test")]
public class GroceryCategoryRepositoryTest : IDisposable
{
    private readonly IMongoFeirappContext _context;

    public GroceryCategoryRepositoryTest()
    {
        _context ??= new MongoDbContextMock().Context;
    }

    [Fact]
    public async Task GetAllAsync_ReturnAllGroceryCategories()
    {
        //Arrange
        var repository = new GroceryCategoryRepository(_context);
        var expected = GroceryCategoryFixture.CreateListGroceryCategory(10);
        await repository.InsertBatchAsync(expected, CancellationToken.None);

        //Act
        var result = await repository.GetAllAsync();

        //Assert
        result.Count.Should().Be(expected.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnGroceryCategoryWithThatId()
    {
        //Arrange
        var repository = new GroceryCategoryRepository(_context);
        var groceryCategories = GroceryCategoryFixture.CreateListGroceryCategory(10);
        await repository.InsertBatchAsync(groceryCategories, CancellationToken.None);
        var expected = (await repository.GetAllAsync()).MinBy(x => Guid.NewGuid());

        //Act
        var actual = await repository.GetByIdAsync(expected.Id);

        //Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task InsertAsync_InsertOnDatabaseSuccessfully()
    {
        //Arrange
        var repository = new GroceryCategoryRepository(_context);
        var expected = GroceryCategoryFixture.CreateRandomGroceryCategory();

        //Act
        var actual = await repository.InsertAsync(expected, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task InsertBatchAsync_InsertOnDatabaseSuccessfully()
    {
        //Arrange
        var repository = new GroceryCategoryRepository(_context);
        var expected = GroceryCategoryFixture.CreateListGroceryCategory(10);

        //Act
        await repository.InsertBatchAsync(expected, CancellationToken.None);

        //Assert
        var actual = await repository.GetAllAsync();
        actual.Count.Should().Be(expected.Count);
    }

    [Fact]
    public async Task UpdateAsync_UpdateDocumentOnDatabaseSuccessfully()
    {
        //Arrange
        var repository = new GroceryCategoryRepository(_context);
        var insertGroceryCategory = GroceryCategoryFixture.CreateRandomGroceryCategory();
        await repository.InsertAsync(insertGroceryCategory, CancellationToken.None);
        var expected = GroceryCategoryFixture.CreateRandomGroceryCategory();
        expected.Id = insertGroceryCategory.Id;

        //Act
        await repository.UpdateAsync(expected, CancellationToken.None);

        //Assert
        var actual = await repository.GetByIdAsync(insertGroceryCategory.Id!);
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task DeleteAsync_DeleteFromDatabaseSuccessfully()
    {
        //Arrange
        var repository = new GroceryCategoryRepository(_context);
        var groceryCategory = GroceryCategoryFixture.CreateRandomGroceryCategory();
        await repository.InsertAsync(groceryCategory, CancellationToken.None);
        var deleteId = groceryCategory.Id;
        
        //Act
        await repository.DeleteAsync(deleteId, CancellationToken.None);

        //Assert
        var actual = await repository.GetByIdAsync(deleteId);
        actual.Should().BeNull();
    }

    public void Dispose()
    {
        _context.DropCollection(nameof(GroceryCategory));
    }
}