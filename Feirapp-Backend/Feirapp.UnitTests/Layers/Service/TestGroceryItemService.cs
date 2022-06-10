using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Feirapp.Domain.Contracts;
using Feirapp.Domain.Models;
using Feirapp.Service.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Feirapp.UnitTests.Layers.Service;

public class TestGroceryItemService
{
    [Fact]
    public async Task GetAllGroceryItems_OnSuccess_ReturnAllGroceryItems()
    {
        // Arrange
        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
        mockGroceryItemRepository
            .Setup(repository => repository.GetAllGroceryItems())
            .ReturnsAsync(new List<GroceryItem>());
        var sut = new GroceryItemService(mockGroceryItemRepository.Object);
        
        // Act
        var result = await sut.GetAllGroceryItems();
        
        // Assert
        result.Should().BeOfType<List<GroceryItem>>();
    }

    [Fact]
    public async Task GetAllGroceryItems_OnSuccess_InvokeGroceryItemRepository()
    {
        // Arrange
        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
        mockGroceryItemRepository
            .Setup(repository => repository.GetAllGroceryItems())
            .ReturnsAsync(new List<GroceryItem>());
        var sut = new GroceryItemService(mockGroceryItemRepository.Object);
        
        // Act
        await sut.GetAllGroceryItems();
        
        // Assert
        mockGroceryItemRepository.Verify(
            repository => repository.GetAllGroceryItems(),
            Times.Once
            );
    }
}