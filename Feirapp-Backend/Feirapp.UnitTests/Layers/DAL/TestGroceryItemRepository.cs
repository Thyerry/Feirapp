using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Feirapp.DAL.Repositories;
using Feirapp.Domain.Models;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Bindings;
using MongoDB.Driver.Core.Operations;
using Moq;
using Xunit;

namespace Feirapp.UnitTests.Layers.DAL;

public class TestGroceryItemRepository
{
    [Fact]
    public async Task GetGroceryItem_ShouldReturnGroceryItems()
    {
        // Arrange
        var sut = new GroceryItemRepository();

        // Act
        var result = await sut.GetAllGroceryItems();
        
        // Assert
        result.Should().BeOfType<List<GroceryItem>>();
    }

    [Fact]
    public async Task GetGroceryItem_ShouldConnectWithDatabase()
    {
        // Arrange
        var mockCollection = new Mock<IMongoCollection<GroceryItem>>();
        mockCollection
            .Setup(colleciton => colleciton.FindAsync(
                It.IsAny<FilterDefinition<GroceryItem>>(),
                null,
                It.IsAny<CancellationToken>()
                ))
            ;

        var ass = (await mockCollection.Object.FindAsync(a => a.Id == Guid.Empty)).ToList();
    }
}
