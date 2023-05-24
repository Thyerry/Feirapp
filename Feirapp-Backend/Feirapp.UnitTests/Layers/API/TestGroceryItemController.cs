using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    [Description("Tests to the GetAll method")]
    public class TestGetAllGroceryItems
    {
        [Fact]
        public async Task OnSuccess_ReturnsStatusCode200()
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
        public async Task OnSuccess_InvokeGroceryItemService()
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
        public async Task OnSuccess_ReturnListOfGroceryItems()
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
        public async Task OnNoGroceryItemsFound_Returns404()
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
    }

    public class TestGetRandomGroceryItems
    {
        [Fact]
        public async Task OnSuccess_ShouldReturnStatusCode200()
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
        public async Task OnSuccess_ShouldReturnListOfGroceryItems()
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
        public async Task OnSuccess_ShouldInvokeGroceryItemService()
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
        public async Task OnQuantityLessThanOne_ShouldReturnStatus400()
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
    }

    public class TestGetGroceryItemById
    {
        [Fact]
        public async Task OnSuccess_ReturnStatus200()
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
        public async Task OnSuccess_ReturnGroceryItem()
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
        public async Task OnSuccess_InvokeGroceryItemService()
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
        public async Task OnNoGroceryItemFound_ReturnStatus404()
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
    }

    public class TestGetGroceryItemsByName 
    {
        [Fact]
        public async Task OnSuccess_ReturnStatus200()
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
        public async Task OnSuccess_InvokeGroceryItemService()
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
        public async Task OnSuccess_ReturnListOfGroceryItems()
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
        public async Task OnNoGroceryItemsFound_ReturnListOfGroceryItems()
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
    }

    public class TestCreateGroceryItem
    {
        [Fact]
        public async Task OnSuccess_ReturnStatusCode201()
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
        public async Task OnSuccess_ReturnGroceryItem()
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
        public async Task OnSuccess_InvokeGroceryItemService()
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
    }

    public class TestUpdateGroceryItem
    {
        [Fact]
        public async Task OnSuccess_ReturnStatus200()
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
        public async Task OnSuccess_ReturnUpdatedGroceryItem()
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
        public async Task OnSuccess_InvokeGroceryItemService()
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
    }

    public class TestDeleteGroceryItem
    {
        [Fact]
        public async Task OnSuccess_ReturnStatus200()
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
        public async Task OnSuccess_InvokeGroceryItemService()
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
        public async Task OnIdNullOrEmpty_ReturnStatus400()
        {
            // Arrange
            var mockService = new Mock<IGroceryItemService>();
            var sut = new GroceryItemController(mockService.Object);

            // Act
            var result = await sut.DeleteGroceryItem(string.Empty);
        
            // Assert
            result.Should().BeOfType<BadRequestResult>();
            result.As<BadRequestResult>().StatusCode.Should().Be(400);	// Output: "False"
        }
    }
}