using Feirapp.DocumentModels.Documents;
using Feirapp.Infrastructure.DataContext;
using Feirapp.Infrastructure.Repository;
using Feirapp.Tests.Fixtures;
using Feirapp.Tests.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Feirapp.Tests.IntegrationTest.Infrastructure;

[Collection("Database Integration Test")]
public class GroceryCategoryRepositoryTest : IDisposable
{
    private readonly IMongoFeirappContext _context;

    public static List<GroceryCategory> SearchCategoryList = new List<GroceryCategory>()
    {
        new()
        {
            Name = "Objeto 1",
            Description = "Descrição 1",
            Cest = "12345",
            ItemNumber = "1",
            Ncm = "1234"
        },
        new()
        {
            Name = "Objeto 2",
            Description = "Descrição 2",
            Cest = "54321",
            ItemNumber = "1",
            Ncm = "5432"
        },
        new()
        {
            Name = "Objeto 3",
            Description = "Descrição 3",
            Cest = "98765",
            ItemNumber = "1",
            Ncm = "1234"
        },
        new()
        {
            Name = "Objeto 4",
            Description = "Descrição 4",
            Cest = "67890",
            ItemNumber = "1",
            Ncm = "7890"
        },
        new()
        {
            Name = "Objeto 5",
            Description = "Descrição 5",
            Cest = "54321",
            ItemNumber = "2",
            Ncm = "5432"
        },
        new()
        {
            Name = "Objeto 6",
            Description = "Descrição 6",
            Cest = "11111",
            ItemNumber = "1",
            Ncm = "1111"
        },
        new()
        {
            Name = "Objeto 7",
            Description = "Descrição 7",
            Cest = "22222",
            ItemNumber = "1",
            Ncm = "2222"
        },
        new()
        {
            Name = "Objeto 8",
            Description = "Descrição 8",
            Cest = "33333",
            ItemNumber = "1",
            Ncm = "3333"
        },
        new()
        {
            Name = "Objeto 9",
            Description = "Descrição 9",
            Cest = "44444",
            ItemNumber = "1",
            Ncm = "4444"
        },
        new()
        {
            Id = "123456789012345678901234",
            Name = "Objeto 10",
            Description = "Descrição 10",
            Cest = "55555",
            ItemNumber = "1",
            Ncm = "5555"
        }
    };

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

    #region SearchAsync

    [Fact]
    public async Task SearchAsync_PassGroceryCategoryWithCest_ReturnGroceryCategoriesWithThatCestAndDistinctItemNumber()
    {
        //Arrange
        var repository = new GroceryCategoryRepository(_context);
        await repository.InsertBatchAsync(SearchCategoryList, CancellationToken.None);
        var searchGroceryCategory = new GroceryCategory
        {
            Cest = "12345"
        };

        //Act
        var actualList = await repository.SearchAsync(searchGroceryCategory, CancellationToken.None);

        //Assert
        actualList.FirstOrDefault().Cest.Should().Be(searchGroceryCategory.Cest);
    }

    [Fact]
    public async Task SearchAsync_PassGroceryCategoryWithNcm_ReturnGroceryCategoriesWithThatNcm()
    {
        //Arrange
        var repository = new GroceryCategoryRepository(_context);
        await repository.InsertBatchAsync(SearchCategoryList, CancellationToken.None);
        var searchGroceryCategory = new GroceryCategory
        {
            Ncm = "1234"
        };
        //Act
        var actualList = await repository.SearchAsync(searchGroceryCategory, CancellationToken.None);

        //Assert
        actualList.Count.Should().Be(2);

        var firstResult = actualList[0];
        var secondResult = actualList[1];

        firstResult.Ncm.Should().Be(secondResult.Ncm);
    }

    #endregion SearchAsync

    public void Dispose()
    {
        _context.DropCollection(nameof(GroceryCategory));
    }
}