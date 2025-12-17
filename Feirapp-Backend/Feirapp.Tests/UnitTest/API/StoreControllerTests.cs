using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Feirapp.API.Controllers;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Domain.Services.Stores.Methods.GetStoreById;
using Feirapp.Domain.Services.Stores.Methods.InsertGroceryItem;
using Feirapp.Domain.Services.Stores.Methods.SearchStores;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;
using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.Utils;

namespace Feirapp.Tests.UnitTest.API;

public class StoreControllerTests
{
    #region CreateStore
    
    [Fact]
    public async Task CreateStore_ShouldCallServiceAndReturnCreated()
    {
        // Arrange
        var storeService = Substitute.For<IStoreService>();
        var controller = new StoreController(storeService);
        var request = new InsertStoreRequest();
        var cancellationToken = CancellationToken.None;
        storeService.InsertStoreAsync(request, cancellationToken).Returns(Result<bool>.Ok(true));

        // Act
        var result = await controller.CreateStore(request, cancellationToken);

        // Assert
        await storeService.Received(1).InsertStoreAsync(request, cancellationToken);
        result.Should().BeOfType<CreatedResult>();
        var created = result as CreatedResult;
        created?.Value.Should().BeOfType<ApiResponse<bool>>();
    }

    [Fact]
    public async Task CreateStore_WhenServiceThrowsException_ShouldPropagate()
    {
        // Arrange
        var storeService = Substitute.For<IStoreService>();
        var controller = new StoreController(storeService);
        var request = new InsertStoreRequest();
        var cancellationToken = CancellationToken.None;
        storeService.InsertStoreAsync(request, cancellationToken).Throws(new Exception("fail"));

        // Act 
        var act = async () => await controller.CreateStore(request, cancellationToken);
        
        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    
    #endregion CreateStore

    #region SearchStores

    [Fact]
    public async Task SearchStores_ShouldReturnOk_WhenStoresFound()
    {
        // Arrange
        var storeService = Substitute.For<IStoreService>();
        var controller = new StoreController(storeService);
        var request = new SearchStoresRequest();
        var cancellationToken = CancellationToken.None;
        var stores = new List<SearchStoresResponse> { new(), new() };
        storeService.SearchStoresAsync(request, cancellationToken)
            .Returns(Result<List<SearchStoresResponse>>.Ok(stores));

        // Act
        var result = await controller.SearchStores(request, cancellationToken);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task SearchStores_ShouldReturnNotFound_WhenNoStoresFound()
    {
        // Arrange
        var storeService = Substitute.For<IStoreService>();
        var controller = new StoreController(storeService);
        var request = new SearchStoresRequest();
        var cancellationToken = CancellationToken.None;
        storeService.SearchStoresAsync(request, cancellationToken)
            .Returns(Result<List<SearchStoresResponse>>.Ok([]));

        // Act
        var result = await controller.SearchStores(request, cancellationToken);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task SearchStores_WhenServiceThrowsException_ShouldPropagate()
    {
        // Arrange
        var storeService = Substitute.For<IStoreService>();
        var controller = new StoreController(storeService);
        var request = new SearchStoresRequest();
        var cancellationToken = CancellationToken.None;
        storeService.SearchStoresAsync(request, cancellationToken).Throws(new Exception("fail"));

        // Act
        var act = async () => await controller.SearchStores(request, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    #endregion SearchStores

    #region GetStoreById

    [Fact]
    public async Task GetStoreById_ShouldReturnOk_WhenStoreFound()
    {
        // Arrange
        var storeService = Substitute.For<IStoreService>();
        var controller = new StoreController(storeService);
        var storeId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        var storeResponse = new GetStoreByIdResponse();
        storeService.GetStoreByIdAsync(storeId, cancellationToken)
            .Returns(Result<GetStoreByIdResponse>.Ok(storeResponse));

        // Act
        var result = await controller.GetStoreById(storeId, cancellationToken);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetStoreById_ShouldReturnNotFound_WhenStoreNotFound()
    {
        // Arrange
        var storeService = Substitute.For<IStoreService>();
        var controller = new StoreController(storeService);
        var storeId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        storeService.GetStoreByIdAsync(storeId, cancellationToken)
            .Returns(Result<GetStoreByIdResponse>.Fail("Stores not found."));

        // Act
        var result = await controller.GetStoreById(storeId, cancellationToken);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetStoreById_WhenServiceThrowsException_ShouldPropagate()
    {
        // Arrange
        var storeService = Substitute.For<IStoreService>();
        var controller = new StoreController(storeService);
        var storeId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        storeService.GetStoreByIdAsync(storeId, cancellationToken).Throws(new Exception("fail"));

        // Act
        var act = async () => await controller.GetStoreById(storeId, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    #endregion GetStoreById
}