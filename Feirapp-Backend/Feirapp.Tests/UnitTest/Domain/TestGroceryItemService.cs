using Feirapp.Domain.Contracts.Repository;
using Feirapp.Domain.Models;
using Feirapp.Domain.Services;
using Feirapp.Entities;
using Feirapp.Tests.Fixtures;
using FluentAssertions;
using FluentValidation;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Feirapp.Tests.UnitTest.Domain;

public class TestGroceryItemService
{
    public class TestGetAllGroceryItems
    {
        [Fact]
        public async Task GetAllGroceryItems_ReturnListOfGroceryItems()
        {
            // Arrange
            var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
            mockGroceryItemRepository
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<GroceryItem>());
            var sut = new GroceryItemService(mockGroceryItemRepository.Object);

            // Act
            var result = await sut.GetAllAsync(CancellationToken.None);

            // Assert
            result.Should().BeOfType<List<GroceryItemModel>>();
        }

        [Fact]
        public async Task GetAllGroceryItems_InvokeGroceryItemRepository()
        {
            // Arrange
            var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
            mockGroceryItemRepository
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<GroceryItem>());
            var sut = new GroceryItemService(mockGroceryItemRepository.Object);

            // Act
            await sut.GetAllAsync(CancellationToken.None);

            // Assert
            mockGroceryItemRepository.Verify(
                repository => repository.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
    }

    public class TestGetRandomGroceryItems
    {
        [Fact]
        public async Task GetRandomGroceryItems_ShouldReturnListOfGroceryItems()
        {
            // Arrange
            var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
            mockGroceryItemRepository
                .Setup(repository => repository.GetRandomGroceryItems(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<GroceryItem>());
            var sut = new GroceryItemService(mockGroceryItemRepository.Object);

            // Act
            var result = await sut.GetRandomGroceryItemsAsync(new int(), CancellationToken.None);

            // Assert
            result.Should().BeOfType<List<GroceryItemModel>>();
        }

        [Fact]
        public async Task GetRandomGroceryItems_ShouldInvokeRepository()
        {
            // Arrange
            var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
            mockGroceryItemRepository
                .Setup(repository => repository.GetRandomGroceryItems(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<GroceryItem>());
            var sut = new GroceryItemService(mockGroceryItemRepository.Object);

            // Act
            await sut.GetRandomGroceryItemsAsync(new int(), CancellationToken.None);

            // Assert
            mockGroceryItemRepository.Verify(repo => repo.GetRandomGroceryItems(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    public class TestGetGroceryItemById
    {
        [Fact]
        public async Task GetGroceryItemById_ReturnGroceryItem()
        {
            // Arrange
            var mockRepository = new Mock<IGroceryItemRepository>();
            mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GroceryItem());
            var sut = new GroceryItemService(mockRepository.Object);

            // Act
            var result = await sut.GetById(string.Empty, CancellationToken.None);

            // Assert
            result.Should().BeOfType<GroceryItemModel>();
        }

        [Fact]
        public async Task GetGroceryItemById_InvokeGroceryItemRepository()
        {
            // Arrange
            var mockRepository = new Mock<IGroceryItemRepository>();
            mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GroceryItem());
            var sut = new GroceryItemService(mockRepository.Object);

            // Act
            await sut.GetById(string.Empty, It.IsAny<CancellationToken>());

            // Assert
            mockRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    public class TestCreateGroceryItem
    {
        [Fact]
        public async Task ValidGroceryItem_ReturnGroceryItem()
        {
            // Arrange
            var mockRepository = new Mock<IGroceryItemRepository>();
            mockRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<GroceryItem>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GroceryItem());
            var sut = new GroceryItemService(mockRepository.Object);
            var groceryItem = GroceryItemFixture.GetGroceryItems().FirstOrDefault();

            // Act
            var result = await sut.InsertAsync(groceryItem!, CancellationToken.None);

            // Assert
            result.Should().BeOfType<GroceryItemModel>();
        }

        [Fact]
        public async Task ValidGroceryItem_InvokeGroceryItemRepository()
        {
            // Arrange
            var mockRepository = new Mock<IGroceryItemRepository>();
            mockRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<GroceryItem>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GroceryItem());
            var sut = new GroceryItemService(mockRepository.Object);
            var groceryItem = GroceryItemFixture.GetGroceryItems().FirstOrDefault();
            // Act
            await sut.InsertAsync(groceryItem!, CancellationToken.None);

            // Assert
            mockRepository.Verify(repo => repo.InsertAsync(It.IsAny<GroceryItem>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task InvalidGroceryItem_RaiseValidationException()
        {
            // Arrange
            var mockRepository = new Mock<IGroceryItemRepository>();
            mockRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<GroceryItem>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GroceryItem());
            var sut = new GroceryItemService(mockRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => sut.InsertAsync(new GroceryItemModel(), CancellationToken.None));
        }
    }

    public class TestUpdateGroceryItem
    {
        [Fact]
        public async Task OnValidGroceryItem_InvokeGroceryItemRepository()
        {
            // Arrange
            var groceryItemModel = GroceryItemFixture.GetGroceryItems().FirstOrDefault()!;
            var mockRepository = new Mock<IGroceryItemRepository>();
            mockRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<GroceryItem>(), It.IsAny<CancellationToken>()));
            var sut = new GroceryItemService(mockRepository.Object);

            // Act
            await sut.UpdateAsync(groceryItemModel, CancellationToken.None);

            // Assert
            mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<GroceryItem>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task OnInvalidValidGroceryItem_ThrowValidationException()
        {
            // Arrange
            var mockRepository = new Mock<IGroceryItemRepository>();
            var sut = new GroceryItemService(mockRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => sut.UpdateAsync(new GroceryItemModel(), CancellationToken.None));
        }
    }

    public class TestDeleteGroceryItem
    {
        [Fact]
        public async Task DeleteGroceryItem_InvokeGroceryItemRepository()
        {
            // Arrange
            var mockRepository = new Mock<IGroceryItemRepository>();
            var sut = new GroceryItemService(mockRepository.Object);

            // Act
            await sut.DeleteAsync(string.Empty, CancellationToken.None);

            // Arrange
            mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}