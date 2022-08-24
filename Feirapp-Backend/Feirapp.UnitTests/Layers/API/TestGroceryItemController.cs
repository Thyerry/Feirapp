using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Feirapp.API.Controllers;
using Feirapp.Domain.Contracts;
using Feirapp.Domain.Models;
using Feirapp.UnitTests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Feirapp.UnitTests.Layers.API;

public class TestGroceryItemController
{
    #region TestGetAllEndpoint

    [Fact]
    public async Task GetAll_OnSuccess_ReturnsStatusCode200()
    {
        // Arrange
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetAllGroceryItems())
            .ReturnsAsync(GroceryItemFixture.GetGroceryItems());
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        var result = (OkObjectResult)await sut.GetAllGroceryItems();

        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetAll_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetAllGroceryItems())
            .ReturnsAsync(GroceryItemFixture.GetGroceryItems());
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        await sut.GetAllGroceryItems();

        // Assert
        mockGroceryItemService.Verify(service => service.GetAllGroceryItems(), Times.Once);
    }
    
    [Fact]
    public async Task GetAll_OnSuccess_ReturnListOfGroceryItems()
    {
        // Arrange
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetAllGroceryItems())
            .ReturnsAsync(GroceryItemFixture.GetGroceryItems());
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        var result = await sut.GetAllGroceryItems();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult.Value.Should().BeOfType<List<GroceryItem>>();
    }

    [Fact]
    public async Task GetAll_OnNoGroceryItemsFound_Returns404()
    {
        // Arrange
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetAllGroceryItems())
            .ReturnsAsync(new List<GroceryItem>());
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        var result = await sut.GetAllGroceryItems();

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        var objectResult = result as NotFoundResult;
        objectResult.StatusCode.Should().Be(404);
    }

    #endregion

    #region TestGetRandomGroceryItemsEndpoint

    [Fact]
    public async Task GetRandomGroceryItems_OnSuccess_ShouldReturnStatusCode200()
    {
        // Arrange
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetAllGroceryItems())
            .ReturnsAsync(GroceryItemFixture.GetGroceryItems());
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        var result = (OkObjectResult) await sut.GetRandomGroceryItems(1);
        
        // Assert 
        result.StatusCode.Should().Be(200);
    }
    
    [Fact]
    public async Task GetRandomGroceryItems_OnSuccess_ShouldReturnListOfGroceryItems()
    {
        // Arrange
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetRandomGroceryItems(It.IsAny<int>()))
            .ReturnsAsync(GroceryItemFixture.GetGroceryItems());
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        var result = (OkObjectResult) await sut.GetRandomGroceryItems(1);
        
        // Assert 
        result.Should().BeOfType<OkObjectResult>();
        result.Value.Should().BeOfType<List<GroceryItem>>();
    }
    
    [Fact]
    public async Task GetRandomGroceryItems_OnSuccess_ShouldInvokeGroceryItemService()
    {
        // Arrange
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetAllGroceryItems())
            .ReturnsAsync(GroceryItemFixture.GetGroceryItems());
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        await sut.GetRandomGroceryItems(1);
        
        // Assert 
        mockGroceryItemService.Verify(service => service.GetRandomGroceryItems(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetRandomGroceryItems_OnQuantityLessThanOne_ShouldReturnStatus400()
    {
        // Arrange
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetAllGroceryItems())
            .ReturnsAsync(GroceryItemFixture.GetGroceryItems());
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        var result = (BadRequestResult) await sut.GetRandomGroceryItems(0);
        
        // Assert 
        result.Should().BeOfType<BadRequestResult>();
        result.StatusCode.Should().Be(400);
    }
    
    #endregion
    
    #region TestGetGroceryItemById

    [Fact]
    public async Task GetGroceryItemById_OnSuccess_ReturnStatus200()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        mockService
            .Setup(service => service.GetGroceryItemById(It.IsAny<string>()))
            .ReturnsAsync(new GroceryItem());
        var sut = new GroceryItemController(mockService.Object);

        // Act
        var result = (OkObjectResult) await sut.GetGroceryItemById(string.Empty);
        
        // Assert
        result.StatusCode.Should().Be(200);
    }
    
    [Fact]
    public async Task GetGroceryItemById_OnSuccess_ReturnGroceryItem()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        mockService
            .Setup(service => service.GetGroceryItemById(It.IsAny<string>()))
            .ReturnsAsync(new GroceryItem());
        var sut = new GroceryItemController(mockService.Object);

        // Act
        var result = await sut.GetGroceryItemById(string.Empty);
        
        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult.Value.Should().BeOfType<GroceryItem>();
    }
    
    [Fact]
    public async Task GetGroceryItemById_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        mockService
            .Setup(service => service.GetGroceryItemById(It.IsAny<string>()))
            .ReturnsAsync(new GroceryItem());
        
        var sut = new GroceryItemController(mockService.Object);

        // Act
        await sut.GetGroceryItemById(string.Empty);
        
        // Assert
        mockService.Verify(service => service.GetGroceryItemById(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetGroceryItemById_OnNoGroceryItemFound_ReturnStatus404()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        mockService
            .Setup(service => service.GetGroceryItemById(It.IsAny<string>()));
        var sut = new GroceryItemController(mockService.Object);

        // Act
        var result = await sut.GetGroceryItemById(string.Empty);
        
        // Assert
        result.Should().BeOfType<NotFoundResult>();
        var objectResult = (NotFoundResult)result;
        objectResult.StatusCode.Should().Be(404);
    }
    #endregion 
    
    #region TestGetGroceryItemByName

    [Fact]
    public async Task GetGroceryItemByName_OnSuccess_ReturnStatus200()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        mockService
            .Setup(service => service.GetGroceryItemByName(It.IsAny<string>()))
            .ReturnsAsync(GroceryItemFixture.GetGroceryItems());
        var sut = new GroceryItemController(mockService.Object);

        // Act
        var result = (OkObjectResult) await sut.GetGroceryItemsByName(string.Empty);

        // Assert
        result.StatusCode.Should().Be(200);
    }
    
    [Fact]
    public async Task GetGroceryItemByName_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        mockService
            .Setup(service => service.GetGroceryItemByName(It.IsAny<string>()))
            .ReturnsAsync(GroceryItemFixture.GetGroceryItems());
        var sut = new GroceryItemController(mockService.Object);

        // Act
        await sut.GetGroceryItemsByName(string.Empty);

        // Assert
        mockService.Verify(service => service.GetGroceryItemByName(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public async Task GetGroceryItemByName_OnSuccess_ReturnListOfGroceryItems()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        mockService
            .Setup(service => service.GetGroceryItemByName(It.IsAny<string>()))
            .ReturnsAsync(GroceryItemFixture.GetGroceryItems());
        var sut = new GroceryItemController(mockService.Object);

        // Act
        var result = await sut.GetGroceryItemsByName(string.Empty);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult.Value.Should().BeOfType<List<GroceryItem>>();
    }
    
    [Fact]
    public async Task GetGroceryItemByName_OnNoGroceryItemsFound_ReturnListOfGroceryItems()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        mockService
            .Setup(service => service.GetGroceryItemByName(It.IsAny<string>()))
            .ReturnsAsync(new List<GroceryItem>());
        var sut = new GroceryItemController(mockService.Object);

        // Act
        var result = await sut.GetGroceryItemsByName(string.Empty);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        var objectResult = (NotFoundResult)result;
        objectResult.StatusCode.Should().Be(404);
    }

    #endregion
    
    #region TestCreateGroceryItem

    [Fact]
    public async Task CreateGroceryItem_OnSuccess_ReturnStatusCode201()
    {
        // Assert
        var mockGroceryService = new Mock<IGroceryItemService>();
        var sut = new GroceryItemController(mockGroceryService.Object);

        // Act
        var result = (CreatedResult)await sut.CreateGroceryItem(new GroceryItem());

        // Assert
        result.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task CreateGroceryItem_OnSuccess_ReturnGroceryItem()
    {
        // Arrange
        var mockGroceryService = new Mock<IGroceryItemService>();
        mockGroceryService
            .Setup(service => service.CreateGroceryItem(It.IsAny<GroceryItem>()))
            .ReturnsAsync(new GroceryItem());
        var sut = new GroceryItemController(mockGroceryService.Object);

        // Act
        var result = await sut.CreateGroceryItem(new GroceryItem());
        
        // Assert
        result.Should().BeOfType<CreatedResult>();
        var objectRetult = (CreatedResult)result;
        objectRetult.Value.Should().BeOfType<GroceryItem>();
    }
    
    [Fact]
    public async Task CreateGroceryItem_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var mockGroceryService = new Mock<IGroceryItemService>();
        mockGroceryService
            .Setup(service => service.CreateGroceryItem(It.IsAny<GroceryItem>()))
            .ReturnsAsync(new GroceryItem());
        var sut = new GroceryItemController(mockGroceryService.Object);

        // Act
        await sut.CreateGroceryItem(new GroceryItem());
        
        // Assert
        mockGroceryService.Verify(
            service => service.CreateGroceryItem(It.IsAny<GroceryItem>()),
            Times.Once
            );
    }
    
    #endregion

    #region TestUpdateGroceryItem

    [Fact]
    public async Task UpdateGroceryItem_OnSuccess_ReturnStatus200()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        var sut = new GroceryItemController(mockService.Object);
        
        // Act
        var result = (OkObjectResult) await sut.UpdateGroceryItem(new GroceryItem());
        
        // Assert
        result.StatusCode.Should().Be(200);
    }
    
    [Fact]
    public async Task UpdateGroceryItem_OnSuccess_ReturnUpdatedGroceryItem()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        mockService
            .Setup(service => service.UpdateGroceryItem(It.IsAny<GroceryItem>()))
            .ReturnsAsync(new GroceryItem());
        var sut = new GroceryItemController(mockService.Object);
        
        // Act
        var result = await sut.UpdateGroceryItem(new GroceryItem());
        
        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult.Value.Should().BeOfType<GroceryItem>();
    }
    
    [Fact]
    public async Task UpdateGroceryItem_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        mockService
            .Setup(service => service.UpdateGroceryItem(It.IsAny<GroceryItem>()))
            .ReturnsAsync(new GroceryItem());
        var sut = new GroceryItemController(mockService.Object);
        
        // Act
        await sut.UpdateGroceryItem(new GroceryItem());
        
        // Assert
        mockService.Verify(service => service.UpdateGroceryItem(It.IsAny<GroceryItem>()));
    }
    
    #endregion

    #region TestDeleteGroceryItem

    [Fact]
    public async Task DeleteGroceryItem_OnSuccess_ReturnStatus200()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        var sut = new GroceryItemController(mockService.Object);
        
        // Act
        var result = (OkResult) await sut.DeleteGroceryItem(new string('*',10));

        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task DeleteGroceryItem_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        var sut = new GroceryItemController(mockService.Object);

        // Act
        await sut.DeleteGroceryItem(new string('*',10));

        // Assert
        mockService.Verify(service => service.DeleteGroceryItem(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteGroceryItem_OnIdNullOrEmpty_ReturnStatus400()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        var sut = new GroceryItemController(mockService.Object);

        // Act
        var result = await sut.DeleteGroceryItem(string.Empty);
        
        // Assert
        result.Should().BeOfType<BadRequestResult>();
        result.As<BadRequestResult>().StatusCode.Should().Be(400);
    }

    #endregion
}