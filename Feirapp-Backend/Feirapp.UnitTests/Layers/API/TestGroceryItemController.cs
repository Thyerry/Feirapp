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

public class GroceryItemControllerTests
{
    [Fact]
    public async Task Get_OnSuccess_ReturnsStatusCode200()
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
    public async Task Get_OnSuccess_InvokeGroceryItemService()
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
    public async Task Get_OnSuccess_ReturnListOfGroceryItems()
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
    public async Task Get_OnNoGroceryItemsFound_Returns404()
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
        var objectResult = (NotFoundResult)result;
        objectResult.StatusCode.Should().Be(404);
    }
}