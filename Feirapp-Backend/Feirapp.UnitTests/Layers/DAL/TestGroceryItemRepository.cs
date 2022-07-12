using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Feirapp.DAL.DataContext;
using Feirapp.DAL.Repositories;
using Feirapp.Domain.Models;
using Feirapp.UnitTests.Fixtures;
using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Feirapp.UnitTests.Layers.DAL;

public class TestGroceryItemRepository
{
    [Fact]
    public async Task GetAllGroceryItems_ShouldReturnListOfGroceryItems()
    {
        // Arrange
        var mockDbContext = new Mock<IMongoFeirappContext>();
        var mockCollection = new Mock<IMongoCollection<GroceryItem>>();
        var mockCursor = new Mock<IAsyncCursor<GroceryItem>>();
        var groceryList = GroceryItemFixture.GetGroceryItems();
        
        mockCursor
            .Setup(c => c.Current)
            .Returns(groceryList);
        
        mockCursor
            .SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);

        mockCollection
            .Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<GroceryItem>>(),
                It.IsAny<FindOptions<GroceryItem, GroceryItem>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(mockCursor.Object);

        mockDbContext
            .Setup(db => db.GetCollection<GroceryItem>(nameof(GroceryItem)))
            .Returns(mockCollection.Object);
        var sut = new GroceryItemRepository(mockDbContext.Object);
        
        // Act
        var result = await sut.GetAllGroceryItems();
        
        // Assert
        result.Should().BeOfType<List<GroceryItem>>();
    }

    [Fact]
    public async Task GetAllGroceryItems_ShouldInvokeGroceryItemCollection()
    {
        // Arrange
        var mockDbContext = new Mock<IMongoFeirappContext>();
        var mockCollection = new Mock<IMongoCollection<GroceryItem>>();
        var mockCursor = new Mock<IAsyncCursor<GroceryItem>>();
        var groceryList = GroceryItemFixture.GetGroceryItems();
        
        mockCursor
            .Setup(c => c.Current)
            .Returns(groceryList);
        mockCursor
            .SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);

        mockCollection
            .Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<GroceryItem>>(),
                It.IsAny<FindOptions<GroceryItem, GroceryItem>>(),
                It.IsAny<CancellationToken>()
                ))
            .ReturnsAsync(mockCursor.Object);

        mockDbContext
            .Setup(db => db.GetCollection<GroceryItem>(nameof(GroceryItem)))
            .Returns(mockCollection.Object);
        var sut = new GroceryItemRepository(mockDbContext.Object);
        
        // Act
        await sut.GetAllGroceryItems();
        
        // Assert
        mockCollection.Verify(db => db.FindAsync(
            It.IsAny<FilterDefinition<GroceryItem>>(),
            It.IsAny<FindOptions<GroceryItem, GroceryItem>>(),
            It.IsAny<CancellationToken>()
            ), Times.Once);

    }
}