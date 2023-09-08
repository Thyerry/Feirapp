using Feirapp.Domain.Contracts.Repository;
using Feirapp.Domain.Models;
using Feirapp.Domain.Services;
using Feirapp.Entities;
using FluentAssertions;
using FluentValidation;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Feirapp.Tests.UnitTest.Domain;

public class GroceryCategoryServiceTest
{
    private const string ValidId = "123456789012345678901234";
    internal readonly List<GroceryCategory> GroceryCategoryEntityList = new List<GroceryCategory>();
    internal readonly GroceryCategory GroceryCategoryEntity = new GroceryCategory();
    internal readonly GroceryCategoryModel InvalidValidGroceryCategoryModel = new GroceryCategoryModel();

    internal readonly GroceryCategoryModel ValidGroceryCategoryModel = new GroceryCategoryModel()
    {
        Cest = "1234567",
        Description = "description",
        ItemNumber = "1.0",
        Ncm = "12345678",
        Name = "Test Product"
    };

    #region GetAllAsync

    [Fact]
    public async Task GetAllAsync_ReturnListOfGroceryCategories()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryCategoryRepository>();

        var sut = new GroceryCategoryService(mockRepository.Object);
        mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(GroceryCategoryEntityList);
        // Act
        var result = await sut.GetAllAsync(CancellationToken.None);

        // Assert
        result.Should().BeOfType<List<GroceryCategoryModel>>();
    }

    [Fact]
    public async Task GetAllAsync_InvokeGroceryCategoryRepository()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryCategoryRepository>();
        mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(GroceryCategoryEntityList);
        var sut = new GroceryCategoryService(mockRepository.Object);

        // Act
        await sut.GetAllAsync(CancellationToken.None);

        // Assert
        mockRepository.Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion GetAllAsync

    #region GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_ReturnGroceryCategories()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryCategoryRepository>();

        var sut = new GroceryCategoryService(mockRepository.Object);
        mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GroceryCategoryEntity);
        // Act
        var result = await sut.GetByIdAsync(ValidId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<GroceryCategoryModel>();
    }

    [Fact]
    public async Task GetByIdAsync_InvokeGroceryCategoryRepository()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryCategoryRepository>();

        var sut = new GroceryCategoryService(mockRepository.Object);
        mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GroceryCategoryEntity);
        // Act
        await sut.GetByIdAsync(ValidId, CancellationToken.None);

        // Assert
        mockRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion GetByIdAsync

    #region InsertAsync

    [Fact]
    public async Task InsertAsync_OnValidGroceryCategory_ReturnGroceryCategories()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryCategoryRepository>();

        var sut = new GroceryCategoryService(mockRepository.Object);
        mockRepository.Setup(repo => repo.InsertAsync(It.IsAny<GroceryCategory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GroceryCategoryEntity);
        // Act
        var result = await sut.InsertAsync(ValidGroceryCategoryModel, CancellationToken.None);

        // Assert
        result.Should().BeOfType<GroceryCategoryModel>();
    }

    [Fact]
    public async Task InsertAsync_OnValidGroceryCategory_InvokeGroceryCategoryRepository()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryCategoryRepository>();

        var sut = new GroceryCategoryService(mockRepository.Object);
        mockRepository.Setup(repo => repo.InsertAsync(It.IsAny<GroceryCategory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GroceryCategoryEntity);
        // Act
        await sut.InsertAsync(ValidGroceryCategoryModel, CancellationToken.None);

        // Assert
        mockRepository.Verify(repo => repo.InsertAsync(It.IsAny<GroceryCategory>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_OnInvalidGroceryCategory_ThrowValidationException()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryCategoryRepository>();

        var sut = new GroceryCategoryService(mockRepository.Object);
        mockRepository.Setup(repo => repo.InsertAsync(It.IsAny<GroceryCategory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GroceryCategoryEntity);
        // Act
        var act = async () => await sut.InsertAsync(InvalidValidGroceryCategoryModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    #endregion InsertAsync

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_OnValidGroceryCategory_InvokeGroceryCategoryRepository()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryCategoryRepository>();

        var sut = new GroceryCategoryService(mockRepository.Object);
        mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<GroceryCategory>(), It.IsAny<CancellationToken>()));
        // Act
        await sut.UpdateAsync(ValidGroceryCategoryModel, CancellationToken.None);

        // Assert
        mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<GroceryCategory>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_OnInvalidGroceryCategory_ThrowValidationException()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryCategoryRepository>();

        var sut = new GroceryCategoryService(mockRepository.Object);
        mockRepository.Setup(repo => repo.InsertAsync(It.IsAny<GroceryCategory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GroceryCategoryEntity);
        // Act
        var act = async () => await sut.UpdateAsync(InvalidValidGroceryCategoryModel, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    #endregion UpdateAsync

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_OnValidGroceryCategory_InvokeGroceryCategoryRepository()
    {
        // Arrange
        var mockRepository = new Mock<IGroceryCategoryRepository>();

        var sut = new GroceryCategoryService(mockRepository.Object);
        mockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));
        // Act
        await sut.DeleteAsync(ValidId, CancellationToken.None);

        // Assert
        mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion DeleteAsync
}