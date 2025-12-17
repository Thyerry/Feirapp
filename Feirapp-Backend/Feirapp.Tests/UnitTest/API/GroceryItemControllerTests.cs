using System;
using System.Collections.Generic;
using Feirapp.API.Controllers;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Feirapp.API.Helpers.Response;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Domain.Services.DataScrapper.Methods.InvoiceScan;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemsByStore;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;
using Feirapp.Domain.Services.Utils;
using Xunit;

namespace Feirapp.Tests.UnitTest.API;

public class GroceryItemControllerTests
{
    #region SearchGroceryItems

    [Fact]
    public async Task SearchGroceryItems_OnSuccess_ReturnStatus200()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();

        List<SearchGroceryItemsResponse> response =
        [
            new() { Name = "Arroz" },
            new() { Name = "Feijão" }
        ];

        service
            .SearchAsync(Arg.Any<SearchGroceryItemsRequest>(), Arg.Any<CancellationToken>())
            .Returns(Result<List<SearchGroceryItemsResponse>>.Ok(response));

        var sut = new GroceryItemController(service, invoiceService);

        // Act
        var result = await sut.SearchGroceryItems(new SearchGroceryItemsRequest());

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task SearchGroceryItems_OnSuccess_ReturnGroceryItems()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();

        List<SearchGroceryItemsResponse> expectedResponse =
        [
            new() { Name = "Arroz" },
            new() { Name = "Feijão" }
        ];

        service
            .SearchAsync(Arg.Any<SearchGroceryItemsRequest>(), Arg.Any<CancellationToken>())
            .Returns(Result<List<SearchGroceryItemsResponse>>.Ok(expectedResponse));

        var sut = new GroceryItemController(service, invoiceService);

        // Act
        var result = await sut.SearchGroceryItems(new SearchGroceryItemsRequest());

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult.Value.Should().BeOfType<ApiResponse<List<SearchGroceryItemsResponse>>>();

        var apiResponse = objectResult.Value as ApiResponse<List<SearchGroceryItemsResponse>>;
        apiResponse.Status.Should().Be("success");
        apiResponse.Data.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task SearchGroceryItems_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        var request = new SearchGroceryItemsRequest();
        List<SearchGroceryItemsResponse> response =
        [
            new() { Name = "Arroz" },
            new() { Name = "Feijão" }
        ];

        service
            .SearchAsync(Arg.Any<SearchGroceryItemsRequest>(), Arg.Any<CancellationToken>())
            .Returns(Result<List<SearchGroceryItemsResponse>>.Ok(response));

        var sut = new GroceryItemController(service, invoiceService);

        // Act
        await sut.SearchGroceryItems(request);

        // Assert
        await service.Received(1).SearchAsync(Arg.Is<SearchGroceryItemsRequest>(r => r == request),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SearchGroceryItems_WhenNoItemsFound_ReturnStatus404()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        service
            .SearchAsync(Arg.Any<SearchGroceryItemsRequest>(), Arg.Any<CancellationToken>())
            .Returns(Result<List<SearchGroceryItemsResponse>>.Ok([]));

        var sut = new GroceryItemController(service, invoiceService);

        // Act
        var result = await sut.SearchGroceryItems(new SearchGroceryItemsRequest());

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var objectResult = result as NotFoundObjectResult;
        objectResult?.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task SearchGroceryItems_WhenNoItemsFound_ReturnCorrectErrorMessage()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        service
            .SearchAsync(Arg.Any<SearchGroceryItemsRequest>(), Arg.Any<CancellationToken>())
            .Returns(Result<List<SearchGroceryItemsResponse>>.Ok([]));

        var sut = new GroceryItemController(service, invoiceService);

        // Act
        var result = await sut.SearchGroceryItems(new SearchGroceryItemsRequest());

        // Assert
        var objectResult = result as NotFoundObjectResult;
        var response = objectResult?.Value as ApiResponse<List<SearchGroceryItemsResponse>>;
        response?.Message.Should().Be("No Grocery Items Found");
        response?.Status.Should().Be("failure");
    }

    #endregion SearchGroceryItems

    #region GetById

    [Fact]
    public async Task GetById_OnSuccess_ReturnStatus200()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        var groceryItemResponse = new GetGroceryItemByIdResponse { Name = "Arroz" };
        service
            .GetByIdAsync(Arg.Any<Guid>(), CancellationToken.None)
            .Returns(Result<GetGroceryItemByIdResponse>.Ok(groceryItemResponse));

        var sut = new GroceryItemController(service, invoiceService);

        // Act
        var result = (OkObjectResult)await sut.GetById(Guid.NewGuid());

        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetById_OnSuccess_ReturnGroceryItem()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        var groceryItemResponse = new GetGroceryItemByIdResponse { Name = "Arroz" };

        service
            .GetByIdAsync(Arg.Any<Guid>(), CancellationToken.None)
            .Returns(Result<GetGroceryItemByIdResponse>.Ok(groceryItemResponse));

        var sut = new GroceryItemController(service, invoiceService);

        // Act
        var result = await sut.GetById(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var objectResult = (OkObjectResult)result;
        objectResult.Value.Should().BeOfType<ApiResponse<GetGroceryItemByIdResponse>>();

        var apiResponse = objectResult.Value as ApiResponse<GetGroceryItemByIdResponse>;
        apiResponse.Status.Should().Be("success");
        apiResponse.Data.Should().BeEquivalentTo(groceryItemResponse);
    }

    [Fact]
    public async Task GetById_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        var groceryItemResponse = new GetGroceryItemByIdResponse { Name = "Arroz" };

        service
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetGroceryItemByIdResponse>.Ok(groceryItemResponse));

        var sut = new GroceryItemController(service, invoiceService);

        // Act
        await sut.GetById(Guid.NewGuid());

        // Assert
        await service.Received(1).GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetById_OnNoGroceryItemFound_ReturnStatus404()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        service
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetGroceryItemByIdResponse>.Fail("Grocery item not found"));

        var sut = new GroceryItemController(service, invoiceService);

        // Act
        var result = await sut.GetById(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var objectResult = result as NotFoundObjectResult;
        objectResult?.StatusCode.Should().Be(404);
    }

    #endregion GetById

    #region GetFromStore

    [Fact]
    public async Task GetFromStore_OnSuccess_ReturnStatus200()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();

        var response = new GetGroceryItemsByStoreIdResponse
        {
            Store = new GetGroceryItemsByStoreStoreDto { Name = "Supermercado Central", Id = Guid.NewGuid() },
            Items =
            [
                new GetGroceryItemsByStoreGroceryItemDto { Name = "Arroz" },
                new GetGroceryItemsByStoreGroceryItemDto { Name = "Feijão" }
            ]
        };

        service
            .GetByStoreAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetGroceryItemsByStoreIdResponse>.Ok(response));

        var sut = new GroceryItemController(service, invoiceService);

        // Act
        var result = await sut.GetFromStore(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetFromStore_OnSuccess_ReturnGroceryItems()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        var expectedResponse = new GetGroceryItemsByStoreIdResponse
        {
            Store = new GetGroceryItemsByStoreStoreDto { Name = "Supermercado Central", Id = Guid.NewGuid() },
            Items =
            [
                new GetGroceryItemsByStoreGroceryItemDto { Name = "Arroz" },
                new GetGroceryItemsByStoreGroceryItemDto { Name = "Feijão" }
            ]
        };

        service
            .GetByStoreAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetGroceryItemsByStoreIdResponse>.Ok(expectedResponse));

        var sut = new GroceryItemController(service, invoiceService);

        // Act
        var result = await sut.GetFromStore(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult.Value.Should().BeOfType<ApiResponse<GetGroceryItemsByStoreIdResponse>>();

        var apiResponse = objectResult.Value as ApiResponse<GetGroceryItemsByStoreIdResponse>;
        apiResponse.Status.Should().Be("success");
        apiResponse.Data.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetFromStore_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();

        var storeId = Guid.NewGuid();

        var response = new GetGroceryItemsByStoreIdResponse
        {
            Store = new GetGroceryItemsByStoreStoreDto { Name = "Supermercado Central", Id = Guid.NewGuid() },
            Items =
            [
                new GetGroceryItemsByStoreGroceryItemDto { Name = "Arroz" },
                new GetGroceryItemsByStoreGroceryItemDto { Name = "Feijão" }
            ]
        };

        service
            .GetByStoreAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetGroceryItemsByStoreIdResponse>.Ok(response));

        var sut = new GroceryItemController(service, invoiceService);

        // Act
        await sut.GetFromStore(storeId);

        // Assert
        await service.Received(1).GetByStoreAsync(storeId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetFromStore_WhenStoreNotFound_ReturnStatus404()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        var response = new GetGroceryItemsByStoreIdResponse { Items = [] };

        service
            .GetByStoreAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetGroceryItemsByStoreIdResponse>.Fail("Store not found"));

        var sut = new GroceryItemController(service, invoiceService);

        // Act
        var result = await sut.GetFromStore(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var objectResult = result as NotFoundObjectResult;
        objectResult?.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetFromStore_WhenStoreNotFound_ReturnCorrectErrorMessage()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        service
            .GetByStoreAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetGroceryItemsByStoreIdResponse>.Fail("Store not found"));

        var sut = new GroceryItemController(service, invoiceService);

        // Act
        var result = await sut.GetFromStore(Guid.NewGuid());

        // Assert
        var objectResult = result as NotFoundObjectResult;
        var apiResponse = objectResult?.Value as ApiResponse<GetGroceryItemsByStoreIdResponse>;
        apiResponse?.Message.Should().Be("Store not found");
        apiResponse?.Status.Should().Be("failure");
    }

    #endregion GetFromStore

    #region GetFromInvoice

    [Fact]
    public async Task GetFromInvoice_OnSuccess_ReturnStatus200()
    {
        // Arrange
        var groceryItemService = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        var response = new InvoiceImportResponse
        {
            Items =
            [
                new InvoiceImportGroceryItem { Name = "Arroz" },
                new InvoiceImportGroceryItem { Name = "Feijão" }
            ]
        };

        invoiceService
            .InvoiceImportAsync(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(response);

        var sut = new GroceryItemController(groceryItemService, invoiceService);

        // Act
        var result = await sut.GetFromInvoice("invoice123");

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetFromInvoice_OnSuccess_ReturnInvoiceItems()
    {
        // Arrange
        var groceryItemService = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        var expectedResponse = new InvoiceImportResponse
        {
            Items =
            [
                new InvoiceImportGroceryItem { Name = "Arroz" },
                new InvoiceImportGroceryItem { Name = "Feijão" }
            ]
        };

        invoiceService
            .InvoiceImportAsync(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        var sut = new GroceryItemController(groceryItemService, invoiceService);

        // Act
        var result = await sut.GetFromInvoice("invoice123");

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult.Value.Should().BeOfType<ApiResponse<InvoiceImportResponse>>();

        var apiResponse = objectResult.Value as ApiResponse<InvoiceImportResponse>;
        apiResponse.Status.Should().Be("success");
        apiResponse.Data.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetFromInvoice_OnSuccess_InvokeServiceWithCorrectParameters()
    {
        // Arrange
        var groceryItemService = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();

        var invoiceId = "invoice123";

        var response = new InvoiceImportResponse { Items = [] };

        invoiceService
            .InvoiceImportAsync(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(response);

        var sut = new GroceryItemController(groceryItemService, invoiceService);

        // Act
        await sut.GetFromInvoice(invoiceId);

        // Assert
        await invoiceService.Received(1)
            .InvoiceImportAsync(
                Arg.Is<string>(x => x == invoiceId),
                Arg.Is<bool>(x => x == false),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetFromInvoice_WhenNoItemsFound_ReturnStatus404()
    {
        // Arrange
        var groceryItemService = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        var response = new InvoiceImportResponse { Items = [] };

        invoiceService
            .InvoiceImportAsync(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(response);

        var sut = new GroceryItemController(groceryItemService, invoiceService);

        // Act
        var result = await sut.GetFromInvoice("invoice123");

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var objectResult = result as NotFoundObjectResult;
        objectResult?.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetFromInvoice_WhenNoItemsFound_ReturnCorrectErrorMessage()
    {
        // Arrange
        var groceryItemService = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        var response = new InvoiceImportResponse { Items = [] };

        invoiceService
            .InvoiceImportAsync(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(response);

        var sut = new GroceryItemController(groceryItemService, invoiceService);

        // Act
        var result = await sut.GetFromInvoice("invoice123");

        // Assert
        var objectResult = result as NotFoundObjectResult;
        var apiResponse = objectResult?.Value as ApiResponse<InvoiceImportResponse>;
        apiResponse?.Message.Should().Be("Grocery items not found");
        apiResponse?.Status.Should().Be("failure");
    }

    #endregion GetFromInvoice

    #region Create

    [Fact]
    public async Task Create_OnSuccess_ReturnStatusCode201()
    {
        // Assert
        var request = new InsertGroceryItemsRequest
        {
            GroceryItems =
            [
                new InsertGroceryItemsDto { Name = "Arroz" },
                new InsertGroceryItemsDto { Name = "Feijão" }
            ]
        };

        var groceryItemService = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();

        groceryItemService
            .InsertAsync(Arg.Any<InsertGroceryItemsRequest>(), Arg.Any<CancellationToken>())
            .Returns(Result<int>.Ok(2));

        var sut = new GroceryItemController(groceryItemService, invoiceService);

        // Act
        var result = await sut.Insert(request);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        var objectResult = result as CreatedResult;
        objectResult?.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task Create_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var request = new InsertGroceryItemsRequest
        {
            GroceryItems =
            [
                new InsertGroceryItemsDto { Name = "Arroz" },
                new InsertGroceryItemsDto { Name = "Feijão" }
            ]
        };
        var groceryItemService = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        groceryItemService
            .InsertAsync(Arg.Any<InsertGroceryItemsRequest>(), Arg.Any<CancellationToken>())
            .Returns(Result<int>.Ok(2));

        var sut = new GroceryItemController(groceryItemService, invoiceService);

        // Act
        await sut.Insert(request);

        // Assert
        await groceryItemService.Received(1)
            .InsertAsync(Arg.Any<InsertGroceryItemsRequest>(), Arg.Any<CancellationToken>());
    }

    #endregion Create

    #region Delete

    [Fact]
    public async Task Delete_OnSuccess_ReturnStatus202()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        service
            .DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Result<bool>.Ok(true));
        var sut = new GroceryItemController(service, invoiceService);

        // Act
        var result = (AcceptedResult)await sut.Delete(Guid.NewGuid());

        // Assert
        result.StatusCode.Should().Be(202);
    }

    [Fact]
    public async Task Delete_OnSuccess_InvokeGroceryItemService()
    {
        // Arrange
        var service = Substitute.For<IGroceryItemService>();
        var invoiceService = Substitute.For<IInvoiceReaderService>();
        service
            .DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Result<bool>.Ok(true));
        var sut = new GroceryItemController(service, invoiceService);

        // Act
        await sut.Delete(Guid.NewGuid());

        // Assert
        await service.Received(1).DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    #endregion Delete
}