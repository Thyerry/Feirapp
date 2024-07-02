//using Feirapp.Domain.Services.GroceryItems.Dtos;
//using Feirapp.Domain.Services.GroceryItems.Implementations;
//using Feirapp.Domain.Services.GroceryItems.Interfaces;
//using Feirapp.Tests.Fixtures;
//using FluentAssertions;
//using FluentValidation;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using Feirapp.Entities.Entities;
//using Xunit;

//namespace Feirapp.Tests.UnitTest.Domain;

//public class GroceryItemServiceTests
//{
//    public static GroceryItem ValidGroceryItem = GroceryItemFixture.CreateRandomGroceryItem();

//    public static GroceryItemDto ValidGroceryItemDto = new GroceryItemDto()
//    {
//        Name = "Name",
//        Price = 30.5m,
//        PurchaseDate = DateTime.UtcNow,
//        Barcode = new string('1', 7),
//        Brand = "brand",
//        Cest = "1212312",
//        Ncm = "1234123412",
//        StoreName = "Store",
//        ImageUrl = "www.jpg",
//    };

//    public static GroceryItemDto GroceryItemModel = new GroceryItemDto();

//    #region GetAllAsync

//    [Fact]
//    public async Task GetAllAsync_ReturnListOfGroceryItems()
//    {
//        // Arrange
//        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
//        mockGroceryItemRepository
//            .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
//            .ReturnsAsync(new List<GroceryItem>());

//        var sut = new GroceryItemService(mockGroceryItemRepository.Object);

//        // Act
//        var result = await sut.GetAllAsync(CancellationToken.None);

//        // Assert
//        result.Should().BeOfType<List<GroceryItemDto>>();
//    }

//    [Fact]
//    public async Task GetAllAsync_InvokeGroceryItemRepository()
//    {
//        // Arrange
//        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
//        mockGroceryItemRepository
//            .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
//            .ReturnsAsync(new List<GroceryItem>());

//        var sut = new GroceryItemService(mockGroceryItemRepository.Object);

//        // Act
//        await sut.GetAllAsync(CancellationToken.None);

//        // Assert
//        mockGroceryItemRepository.Verify(
//            repository => repository.GetAllAsync(It.IsAny<CancellationToken>()),
//            Times.Once
//        );
//    }

//    #endregion GetAllAsync

//    #region GetRandomGroceryItems

//    [Fact]
//    public async Task GetRandomGroceryItems_ShouldReturnListOfGroceryItems()
//    {
//        // Arrange
//        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
//        mockGroceryItemRepository
//            .Setup(repository => repository.GetRandomGroceryItems(It.IsAny<int>(), It.IsAny<CancellationToken>()))
//            .ReturnsAsync(new List<GroceryItem>());

//        var sut = new GroceryItemService(mockGroceryItemRepository.Object);

//        // Act
//        var result = await sut.GetRandomGroceryItemsAsync(new int(), CancellationToken.None);

//        // Assert
//        result.Should().BeOfType<List<GroceryItemDto>>();
//    }

//    [Fact]
//    public async Task GetRandomGroceryItems_ShouldInvokeRepository()
//    {
//        // Arrange
//        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
//        mockGroceryItemRepository
//            .Setup(repository => repository.GetRandomGroceryItems(It.IsAny<int>(), It.IsAny<CancellationToken>()))
//            .ReturnsAsync(new List<GroceryItem>());

//        var sut = new GroceryItemService(mockGroceryItemRepository.Object);

//        // Act
//        await sut.GetRandomGroceryItemsAsync(new int(), CancellationToken.None);

//        // Assert
//        mockGroceryItemRepository.Verify(
//            repo => repo.GetRandomGroceryItems(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
//    }

//    #endregion GetRandomGroceryItems

//    #region GetByIdAsync

//    [Fact]
//    public async Task GetByIdAsync_ReturnGroceryItem()
//    {
//        // Arrange
//        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
//        mockGroceryItemRepository
//            .Setup(repo => repo.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
//            .ReturnsAsync(ValidGroceryItem);

//        var sut = new GroceryItemService(mockGroceryItemRepository.Object);

//        // Act
//        var result = await sut.GetById(ValidGroceryItem.Id, CancellationToken.None);

//        // Assert
//        result.Should().BeOfType<GroceryItemDto>();
//    }

//    [Fact]
//    public async Task GetByIdAsync_InvokeGroceryItemRepository()
//    {
//        // Arrange
//        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
//        mockGroceryItemRepository
//            .Setup(repo => repo.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
//            .ReturnsAsync(ValidGroceryItem);

//        var sut = new GroceryItemService(mockGroceryItemRepository.Object);

//        // Act
//        await sut.GetById(ValidGroceryItem.Id, It.IsAny<CancellationToken>());

//        // Assert
//        mockGroceryItemRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
//            Times.Once);
//    }

//    #endregion GetByIdAsync

//    #region InsertAsync

//    [Fact]
//    public async Task InsertAsync_ValidGroceryItem_ReturnGroceryItem()
//    {
//        // Arrange
//        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
//        mockGroceryItemRepository
//            .Setup(repo => repo.InsertAsync(It.IsAny<GroceryItem>(), It.IsAny<CancellationToken>()))
//            .ReturnsAsync(ValidGroceryItem);

//        var sut = new GroceryItemService(mockGroceryItemRepository.Object);

//        // Act
//        var result = await sut.InsertAsync(ValidGroceryItemDto, CancellationToken.None);

//        // Assert
//        result.Should().BeOfType<GroceryItemDto>();
//    }

//    [Fact]
//    public async Task InsertAsync_ValidGroceryItem_InvokeGroceryItemRepository()
//    {
//        // Arrange
//        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
//        mockGroceryItemRepository
//            .Setup(repo => repo.InsertAsync(It.IsAny<GroceryItem>(), It.IsAny<CancellationToken>()))
//            .ReturnsAsync(ValidGroceryItem);

//        var sut = new GroceryItemService(mockGroceryItemRepository.Object);

//        // Act
//        await sut.InsertAsync(ValidGroceryItemDto, CancellationToken.None);

//        // Assert
//        mockGroceryItemRepository.Verify(
//            repo => repo.InsertAsync(It.IsAny<GroceryItem>(), It.IsAny<CancellationToken>()), Times.Once);
//    }

//    [Fact]
//    public async Task InsertAsync_InvalidGroceryItem_RaiseValidationException()
//    {
//        // Arrange
//        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
//        mockGroceryItemRepository
//            .Setup(repo => repo.InsertAsync(It.IsAny<GroceryItem>(), It.IsAny<CancellationToken>()))
//            .ReturnsAsync(ValidGroceryItem);

//        var sut = new GroceryItemService(mockGroceryItemRepository.Object);

//        // Act
//        var act = async () => await sut.InsertAsync(GroceryItemModel, CancellationToken.None);

//        // Assert
//        await act.Should().ThrowAsync<ValidationException>();
//    }

//    #endregion InsertAsync

//    #region UpdateAsync

//    [Fact]
//    public async Task UpdateAsync_OnValidGroceryItem_InvokeGroceryItemRepository()
//    {
//        // Arrange
//        var groceryItemModel = GroceryItemFixture.CreateRandomGroceryItemModel();
//        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
//        mockGroceryItemRepository
//            .Setup(repo => repo.UpdateAsync(It.IsAny<GroceryItem>(), It.IsAny<CancellationToken>()));

//        var sut = new GroceryItemService(mockGroceryItemRepository.Object);

//        // Act
//        await sut.UpdateAsync(groceryItemModel, CancellationToken.None);

//        // Assert
//        mockGroceryItemRepository.Verify(
//            repo => repo.UpdateAsync(It.IsAny<GroceryItem>(), It.IsAny<CancellationToken>()), Times.Once);
//    }

//    [Fact]
//    public async Task UpdateAsync_OnInvalidValidGroceryItem_ThrowValidationException()
//    {
//        // Arrange
//        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();

//        var sut = new GroceryItemService(mockGroceryItemRepository.Object);

//        // Act
//        var act = async () => await sut.UpdateAsync(GroceryItemModel, CancellationToken.None);

//        // Assert
//        await act.Should().ThrowAsync<ValidationException>();
//    }

//    #endregion UpdateAsync

//    #region DeleteAsync

//    [Fact]
//    public async Task DeleteAsync_InvokeGroceryItemRepository()
//    {
//        // Arrange
//        var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();

//        var sut = new GroceryItemService(mockGroceryItemRepository.Object);

//        // Act
//        await sut.DeleteAsync(string.Empty, CancellationToken.None);

//        // Arrange
//        mockGroceryItemRepository.Verify(repo => repo.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
//            Times.Once);
//    }

//    #endregion DeleteAsync
//}

