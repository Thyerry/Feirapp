using Feirapp.API.Controllers;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Tests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Feirapp.Tests.UnitTest.API;

public class GroceryItemControllerTests
{
    private const string ValidGroceryItemId = "123456789012345678901234";

    #region GetAll

    [Fact]
    public async Task GetAll_OnSuccess_ReturnsStatusCode200()
    {
        // Arrange
        var groceryItems = GroceryItemFixture.CreateGroceryItemsModels(10);
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(groceryItems);
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        var result = (OkObjectResult)await sut.GetAll();

        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetAll_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var groceryItems = GroceryItemFixture.CreateGroceryItemsModels(10);
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(groceryItems);
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        await sut.GetAll();

        // Assert
        mockGroceryItemService.Verify(service => service.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_OnSuccess_ReturnListOfGroceryItems()
    {
        // Arrange
        var groceryItems = GroceryItemFixture.CreateGroceryItemsModels(10);
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(groceryItems);
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        var result = await sut.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.Value.Should().BeOfType<List<GroceryItemDto>>();
    }

    [Fact]
    public async Task GetAll_OnNoGroceryItemsFound_Returns404()
    {
        // Arrange
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GroceryItemDto>());
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        var result = await sut.GetAll();

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        var objectResult = result as NotFoundResult;
        objectResult?.StatusCode.Should().Be(404);
    }

    #endregion GetAll

    #region GetRandomGroceryItems

    [Fact]
    public async Task GetRandomGroceryItems_OnSuccess_ShouldReturnStatusCode200()
    {
        // Arrange
        var groceryItems = GroceryItemFixture.CreateGroceryItemsModels(10);
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(groceryItems);
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        var result = (OkObjectResult)await sut.GetRandomGroceryItems(1);

        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetRandomGroceryItems_OnSuccess_ShouldReturnListOfGroceryItems()
    {
        // Arrange
        var groceryItems = GroceryItemFixture.CreateGroceryItemsModels(10);
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetRandomGroceryItemsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groceryItems);
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        var result = (OkObjectResult)await sut.GetRandomGroceryItems(1);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.Value.Should().BeOfType<List<GroceryItemDto>>();
    }

    [Fact]
    public async Task GetRandomGroceryItems_OnSuccess_ShouldInvokeGroceryItemService()
    {
        // Arrange
        var groceryItems = GroceryItemFixture.CreateGroceryItemsModels(10);
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(groceryItems);
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        await sut.GetRandomGroceryItems(1);

        // Assert
        mockGroceryItemService.Verify(
            service => service.GetRandomGroceryItemsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetRandomGroceryItems_OnQuantityLessThanOne_ShouldReturnStatus400()
    {
        // Arrange
        var groceryItems = GroceryItemFixture.CreateGroceryItemsModels(10);
        var mockGroceryItemService = new Mock<IGroceryItemService>();
        mockGroceryItemService
            .Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(groceryItems);
        var sut = new GroceryItemController(mockGroceryItemService.Object);

        // Act
        var result = (BadRequestResult)await sut.GetRandomGroceryItems(0);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
        result.StatusCode.Should().Be(400);
    }

    #endregion GetRandomGroceryItems

    #region GetById

    [Fact]
    public async Task GetById_OnSuccess_ReturnStatus200()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        mockService
            .Setup(service => service.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GroceryItemDto());
        var sut = new GroceryItemController(mockService.Object);

        // Act
        var result = (OkObjectResult)await sut.GetById(ValidGroceryItemId);

        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetById_OnSuccess_ReturnGroceryItem()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        mockService
            .Setup(service => service.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GroceryItemDto());
        var sut = new GroceryItemController(mockService.Object);

        // Act
        var result = await sut.GetById(ValidGroceryItemId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult.Value.Should().BeOfType<GroceryItemDto>();
    }

    [Fact]
    public async Task GetById_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        mockService
            .Setup(service => service.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GroceryItemDto());

        var sut = new GroceryItemController(mockService.Object);

        // Act
        await sut.GetById(ValidGroceryItemId);

        // Assert
        mockService.Verify(service => service.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_OnNoGroceryItemFound_ReturnStatus404()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        mockService
            .Setup(service => service.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>()));
        var sut = new GroceryItemController(mockService.Object);

        // Act
        var result = await sut.GetById(ValidGroceryItemId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        var objectResult = result as NotFoundResult;
        objectResult?.StatusCode.Should().Be(404);
    }

    #endregion GetById

    #region Create

    [Fact]
    public async Task Create_OnSuccess_ReturnStatusCode201()
    {
        // Assert
        var mockGroceryService = new Mock<IGroceryItemService>();
        var sut = new GroceryItemController(mockGroceryService.Object);

        // Act
        var result = await sut.Insert(new GroceryItemDto());

        // Assert
        result.Should().BeOfType<CreatedResult>();
        var objectResult = result as CreatedResult;
        objectResult?.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task Create_OnSuccess_ReturnGroceryItem()
    {
        // Arrange
        var mockGroceryService = new Mock<IGroceryItemService>();
        mockGroceryService
            .Setup(service => service.InsertAsync(It.IsAny<GroceryItemDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GroceryItemDto());
        var sut = new GroceryItemController(mockGroceryService.Object);

        // Act
        var result = await sut.Insert(new GroceryItemDto());

        // Assert
        result.Should().BeOfType<CreatedResult>();
        var objectResult = (CreatedResult)result;
        objectResult.Value.Should().BeOfType<GroceryItemDto>();
    }

    [Fact]
    public async Task Create_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var mockGroceryService = new Mock<IGroceryItemService>();
        mockGroceryService
            .Setup(service => service.InsertAsync(It.IsAny<GroceryItemDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GroceryItemDto());
        var sut = new GroceryItemController(mockGroceryService.Object);

        // Act
        await sut.Insert(new GroceryItemDto());

        // Assert
        mockGroceryService.Verify(
            service => service.InsertAsync(It.IsAny<GroceryItemDto>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    #endregion Create

    #region Update

    [Fact]
    public async Task Update_OnSuccess_ReturnStatus202()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        var sut = new GroceryItemController(mockService.Object);

        // Act
        var result = (AcceptedResult)await sut.Update(new GroceryItemDto());

        // Assert
        result.StatusCode.Should().Be(202);
    }

    [Fact]
    public async Task Update_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        mockService
            .Setup(service => service.UpdateAsync(It.IsAny<GroceryItemDto>(), It.IsAny<CancellationToken>()));
        var sut = new GroceryItemController(mockService.Object);

        // Act
        await sut.Update(new GroceryItemDto());

        // Assert
        mockService.Verify(service => service.UpdateAsync(It.IsAny<GroceryItemDto>(), It.IsAny<CancellationToken>()));
    }

    #endregion Update

    #region Delete

    [Fact]
    public async Task Delete_OnSuccess_ReturnStatus202()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        var sut = new GroceryItemController(mockService.Object);

        // Act
        var result = (AcceptedResult)await sut.Delete(new string('*', 10));

        // Assert
        result.StatusCode.Should().Be(202);
    }

    [Fact]
    public async Task Delete_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        var sut = new GroceryItemController(mockService.Object);

        // Act
        await sut.Delete(new string('*', 10));

        // Assert
        mockService.Verify(service => service.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Delete_OnIdNullOrEmpty_ReturnStatus400()
    {
        // Arrange
        var mockService = new Mock<IGroceryItemService>();
        var sut = new GroceryItemController(mockService.Object);

        // Act
        var result = await sut.Delete(string.Empty);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
        result.As<BadRequestResult>().StatusCode.Should().Be(400);
    }

    #endregion Delete
}