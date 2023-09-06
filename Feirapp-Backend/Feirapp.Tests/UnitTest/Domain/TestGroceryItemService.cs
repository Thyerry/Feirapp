using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Feirapp.Domain.Contracts.Repository;
using Feirapp.Domain.Models;
using Feirapp.Domain.Services;
using Feirapp.Entities;
using Feirapp.Tests.Fixtures;
using FluentAssertions;
using FluentValidation;
using Moq;
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
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<GroceryItem>());
            var sut = new GroceryItemService(mockGroceryItemRepository.Object);

            // Act
            var result = await sut.GetAllGroceryItems();

            // Assert
            result.Should().BeOfType<List<GroceryItemModel>>();
        }

        [Fact]
        public async Task GetAllGroceryItems_InvokeGroceryItemRepository()
        {
            // Arrange
            var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
            mockGroceryItemRepository
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<GroceryItem>());
            var sut = new GroceryItemService(mockGroceryItemRepository.Object);

            // Act
            await sut.GetAllGroceryItems();

            // Assert
            mockGroceryItemRepository.Verify(
                repository => repository.GetAllAsync(),
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
                .Setup(repository => repository.GetRandomGroceryItems(It.IsAny<int>()))
                .ReturnsAsync(new List<GroceryItem>());
            var sut = new GroceryItemService(mockGroceryItemRepository.Object);

            // Act
            var result = await sut.GetRandomGroceryItems(new int());

            // Assert
            result.Should().BeOfType<List<GroceryItemModel>>();
        }

        [Fact]
        public async Task GetRandomGroceryItems_ShouldInvokeRepository()
        {
            // Arrange
            var mockGroceryItemRepository = new Mock<IGroceryItemRepository>();
            mockGroceryItemRepository
                .Setup(repository => repository.GetRandomGroceryItems(It.IsAny<int>()))
                .ReturnsAsync(new List<GroceryItem>());
            var sut = new GroceryItemService(mockGroceryItemRepository.Object);

            // Act
            await sut.GetRandomGroceryItems(new int());

            // Assert
            mockGroceryItemRepository.Verify(repo => repo.GetRandomGroceryItems(It.IsAny<int>()), Times.Once);
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
                .Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new GroceryItem());
            var sut = new GroceryItemService(mockRepository.Object);

            // Act
            var result = await sut.GetGroceryItemById(string.Empty);

            // Assert
            result.Should().BeOfType<GroceryItemModel>();
        }

        [Fact]
        public async Task GetGroceryItemById_InvokeGroceryItemRepository()
        {
            // Arrange
            var mockRepository = new Mock<IGroceryItemRepository>();
            mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new GroceryItem());
            var sut = new GroceryItemService(mockRepository.Object);

            // Act
            await sut.GetGroceryItemById(string.Empty);

            // Assert
            mockRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<string>()), Times.Once);
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
                .Setup(repo => repo.InsertAsync(It.IsAny<GroceryItem>()))
                .ReturnsAsync(new GroceryItem());
            var sut = new GroceryItemService(mockRepository.Object);
            var groceryItem = GroceryItemFixture.GetGroceryItems().FirstOrDefault();

            // Act
            var result = await sut.CreateGroceryItem(groceryItem!);

            // Assert
            result.Should().BeOfType<GroceryItemModel>();
        }

        [Fact]
        public async Task ValidGroceryItem_InvokeGroceryItemRepository()
        {
            // Arrange
            var mockRepository = new Mock<IGroceryItemRepository>();
            mockRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<GroceryItem>()))
                .ReturnsAsync(new GroceryItem());
            var sut = new GroceryItemService(mockRepository.Object);
            var groceryItem = GroceryItemFixture.GetGroceryItems().FirstOrDefault();
            // Act
            await sut.CreateGroceryItem(groceryItem!);

            // Assert
            mockRepository.Verify(repo => repo.InsertAsync(It.IsAny<GroceryItem>()), Times.Once);
        }

        [Fact]
        public async Task InvalidGroceryItem_RaiseValidationException()
        {
            // Arrange
            var mockRepository = new Mock<IGroceryItemRepository>();
            mockRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<GroceryItem>()))
                .ReturnsAsync(new GroceryItem());
            var sut = new GroceryItemService(mockRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => sut.CreateGroceryItem(new GroceryItemModel()));
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
                .Setup(repo => repo.UpdateAsync(It.IsAny<GroceryItem>()));
            var sut = new GroceryItemService(mockRepository.Object);

            // Act
            await sut.UpdateGroceryItem(groceryItemModel);

            // Assert
            mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<GroceryItem>()), Times.Once);
        }

        [Fact]
        public async Task OnInvalidValidGroceryItem_ThrowValidationException()
        {
            // Arrange
            var mockRepository = new Mock<IGroceryItemRepository>();
            var sut = new GroceryItemService(mockRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => sut.UpdateGroceryItem(new GroceryItemModel()));
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
            await sut.DeleteGroceryItem(string.Empty);

            // Arrange
            mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<string>()), Times.Once);
        }
    }
}