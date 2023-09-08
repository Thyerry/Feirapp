using Feirapp.API.Controllers;
using Feirapp.Domain.Contracts.Service;
using Feirapp.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Feirapp.Tests.UnitTest.API;

public class GroceryCategoryControllerTest
{
    private readonly GroceryCategoryModel GroceryCategoryModel = new GroceryCategoryModel();
    private const string ValidGroceryCategoryId = "groceryCategoryId";
    private const string BadRequestMessage = "Invalid id";
    private const string EmptyString = "";
    private const int OkStatusCode = 200;
    private const int CreatedStatusCode = 201;
    private const int AcceptedStatusCode = 202;
    private const int BadRequestStatusCode = 400;
    private const int NotFoundStatusCode = 404;

    #region GetAll

    [Fact]
    public async Task GetAll_OnSuccess_ReturnStatusCode200()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        mockService.Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()));

        var sut = new GroceryCategoryController(mockService.Object);

        // Act
        var result = await sut.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult.StatusCode.Should().Be(OkStatusCode);
    }

    [Fact]
    public async Task GetAll_OnSuccess_ReturnListOfGroceryCategories()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        mockService.Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<GroceryCategoryModel>());

        var sut = new GroceryCategoryController(mockService.Object);

        // Act
        var result = (OkObjectResult)await sut.GetAll();

        // Assert
        result.Value.Should().BeOfType<List<GroceryCategoryModel>>();
    }

    [Fact]
    public async Task GetAll_InvokeGroceryCategoryService()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        mockService.Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()));

        var sut = new GroceryCategoryController(mockService.Object);
        // Act
        await sut.GetAll();
        // Assert
        mockService.Verify(service => service.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    #endregion GetAll

    #region GetById

    [Fact]
    public async Task GetById_OnSuccess_ReturnStatusCode200()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        mockService.Setup(service => service.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GroceryCategoryModel);

        var sut = new GroceryCategoryController(mockService.Object);
        // Act
        var result = await sut.GetById(ValidGroceryCategoryId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var statusCode = result as OkObjectResult;
        statusCode.StatusCode.Should().Be(OkStatusCode);
    }

    [Fact]
    public async Task GetById_OnSuccess_ReturnGroceryCategory()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        mockService.Setup(service => service.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GroceryCategoryModel);

        var sut = new GroceryCategoryController(mockService.Object);

        // Act
        var result = (OkObjectResult)await sut.GetById(ValidGroceryCategoryId);

        // Assert
        result.Value.Should().BeOfType<GroceryCategoryModel>();
    }

    [Fact]
    public async Task GetById_OnValidId_InvokeGroceryCategoryService()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        mockService.Setup(service => service.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GroceryCategoryModel);

        var sut = new GroceryCategoryController(mockService.Object);
        // Act
        await sut.GetById(ValidGroceryCategoryId, CancellationToken.None);
        // Assert
        mockService.Verify(service => service.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task GetById_OnInvalidId_ReturnStatus400()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        mockService.Setup(service => service.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GroceryCategoryModel);

        var sut = new GroceryCategoryController(mockService.Object);

        // Act
        var result = await sut.GetById(EmptyString, CancellationToken.None);

        // Assert

        result.Should().BeOfType<BadRequestObjectResult>();
        var objectResult = result as BadRequestObjectResult;
        objectResult.StatusCode.Should().Be(BadRequestStatusCode);
        objectResult.Value.Should().Be(BadRequestMessage);
    }

    [Fact]
    public async Task GetById_OnResultNull_ReturnStatus404()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        mockService.Setup(service => service.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        var sut = new GroceryCategoryController(mockService.Object);

        // Act
        var result = await sut.GetById(ValidGroceryCategoryId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        var notFoundResult = result as NotFoundResult;
        notFoundResult.StatusCode.Should().Be(NotFoundStatusCode);
    }

    #endregion GetById

    #region Insert

    [Fact]
    public async Task Insert_OnSuccess_ReturnStatusCode201()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        var sut = new GroceryCategoryController(mockService.Object);

        // Act
        var result = await sut.Insert(GroceryCategoryModel, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        var statusCode = result as CreatedResult;
        statusCode.StatusCode.Should().Be(CreatedStatusCode);
    }

    [Fact]
    public async Task Insert_OnSuccess_ReturnGroceryCategoryModel()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        mockService.Setup(service => service.InsertAsync(It.IsAny<GroceryCategoryModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GroceryCategoryModel);

        var sut = new GroceryCategoryController(mockService.Object);

        // Act
        var result = (CreatedResult)await sut.Insert(GroceryCategoryModel, CancellationToken.None);

        // Assert
        result.Value.Should().BeOfType<GroceryCategoryModel>();
    }

    [Fact]
    public async Task Insert_OnValidGroceryCategory_InvokeGroceryCategoryService()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        mockService.Setup(service => service.InsertAsync(It.IsAny<GroceryCategoryModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GroceryCategoryModel);

        var sut = new GroceryCategoryController(mockService.Object);

        // Act
        await sut.Insert(GroceryCategoryModel, CancellationToken.None);

        // Assert
        mockService.Verify(service =>
            service.InsertAsync(It.IsAny<GroceryCategoryModel>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion Insert

    #region Update

    [Fact]
    public async Task Update_OnSuccess_ReturnStatusCode202()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        var sut = new GroceryCategoryController(mockService.Object);

        // Act
        var result = await sut.Update(GroceryCategoryModel, CancellationToken.None);

        // Assert
        result.Should().BeOfType<AcceptedResult>();
        var statusCode = result as AcceptedResult;
        statusCode.StatusCode.Should().Be(AcceptedStatusCode);
    }

    [Fact]
    public async Task Update_OnValidGroceryCategory_InvokeGroceryCategoryService()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        mockService.Setup(service =>
            service.UpdateAsync(It.IsAny<GroceryCategoryModel>(), It.IsAny<CancellationToken>()));

        var sut = new GroceryCategoryController(mockService.Object);

        // Act
        await sut.Update(GroceryCategoryModel, CancellationToken.None);

        // Assert
        mockService.Verify(service =>
            service.UpdateAsync(It.IsAny<GroceryCategoryModel>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

    #endregion Update

    #region Delete

    [Fact]
    public async Task Delete_OnSuccess_ReturnStatusCode202()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        var sut = new GroceryCategoryController(mockService.Object);
        mockService.Setup(service => service.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        // Act
        var result = await sut.Delete(ValidGroceryCategoryId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<AcceptedResult>();
        var statusCode = result as AcceptedResult;
        statusCode.StatusCode.Should().Be(AcceptedStatusCode);
    }

    [Fact]
    public async Task Delete_OnInvalidGroceryCategoryId_ReturnStatusCode202()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        var sut = new GroceryCategoryController(mockService.Object);
        mockService.Setup(service => service.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        // Act
        var result = await sut.Delete(EmptyString, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var statusCode = result as BadRequestObjectResult;
        statusCode.StatusCode.Should().Be(BadRequestStatusCode);
        statusCode.Value.Should().Be(BadRequestMessage);
    }

    [Fact]
    public async Task Delete_OnValidGroceryCategory_InvokeGroceryCategoryService()
    {
        // Arrange
        var mockService = new Mock<IGroceryCategoryService>();
        mockService.Setup(service => service.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        var sut = new GroceryCategoryController(mockService.Object);

        // Act
        await sut.Delete(ValidGroceryCategoryId, CancellationToken.None);

        // Assert
        mockService.Verify(service =>
            service.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    #endregion Delete
}