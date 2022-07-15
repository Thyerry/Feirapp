using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Feirapp.Domain.Contracts;
using Feirapp.Domain.Models;
using Feirapp.Service.Services;
using Feirapp.UnitTests.Fixtures;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
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

    #region TestGetGroceryItemById

    [Fact]
    public async Task GetGroceryItemById_ReturnGroceryItem()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryItemRepository>();
        mockRepository
            .Setup(repo => repo.GetGroceryItemById(It.IsAny<string>()))
            .ReturnsAsync(new GroceryItem());
        var sut = new GroceryItemService(mockRepository.Object);

        // Act
        var result = await sut.GetGroceryItemById(string.Empty);

        // Assert
        result.Should().BeOfType<GroceryItem>();
    }

    [Fact]
    public async Task GetGroceryItemById_InvokeGroceryItemRepository()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryItemRepository>();
        mockRepository
            .Setup(repo => repo.GetGroceryItemById(It.IsAny<string>()))
            .ReturnsAsync(new GroceryItem());
        var sut = new GroceryItemService(mockRepository.Object);

        // Act
        await sut.GetGroceryItemById(string.Empty);

        // Assert
        mockRepository.Verify(repo => repo.GetGroceryItemById(It.IsAny<string>()), Times.Once);
    }
    #endregion
    
    #region TestGetGroceryItemByName
    
    [Fact]
    public async Task GetGroceryItemByName_ReturnsListOfGroceryItems()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryItemRepository>();
        mockRepository
            .Setup(repo => repo.GetGroceryItemsByName(It.IsAny<string>()))
            .ReturnsAsync(new List<GroceryItem>());
        var sut = new GroceryItemService(mockRepository.Object);
        
        // Act
        var result = await sut.GetGroceryItemByName(string.Empty);

        //
        result.Should().BeOfType<List<GroceryItem>>();
    }

    [Fact]
    public async Task GetGroceryItemByName_InvokeGroceryItemRepository()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryItemRepository>();
        mockRepository
            .Setup(repo => repo.GetGroceryItemsByName(It.IsAny<string>()))
            .ReturnsAsync(GroceryItemFixture.GetGroceryItems());
        var sut = new GroceryItemService(mockRepository.Object);
        
        // Act
        var result = await sut.GetGroceryItemByName(string.Empty);

        // Assert
        mockRepository.Verify(repo => repo.GetGroceryItemsByName(It.IsAny<string>()), Times.Once);
    }
    
    #endregion

    #region TestCreateGroceryItem

    [Fact]
    public async Task CreateGroceryItem_ValidGroceryItem_ReturnGroceryItem()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryItemRepository>();
        mockRepository
            .Setup(repo => repo.CreateGroceryItem(It.IsAny<GroceryItem>()))
            .ReturnsAsync(new GroceryItem());
        var sut = new GroceryItemService(mockRepository.Object);
        var groceryItem = GroceryItemFixture.GetGroceryItems().FirstOrDefault();
        
        // Act
        var result = await sut.CreateGroceryItem(groceryItem!);
        
        // Assert
        result.Should().BeOfType<GroceryItem>();
    }

    [Fact]
    public async Task CreateGroceryItem_ValidGroceryItem_InvokeGroceryItemRepository()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryItemRepository>();
        mockRepository
            .Setup(repo => repo.CreateGroceryItem(It.IsAny<GroceryItem>()))
            .ReturnsAsync(new GroceryItem());
        var sut = new GroceryItemService(mockRepository.Object);
        var groceryItem = GroceryItemFixture.GetGroceryItems().FirstOrDefault();
        // Act
        await sut.CreateGroceryItem(groceryItem!);

        // Assert
        mockRepository.Verify(repo => repo.CreateGroceryItem(It.IsAny<GroceryItem>()), Times.Once);
    }

    [Fact]
    public async Task CreateGroceryItem_InvalidGroceryItem_RaiseValidationException()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryItemRepository>();
        mockRepository
            .Setup(repo => repo.CreateGroceryItem(It.IsAny<GroceryItem>()))
            .ReturnsAsync(new GroceryItem());
        var sut = new GroceryItemService(mockRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => sut.CreateGroceryItem(new GroceryItem()));
    }
    
    #endregion

    #region TestUpdateGroceryItem

    [Fact]
    public async Task UpdateGroceryItem_ReturnUpdatedGroceryItem()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryItemRepository>();
        mockRepository
            .Setup(repo => repo.UpdateGroceryItem(It.IsAny<GroceryItem>()))
            .ReturnsAsync(new GroceryItem());
        var sut = new GroceryItemService(mockRepository.Object);
        
        // Act
        var result = await sut.UpdateGroceryItem(new GroceryItem());

        // Assert
        result.Should().BeOfType<GroceryItem>();
    }
    
    [Fact]
    public async Task UpdateGroceryItem_InvokeGroceryItemRepository()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryItemRepository>();
        mockRepository
            .Setup(repo => repo.UpdateGroceryItem(It.IsAny<GroceryItem>()))
            .ReturnsAsync(new GroceryItem());
        var sut = new GroceryItemService(mockRepository.Object);
        
        // Act
        await sut.UpdateGroceryItem(new GroceryItem());

        // Assert
        mockRepository.Verify(repo => repo.UpdateGroceryItem(It.IsAny<GroceryItem>()), Times.Once);
    }
    
    #endregion

    #region TestDeleteGroceryItem

    [Fact]
    public async Task DeleteGroceryItem_InvokeGroceryItemRepository()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryItemRepository>();
        var sut = new GroceryItemService(mockRepository.Object);
        
        // Act
        await sut.DeleteGroceryItem(string.Empty);
        
        // Arrange
        mockRepository.Verify(repo => repo.DeleteGroceryItem(It.IsAny<string>()), Times.Once);
    }

    #endregion
    
}