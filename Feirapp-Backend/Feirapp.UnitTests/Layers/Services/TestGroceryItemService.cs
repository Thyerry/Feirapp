using Feirapp.Domain.Contracts;
using Feirapp.Domain.Models;
using Feirapp.Service.Services;
using Feirapp.UnitTests.Fixtures;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Feirapp.UnitTests.Layers.Services;

public class TestGroceryItemService
{
    [Fact]
    public async Task GetAllGroceryItems_ShouldReturnListOfGroceryItems()
    {
        // Assert
        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
        mockGroceryItemRepository
            .Setup(repo => repo.GetAllGroceryItems())
            .ReturnsAsync(GroceryItemFixture.GetGroceryItems());
        var sut = new GroceryItemService(mockGroceryItemRepository.Object);

        // Act
        var result = await sut.GetAllGroceryItems();

        // Assert
        result.Should().BeOfType<List<GroceryItem>>();
    }

    [Fact]
    public async Task GetAllGroceryItems_ShouldInvokeGroceryItemRepository()
    {
        // Arrange
        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
        mockGroceryItemRepository
            .Setup(repo => repo.GetAllGroceryItems())
            .ReturnsAsync(GroceryItemFixture.GetGroceryItems());
        var sut = new GroceryItemService(mockGroceryItemRepository.Object);

        // Act
        await sut.GetAllGroceryItems();

        // Assert
        mockGroceryItemRepository.Verify(repo => repo.GetAllGroceryItems(), Times.Once);
    }
}