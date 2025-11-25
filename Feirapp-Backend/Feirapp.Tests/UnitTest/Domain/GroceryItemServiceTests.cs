using System;
using Feirapp.Domain.Services.GroceryItems.Implementations;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;
using Feirapp.Domain.Services.UnitOfWork;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;
using Feirapp.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Feirapp.Tests.UnitTest.Domain;

public class GroceryItemServiceTests
{
    private readonly IUnitOfWork _uow;

    public GroceryItemServiceTests()
    {
        _uow = Substitute.For<IUnitOfWork>();
    }

    #region GetAllAsync

    [Fact]
    public async Task SearchAsync_WhenCalled_ShouldReturnMappedResults()
    {
        // Arrange
        var request = new SearchGroceryItemsRequest();
        var cancellationToken = CancellationToken.None;
        var groceryItems = new List<SearchGroceryItemsDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Arroz",
                Description = "Arroz branco tipo 1",
                LastPrice = 20.99m,
                ImageUrl = "https://img.com/arroz.png",
                Barcode = "7891234567890",
                LastUpdate = DateTime.Now,
                MeasureUnit = MeasureUnitEnum.KILO,
                StoreId = Guid.NewGuid(),
                StoreName = "Supermercado Central"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Feijão",
                Description = "Feijão carioca",
                LastPrice = 12.50m,
                ImageUrl = "https://img.com/feijao.png",
                Barcode = "7890987654321",
                LastUpdate = DateTime.Now,
                MeasureUnit = MeasureUnitEnum.KILO,
                StoreId = Guid.NewGuid(),
                StoreName = "Supermercado Central"
            }
        };

        _uow.GroceryItemRepository
            .SearchGroceryItemsAsync(request, cancellationToken)
            .Returns(groceryItems);

        var sut = new GroceryItemService(_uow);

        // Act
        var result = await sut.SearchAsync(request, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        await _uow.GroceryItemRepository
            .Received(1)
            .SearchGroceryItemsAsync(Arg.Is<SearchGroceryItemsRequest>(r => r == request), Arg.Is(cancellationToken));
    }

    [Fact]
    public async Task SearchAsync_WhenNoItemsFound_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new SearchGroceryItemsRequest();
        var cancellationToken = CancellationToken.None;

        _uow.GroceryItemRepository
            .SearchGroceryItemsAsync(request, cancellationToken)
            .Returns([]);

        var sut = new GroceryItemService(_uow);

        // Act
        var result = await sut.SearchAsync(request, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        await _uow.GroceryItemRepository
            .Received(1)
            .SearchGroceryItemsAsync(Arg.Any<SearchGroceryItemsRequest>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SearchAsync_WhenRepositoryThrows_ShouldPropagateException()
    {
        // Arrange
        var request = new SearchGroceryItemsRequest();
        var cancellationToken = CancellationToken.None;

        _uow.GroceryItemRepository
            .SearchGroceryItemsAsync(request, cancellationToken)
            .Returns<Task<List<SearchGroceryItemsDto>>>(_ => throw new Exception("Database error"));

        var sut = new GroceryItemService(_uow);

        // Act
        var act = () => sut.SearchAsync(request, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Database error");

        await _uow.GroceryItemRepository
            .Received(1)
            .SearchGroceryItemsAsync(Arg.Any<SearchGroceryItemsRequest>(), Arg.Any<CancellationToken>());
    }

    #endregion GetAllAsync

    #region GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_WhenItemExists_ShouldReturnMappedItem()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        var groceryItem = new GetGroceryItemByIdDto
        {
            Id = Guid.NewGuid(),
            Name = "Arroz",
            Description = "Arroz branco tipo 1",
            ImageUrl = "https://img.com/arroz.png",
            Brand = "Marca Teste",
            Barcode = "7891234567890",
            NcmCode = "10010010",
            CestCode = "1234",
            MeasureUnit = MeasureUnitEnum.KILO,
            PriceHistory =
            [
                new GetGroceryItemByIdPriceLogDto { Price = 20.99m, LogDate = DateTime.Now.AddDays(-1) },
                new GetGroceryItemByIdPriceLogDto { Price = 19.99m, LogDate = DateTime.Now.AddDays(-2) }
            ]
        };

        _uow.GroceryItemRepository
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(groceryItem);

        var sut = new GroceryItemService(_uow);

        // Act
        var result = await sut.GetByIdAsync(id, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        await _uow.GroceryItemRepository
            .Received(1)
            .GetByIdAsync(Arg.Is<Guid>(g => g == id), Arg.Is(cancellationToken));
    }

    [Fact]
    public async Task GetByIdAsync_WhenItemDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        _uow.GroceryItemRepository
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((GetGroceryItemByIdDto?)null);

        var sut = new GroceryItemService(_uow);

        // Act
        var result = await sut.GetByIdAsync(id, cancellationToken);

        // Assert
        result.Should().BeNull();
        await _uow.GroceryItemRepository
            .Received(1)
            .GetByIdAsync(Arg.Is<Guid>(g => g == id), Arg.Is(cancellationToken));
    }

    [Fact]
    public async Task GetByIdAsync_WhenRepositoryThrows_ShouldPropagateException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        _uow.GroceryItemRepository
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns<Task<GetGroceryItemByIdDto?>>(_ => throw new Exception("Database error"));

        var sut = new GroceryItemService(_uow);

        // Act
        var act = () => sut.GetByIdAsync(id, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Database error");

        await _uow.GroceryItemRepository
            .Received(1)
            .GetByIdAsync(Arg.Is<Guid>(g => g == id), Arg.Is(cancellationToken));
    }

    #endregion GetByIdAsync

    #region InsertAsync

    [Fact]
    public async Task InsertAsync_WhenValidRequest_ShouldInsertSuccessfully()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new InsertGroceryItemsRequest
        {
            Store = new InsertGroceryItemsStoreDto()
            {
                Id = Guid.NewGuid(),
                Name = "Supermercado Central",
                Cnpj = "12345678000199",
                Cep = "12345678",
                Street = "Rua Teste",
                StreetNumber = "100",
                Neighborhood = "Centro",
                CityName = "Cidade Teste",
                State = "PE",
                AltNames = new List<string> { "Mercado Central" }
            },
            GroceryItems = new List<InsertGroceryItemsDto>
            {
                new()
                {
                    Name = "Arroz",
                    Barcode = "7891234567890",
                    NcmCode = "10010010",
                    CestCode = "1234",
                    MeasureUnit = "KG",
                    Price = 20.99m
                },
                new()
                {
                    Name = "Feijão",
                    Barcode = "7890987654321",
                    NcmCode = "20020020",
                    CestCode = "5678",
                    MeasureUnit = "KG",
                    Price = 12.50m
                }
            }
        };
        var store = request.Store.ToEntity();
        store.Id = Guid.NewGuid();

        _uow.StoreRepository
            .AddIfNotExistsAsync(Arg.Any<Func<Store, bool>>(), Arg.Any<Store>(), cancellationToken)
            .Returns(store);

        _uow.GroceryItemRepository
            .CheckIfGroceryItemExistsAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<Guid>(),
                cancellationToken)
            .Returns((GroceryItem?)null);

        var sut = new GroceryItemService(_uow);

        // Act
        await sut.InsertAsync(request, cancellationToken);

        // Assert
        await _uow.StoreRepository
            .Received(1)
            .AddIfNotExistsAsync(
                Arg.Any<Func<Store, bool>>(),
                Arg.Is<Store>(s => s.Cnpj == request.Store.Cnpj),
                cancellationToken);

        await _uow.NcmRepository
            .Received(1)
            .InsertListOfCodesAsync(
                Arg.Is<List<string>>(l => l.Contains(request.GroceryItems[0].NcmCode)),
                cancellationToken);

        await _uow.GroceryItemRepository
            .Received(request.GroceryItems.Count)
            .InsertAsync(Arg.Any<GroceryItem>(), cancellationToken);

        await _uow
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ValidateAndRegisterStoreAltNameAsync_WhenStoreExists_ShouldAddAltName()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var existingStore = Fake.Store();

        var newStore = Fake.Store(cnpj: existingStore.Cnpj);
        
        _uow.StoreRepository
            .AddIfNotExistsAsync(Arg.Any<Func<Store, bool>>(), Arg.Any<Store>(), cancellationToken)
            .Returns(existingStore);

        var sut = new GroceryItemService(_uow);

        // Act
        var result = await sut.ValidateAndRegisterStoreAltNameAsync(newStore, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.AltNames.Should().Contain(existingStore.AltNames);
        result.AltNames.Should().Contain(newStore.Name);
    }

    [Fact]
    public async Task InsertNcmsAndCestsAsync_WhenCalledWithValidData_ShouldInsertCodes()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new InsertGroceryItemsRequest
        {
            GroceryItems = new List<InsertGroceryItemsDto>
            {
                new() { NcmCode = "12345678", CestCode = "1234567" },
                new() { NcmCode = "87654321", CestCode = "7654321" }
            }
        };

        var sut = new GroceryItemService(_uow);

        // Act
        await sut.InsertNcmsAndCestsAsync(request, cancellationToken);

        // Assert
        await _uow.NcmRepository
            .Received(1)
            .InsertListOfCodesAsync(
                Arg.Is<List<string>>(l =>
                    l.Contains("12345678") &&
                    l.Contains("87654321")),
                cancellationToken);

        await _uow.CestRepository
            .Received(1)
            .InsertListOfCodesAsync(
                Arg.Is<List<string?>>(l =>
                    l.Contains("1234567") &&
                    l.Contains("7654321")),
                cancellationToken);
    }

    [Theory]
    [InlineData("SEM GTIN", "SEM GTIN")]
    [InlineData("07891234567890", "7891234567890")]
    [InlineData("7891234567890", "7891234567890")]
    public void ValidateBarcode_ShouldHandleVariousFormats(string input, string expected)
    {
        // Act
        var result = GroceryItemService.ValidateBarcode(input);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public async Task InsertOrUpdatePriceLog_WhenNewPriceIsHigher_ShouldInsertNewLog()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var groceryItem = new GroceryItem { Id = Guid.NewGuid(), Barcode = "7891234567890" };
        var storeId = Guid.NewGuid();
        var lastLog = new PriceLog
        {
            Price = 10.00m,
            LogDate = DateTime.Now.AddDays(-1)
        };

        _uow.GroceryItemRepository
            .GetLastPriceLogAsync(groceryItem.Id, storeId, cancellationToken)
            .Returns(lastLog);

        var sut = new GroceryItemService(_uow);

        // Act
        await sut.InsertOrUpdatePriceLog(groceryItem, storeId, 15.00m, DateTime.Now, "123", cancellationToken);

        // Assert
        await _uow.GroceryItemRepository
            .Received(1)
            .InsertPriceLog(
                Arg.Is<PriceLog>(p =>
                    p.GroceryItemId == groceryItem.Id &&
                    p.Price == 15.00m),
                cancellationToken);
    }

    [Fact]
    public async Task InsertOrUpdatePriceLog_WhenPriceIsEqual_ShouldNotInsertNewLog()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var groceryItem = new GroceryItem { Id = Guid.NewGuid(), Barcode = "7891234567890" };
        var storeId = Guid.NewGuid();
        var price = 10.00m;
        var lastLog = new PriceLog
        {
            Price = price,
            LogDate = DateTime.Now.AddDays(-1)
        };

        _uow.GroceryItemRepository
            .GetLastPriceLogAsync(groceryItem.Id, storeId, cancellationToken)
            .Returns(lastLog);

        var sut = new GroceryItemService(_uow);

        // Act
        await sut.InsertOrUpdatePriceLog(groceryItem, storeId, price, DateTime.Now, "123", cancellationToken);

        // Assert
        await _uow.GroceryItemRepository
            .DidNotReceive()
            .InsertPriceLog(Arg.Any<PriceLog>(), cancellationToken);
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_WhenCalled_ShouldDeleteAndSaveChanges()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var groceryId = Guid.NewGuid();

        // Act
        var sut = new GroceryItemService(_uow);
        await sut.DeleteAsync(groceryId, cancellationToken);

        // Assert
        await _uow.GroceryItemRepository.Received(1).DeleteAsync(groceryId, cancellationToken);
        await _uow.Received(1).SaveChangesAsync(cancellationToken);
    }

    [Fact]
    public async Task DeleteAsync_WhenRepositoryThrows_ShouldPropagateException()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var groceryId = Guid.NewGuid();
        _uow.GroceryItemRepository
            .DeleteAsync(groceryId, cancellationToken)
            .Returns(_ => throw new KeyNotFoundException("Delete failed"));

        var sut = new GroceryItemService(_uow);

        // Act & Assert
        var act = async () => await sut.DeleteAsync(groceryId, cancellationToken);
        
        await act.Should().ThrowAsync<KeyNotFoundException>();
        await _uow.GroceryItemRepository.Received(1).DeleteAsync(groceryId, cancellationToken);
    }

    #endregion
}