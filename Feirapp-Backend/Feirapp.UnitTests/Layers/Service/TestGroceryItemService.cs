using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Feirapp.Domain.Contracts;
using Feirapp.Domain.Models;
using Feirapp.Service.Services;
using Feirapp.UnitTests.Fixtures;
using FluentAssertions;
using Moq;
using Xunit;

namespace Feirapp.UnitTests.Layers.Service;

public class TestGroceryItemService
{
    #region TestGetAllGroceryItem

    [Fact]
    public async Task GetAllGroceryItems_ReturnListOfGroceryItems()
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
    public async Task GetAllGroceryItems_InvokeGroceryItemRepository()
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

    #endregion

    #region TestGetByName
    
    [Fact]
    public async Task GetByName_ReturnsListOfGroceryItems()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryItemRepository>();
        mockRepository
            .Setup(repo => repo.GetByName(It.IsAny<string>()))
            .ReturnsAsync(new List<GroceryItem>());
        var sut = new GroceryItemService(mockRepository.Object);
        
        // Act
        var result = await sut.GetByName(string.Empty);

        //
        result.Should().BeOfType<List<GroceryItem>>();
    }

    [Fact]
    public async Task GetByName_InvokeGroceryItemRepository()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryItemRepository>();
        mockRepository
            .Setup(repo => repo.GetByName(It.IsAny<string>()))
            .ReturnsAsync(GroceryItemFixture.GetGroceryItems());
        var sut = new GroceryItemService(mockRepository.Object);
        
        // Act
        var result = await sut.GetByName(string.Empty);

        // Assert
        mockRepository.Verify(repo => repo.GetByName(It.IsAny<string>()), Times.Once);
    }
    
    #endregion
}