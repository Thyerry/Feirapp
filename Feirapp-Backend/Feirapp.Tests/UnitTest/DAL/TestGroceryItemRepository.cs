using Feirapp.DAL.DataContext;
using Feirapp.DAL.Repositories;
using Feirapp.Domain.Models;
using Feirapp.Tests.Fixtures;
using FluentAssertions;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Feirapp.Tests.UnitTest.DAL;

public class TestGroceryItemRepository
{
    public class TestGetAllGroceryItems
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

    public class TestGetRandomGroceryItems
    {
        [Fact]
        public async Task GetRandomGroceryItems_ShouldReturnListOfGroceryItems()
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
                .Setup(c => c.AggregateAsync(
                        It.IsAny<PipelineDefinition<GroceryItem, GroceryItem>>(),
                        It.IsAny<AggregateOptions>(),
                        It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(mockCursor.Object);

            mockDbContext
                .Setup(db => db.GetCollection<GroceryItem>(nameof(GroceryItem)))
                .Returns(mockCollection.Object);

            var sut = new GroceryItemRepository(mockDbContext.Object);

            // Act
            var result = await sut.GetRandomGroceryItems(1);

            // Assert
            result.Should().BeOfType<List<GroceryItem>>();
        }

        [Fact]
        public async Task GetRandomGroceryItems_ShouldInvokeGroceryItemCollection()
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
                .Setup(c => c.AggregateAsync(
                    It.IsAny<PipelineDefinition<GroceryItem, GroceryItem>>(),
                    It.IsAny<AggregateOptions>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(mockCursor.Object);

            mockDbContext
                .Setup(db => db.GetCollection<GroceryItem>(nameof(GroceryItem)))
                .Returns(mockCollection.Object);

            var sut = new GroceryItemRepository(mockDbContext.Object);

            // Act
            await sut.GetRandomGroceryItems(1);

            // Assert
            mockCollection.Verify(c => c.AggregateAsync(
                It.IsAny<PipelineDefinition<GroceryItem, GroceryItem>>(),
                It.IsAny<AggregateOptions>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }

    public class TestGetGroceryItemById
    {
        [Fact]
        public async Task GetGroceryItemById_ReturnGroceryItem()
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
            var result = await sut.GetGroceryItemById(string.Empty);

            // Assert
            result.Should().BeOfType<GroceryItem>();
        }

        [Fact]
        public async Task GetGroceryItemById_InvokeGroceryItemCollection()
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
            await sut.GetGroceryItemById(string.Empty);

            // Assert
            mockCollection.Verify(c => c.FindAsync(
                It.IsAny<FilterDefinition<GroceryItem>>(),
                It.IsAny<FindOptions<GroceryItem, GroceryItem>>(),
                It.IsAny<CancellationToken>()
                ));
        }
    }

    public class TestGetGroceryItemsByName
    {
        [Fact]
        public async Task GetGroceryItemsByName_ReturnListOfGroceryItem()
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
            var result = await sut.GetGroceryItemsByName(string.Empty);

            // Assert
            result.Should().BeOfType<List<GroceryItem>>();
        }

        [Fact]
        public async Task GetGroceryItemsByName_InvokeGroceryItemCollection()
        {
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
            await sut.GetGroceryItemsByName(string.Empty);

            // Assert
            mockCollection.Verify(db => db.FindAsync(
                It.IsAny<FilterDefinition<GroceryItem>>(),
                It.IsAny<FindOptions<GroceryItem, GroceryItem>>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }

    public class TestCreateGroceryItem
    {
        [Fact]
        public async Task CreateGroceryItem_ReturnCreatedGroceryItem()
        {
            // Assert
            var mockContext = new Mock<IMongoFeirappContext>();
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

            mockCollection
                .Setup(c => c.InsertOneAsync(It.IsAny<GroceryItem>(),
                    It.IsAny<InsertOneOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mockContext
                .Setup(c => c.GetCollection<GroceryItem>(nameof(GroceryItem)))
                .Returns(mockCollection.Object);

            var sut = new GroceryItemRepository(mockContext.Object);

            // Act
            var result = await sut.CreateGroceryItem(new GroceryItem());

            // Assert
            result.Should().BeOfType<GroceryItem>();
        }

        [Fact]
        public async Task CreateGroceryItem_InvokeGroceryItemCollection()
        {
            // Assert
            var mockContext = new Mock<IMongoFeirappContext>();
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

            mockCollection
                .Setup(c => c.InsertOneAsync(It.IsAny<GroceryItem>(),
                    It.IsAny<InsertOneOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mockContext
                .Setup(c => c.GetCollection<GroceryItem>(nameof(GroceryItem)))
                .Returns(mockCollection.Object);

            var sut = new GroceryItemRepository(mockContext.Object);

            // Act
            await sut.CreateGroceryItem(new GroceryItem());

            // Assert
            mockCollection.Verify(c => c.InsertOneAsync(
                    It.IsAny<GroceryItem>(),
                    It.IsAny<InsertOneOptions>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

    public class TestUpdateGroceryItem
    {
        [Fact]
        public async Task UpdateGroceryItem_ReturnGroceryItem()
        {
            // Arrange
            var mockContext = new Mock<IMongoFeirappContext>();
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

            mockCollection
                .Setup(c => c.UpdateOneAsync(
                    It.IsAny<FilterDefinition<GroceryItem>>(),
                    It.IsAny<UpdateDefinition<GroceryItem>>(),
                    It.IsAny<UpdateOptions>(),
                    It.IsAny<CancellationToken>()
                ));

            mockContext
                .Setup(context => context.GetCollection<GroceryItem>(nameof(GroceryItem)))
                .Returns(mockCollection.Object);

            var sut = new GroceryItemRepository(mockContext.Object);

            // Act
            var result = await sut.UpdateGroceryItem(groceryList.FirstOrDefault());

            // Assert
            result.Should().BeOfType<GroceryItem>();
        }

        [Fact]
        public async Task UpdateGroceryItem_InvokeGroceryItemsCollection()
        {
            // Arrange
            var mockContext = new Mock<IMongoFeirappContext>();
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

            mockCollection
                .Setup(c => c.UpdateOneAsync(
                    It.IsAny<FilterDefinition<GroceryItem>>(),
                    It.IsAny<UpdateDefinition<GroceryItem>>(),
                    It.IsAny<UpdateOptions>(),
                    It.IsAny<CancellationToken>()
                ));

            mockContext
                .Setup(context => context.GetCollection<GroceryItem>(nameof(GroceryItem)))
                .Returns(mockCollection.Object);

            var sut = new GroceryItemRepository(mockContext.Object);

            // Act
            await sut.UpdateGroceryItem(groceryList.FirstOrDefault());

            // Assert
            mockCollection.Verify(c => c.UpdateOneAsync(
                It.IsAny<FilterDefinition<GroceryItem>>(),
                It.IsAny<UpdateDefinition<GroceryItem>>(),
                It.IsAny<UpdateOptions>(),
                It.IsAny<CancellationToken>()
                ),
                Times.Once);
        }

        [Fact]
        public async Task UpdateGroceryItem_AddingNewPriceLog()
        {
            // Arrange
            var mockContext = new Mock<IMongoFeirappContext>();
            var mockCollection = new Mock<IMongoCollection<GroceryItem>>();
            var mockCursor = new Mock<IAsyncCursor<GroceryItem>>();
            var groceryList = GroceryItemFixture.GetGroceryItemsWithPriceHistory();

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

            mockCollection
                .Setup(c => c.UpdateOneAsync(
                    It.IsAny<FilterDefinition<GroceryItem>>(),
                    It.IsAny<UpdateDefinition<GroceryItem>>(),
                    It.IsAny<UpdateOptions>(),
                    It.IsAny<CancellationToken>()
                ));

            mockContext
                .Setup(context => context.GetCollection<GroceryItem>(nameof(GroceryItem)))
                .Returns(mockCollection.Object);

            var sut = new GroceryItemRepository(mockContext.Object);

            // Act
            var result = await sut.UpdateGroceryItem(groceryList[2]);

            // Assert
            result.PriceHistory.Count.Should().Be(2);
        }

        [Fact]
        public async Task UpdateGroceryItem_CreatingFirstPriceLog()
        {
            // Arrange
            var mockContext = new Mock<IMongoFeirappContext>();
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

            mockCollection
                .Setup(c => c.UpdateOneAsync(
                    It.IsAny<FilterDefinition<GroceryItem>>(),
                    It.IsAny<UpdateDefinition<GroceryItem>>(),
                    It.IsAny<UpdateOptions>(),
                    It.IsAny<CancellationToken>()
                ));

            mockContext
                .Setup(context => context.GetCollection<GroceryItem>(nameof(GroceryItem)))
                .Returns(mockCollection.Object);

            var sut = new GroceryItemRepository(mockContext.Object);
            var parameterGroceryItem = new GroceryItem()
            {
                Price = 5.5,
                PriceHistory = new List<PriceLog> { new() { Price = 0, LogDate = DateTime.UtcNow } }
            };
            // Act
            var result = await sut.UpdateGroceryItem(parameterGroceryItem);

            // Assert
            result.PriceHistory.Count.Should().Be(1);
            result.PriceHistory.FirstOrDefault().Price.Should().Be(parameterGroceryItem.Price);
        }
    }

    public class TaskDeleteGroceryItem
    {
        [Fact]
        public async Task DeleteGroceryItem_InvokeGroceryItemCollection()
        {
            // Arrange
            var mockContext = new Mock<IMongoFeirappContext>();
            var mockCollection = new Mock<IMongoCollection<GroceryItem>>();
            mockContext
                .Setup(context => context.GetCollection<GroceryItem>(nameof(GroceryItem)))
                .Returns(mockCollection.Object);
            var sut = new GroceryItemRepository(mockContext.Object);

            // Act
            await sut.DeleteGroceryItem(string.Empty);

            // Assert
            mockCollection.Verify(collection => collection.DeleteOneAsync(
                It.IsAny<FilterDefinition<GroceryItem>>(),
                It.IsAny<CancellationToken>()
            ));
        }
    }
}