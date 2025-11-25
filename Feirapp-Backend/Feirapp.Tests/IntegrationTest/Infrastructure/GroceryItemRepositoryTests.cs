using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Feirapp.Infrastructure.Configuration;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Feirapp.Infrastructure.Repository;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;
using Feirapp.Entities.Enums;
using Feirapp.Tests.Fixtures;
using Npgsql;

namespace Feirapp.Tests.IntegrationTest.Infrastructure;

[Collection("integration-tests-collection")]
public class GroceryItemRepositoryTests : IAsyncLifetime
{
    private readonly BaseContext _context;
    private const string ConnectionString = "Host=localhost;Port=5433;Database=feirapp-db;Username=feirapp-user;Password=feirapp-password;Include Error Detail=true;";

    public GroceryItemRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<BaseContext>()
            .UseNpgsql(ConnectionString)
            .Options;
        
        _context = new BaseContext(options);
    }

    public async Task InitializeAsync()
    {
    }

    public async Task DisposeAsync()
    {
        _context.ChangeTracker.Clear();
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM price_logs;");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM grocery_items;");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM stores;");
        await _context.SaveChangesAsync();
    }
    
    #region SearchGroceryItems

    [Fact]
    public async Task SearchGroceryItemsAsync_ReturnsExpectedResults()
    {
        // Arrange
        var store = Fake.Store("Loja Teste");
        var groceryItem = Fake.GroceryItem("Arroz");
        var priceLog = Fake.PriceLog(groceryItem.Id, groceryItem.Barcode,store.Id);

        await _context.Stores.AddAsync(store);
        await _context.GroceryItems.AddAsync(groceryItem);
        await _context.PriceLogs.AddAsync(priceLog);
        await _context.SaveChangesAsync();

        var repository = new GroceryItemRepository(_context);
        var request = new SearchGroceryItemsRequest { Name = "Arroz", PageIndex = 0, PageSize = 10 };

        // Act
        var result = await repository.SearchGroceryItemsAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Arroz");
        result[0].StoreName.Should().Be("Loja Teste");
    }

    [Fact]
    public async Task SearchGroceryItemsAsync_ReturnsLatestPriceLog()
    {
        // Arrange
        var store = Fake.Store();
        var item = Fake.GroceryItem("Arroz");
        await _context.Stores.AddAsync(store);
        await _context.GroceryItems.AddAsync(item);

        var log1 = Fake.PriceLog(item.Id, item.Barcode, store.Id, DateTime.UtcNow.AddDays(-1));
        var log2 = Fake.PriceLog(item.Id, item.Barcode, store.Id, DateTime.UtcNow);

        await _context.PriceLogs.AddRangeAsync(log1, log2);
        await _context.SaveChangesAsync();

        var repository = new GroceryItemRepository(_context);
        
        // Act
        var result = await repository.SearchGroceryItemsAsync(new SearchGroceryItemsRequest { Name = "Arroz", PageIndex = 0, PageSize = 10 }, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result[0].LastPrice.Should().Be(log2.Price);
    }

    [Fact]
    public async Task SearchGroceryItemsAsync_FilterByStore_WorksCorrectly()
    {
        // Arrange
        var store1 = Fake.Store("Loja1");
        var store2 = Fake.Store("Loja2");
        var item1 = Fake.GroceryItem("ProdutoA");
        var item2 = Fake.GroceryItem("ProdutoB");
            
        await _context.Stores.AddRangeAsync(store1, store2);
        await _context.GroceryItems.AddRangeAsync(item1, item2);
        await _context.PriceLogs.AddAsync(Fake.PriceLog(item1.Id, item1.Barcode, store1.Id));
        await _context.PriceLogs.AddAsync(Fake.PriceLog(item2.Id, item2.Barcode, store2.Id));
        await _context.SaveChangesAsync();
        
        var repository = new GroceryItemRepository(_context);
        
        // Act
        var result = await repository.SearchGroceryItemsAsync(new SearchGroceryItemsRequest { StoreId = store1.Id }, CancellationToken.None);
        
        // Assert
        result.Should().ContainSingle(x => x.Name == "ProdutoA");
    }

    [Fact]
    public async Task SearchGroceryItemsAsync_FilterByName_WorksCorrectly()
    {
        // Arrange
        var store = Fake.Store("LojaNome");
        var item1 = Fake.GroceryItem("Arroz Branco");
        var item2 = Fake.GroceryItem("Feijão Preto");

        await _context.Stores.AddAsync(store);
        await _context.GroceryItems.AddRangeAsync(item1, item2);
        await _context.PriceLogs.AddAsync(Fake.PriceLog(item1.Id, item1.Barcode, store.Id));
        await _context.PriceLogs.AddAsync(Fake.PriceLog(item2.Id, item2.Barcode, store.Id));
        await _context.SaveChangesAsync();
        
        var repository = new GroceryItemRepository(_context);
        
        // Act
        var result = await repository.SearchGroceryItemsAsync(new SearchGroceryItemsRequest { Name = "Arroz" }, CancellationToken.None);
        
        // Assert
        result.Should().ContainSingle(x => x.Name == "Arroz Branco");
    }

    [Fact]
    public async Task SearchGroceryItemsAsync_FilterByNameCaseInsensitive_WorksCorrectly()
    {
        // Arrange
        var store = Fake.Store("LojaParcial");
        var item1 = Fake.GroceryItem("Macarrão Integral");
        var item2 = Fake.GroceryItem("Macarrão Tradicional");

        await _context.Stores.AddAsync(store);
        await _context.GroceryItems.AddRangeAsync(item1, item2);
        await _context.PriceLogs.AddAsync(Fake.PriceLog(item1.Id, item1.Barcode, store.Id));
        await _context.PriceLogs.AddAsync(Fake.PriceLog(item2.Id, item2.Barcode, store.Id));
        await _context.SaveChangesAsync();
        
        var repository = new GroceryItemRepository(_context);
        
        // Act
        var result = await repository.SearchGroceryItemsAsync(new SearchGroceryItemsRequest { Name = "macarrão" }, CancellationToken.None);
        
        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task SearchGroceryItemsAsync_FilterByStoreAndName_WorksCorrectly()
    {
        // Arrange
        var store1 = Fake.Store("LojaFiltro");
        var store2 = Fake.Store("LojaOutro");
        var item1 = Fake.GroceryItem("Leite Integral");
        var item2 = Fake.GroceryItem("Leite Desnatado");

        await _context.Stores.AddRangeAsync(store1, store2);
        await _context.GroceryItems.AddRangeAsync(item1, item2);
        await _context.PriceLogs.AddAsync(Fake.PriceLog(item1.Id, item1.Barcode, store1.Id));
        await _context.PriceLogs.AddAsync(Fake.PriceLog(item2.Id, item2.Barcode, store2.Id));
        await _context.SaveChangesAsync();
        
        var repository = new GroceryItemRepository(_context);

        // Act
        var result = await repository.SearchGroceryItemsAsync(new SearchGroceryItemsRequest { StoreId = store1.Id, Name = "Leite" }, CancellationToken.None);
        
        // Assert
        result.Should().ContainSingle(x => x.Name == "Leite Integral");
    }
        
    [Fact]
    public async Task SearchGroceryItemsAsync_Pagination_WorksCorrectly()
    {
        // Arrange
        var store = Fake.Store("Loja Paginação");
        await _context.Stores.AddAsync(store);
        for (var i = 0; i < 15; i++)
        {
            var item = Fake.GroceryItem($"Produto{i}");
            await _context.GroceryItems.AddAsync(item);
            await _context.PriceLogs.AddAsync(Fake.PriceLog(item.Id, item.Barcode, store.Id));
        }
        await _context.SaveChangesAsync();
        var repository = new GroceryItemRepository(_context);
        
        // Act
        var resultPage1 = await repository.SearchGroceryItemsAsync(new SearchGroceryItemsRequest { PageIndex = 0, PageSize = 10 }, CancellationToken.None);
        var resultPage2 = await repository.SearchGroceryItemsAsync(new SearchGroceryItemsRequest { PageIndex = 1, PageSize = 10 }, CancellationToken.None);
        
        // Assert
        resultPage1.Should().HaveCount(10);
        resultPage2.Should().HaveCount(5);
    }
    
    [Fact]
    public async Task SearchGroceryItemsAsync_PageSizeGreaterThanTotal_ReturnsAll()
    {
        // Arrange
        var store = Fake.Store("LojaGrande");
        await _context.Stores.AddAsync(store);
        for (var i = 0; i < 3; i++)
        {
            var item = Fake.GroceryItem($"Produto{i}");
            await _context.GroceryItems.AddAsync(item);
            await _context.PriceLogs.AddAsync(Fake.PriceLog(item.Id, item.Barcode, store.Id));
        }
        await _context.SaveChangesAsync();
        
        var repository = new GroceryItemRepository(_context);

        // Act
        var result = await repository.SearchGroceryItemsAsync(new SearchGroceryItemsRequest { PageIndex = 0, PageSize = 10 }, CancellationToken.None);
        
        // Assert
        result.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task SearchGroceryItemsAsync_NameNotFound_ReturnsEmpty()
    {
        // Arrange
        var store = Fake.Store("LojaVazia");
        var item = Fake.GroceryItem("Café");
        
        await _context.Stores.AddAsync(store);
        await _context.GroceryItems.AddAsync(item);
        await _context.PriceLogs.AddAsync(Fake.PriceLog(item.Id, item.Barcode, store.Id));
        await _context.SaveChangesAsync();

        var repository = new GroceryItemRepository(_context);
        
        // Act
        var result = await repository.SearchGroceryItemsAsync(new SearchGroceryItemsRequest { Name = "Inexistente" }, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchGroceryItemsAsync_StoreNotFound_ReturnsEmpty()
    {
        // Arrange
        var store = Fake.Store("LojaReal");
        var item = Fake.GroceryItem("Açúcar");

        await _context.Stores.AddAsync(store);
        await _context.GroceryItems.AddAsync(item);
        await _context.PriceLogs.AddAsync(Fake.PriceLog(item.Id, item.Barcode, store.Id));
        await _context.SaveChangesAsync();
        
        var repository = new GroceryItemRepository(_context);
        
        // Act
        var result = await repository.SearchGroceryItemsAsync(new SearchGroceryItemsRequest { StoreId = Guid.NewGuid() }, CancellationToken.None);
        
        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchGroceryItemsAsync_NoFilters_ReturnsAll()
    {
        // Arrange
        var store = Fake.Store("LojaTodos");
        
        await _context.Stores.AddAsync(store);
        for (var i = 0; i < 5; i++)
        {
            var item = Fake.GroceryItem($"Produto{i}");
            await _context.GroceryItems.AddAsync(item);
            await _context.PriceLogs.AddAsync(Fake.PriceLog(item.Id, item.Barcode, store.Id));
        }
        
        await _context.SaveChangesAsync();
        
        var repository = new GroceryItemRepository(_context);
        
        // Act
        var result = await repository.SearchGroceryItemsAsync(new SearchGroceryItemsRequest { }, CancellationToken.None);
        
        // Assert
        result.Should().HaveCount(5);
    }
    
    #endregion SearchGroceryItems
    
    #region Insert
    
    [Fact]
    public async Task InsertAsync_ShouldInsertGroceryItem()
    {
        // Arrange
        var item = Fake.GroceryItem("Teste Insert");
        var repository = new GroceryItemRepository(_context);
        
        // Act
        var result = await repository.InsertAsync(item, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        // Assert
        var dbItem = await _context.GroceryItems.FindAsync(item.Id);
        dbItem.Should().NotBeNull();
        result.Should().BeEquivalentTo(item);
    }

    [Fact]
    public async Task InsertAsync_WithNullName_ShouldThrowException()
    {
        // Arrange
        var item = Fake.GroceryItem();
        item.Name = null;
        var repository = new GroceryItemRepository(_context);
        
        // Act
        await repository.InsertAsync(item, CancellationToken.None);
        var act = async () => await _context.SaveChangesAsync();

        // Assert
        var ex = await act.Should().ThrowAsync<DbUpdateException>();
        ex.Which.InnerException.Should().BeOfType<PostgresException>().Which.SqlState.Should().Be("23502");
    }
    
    #endregion Insert
    
    #region Delete

    [Fact]
    public async Task DeleteAsync_RemovesExistingItem()
    {
        // Arrange
        var item = Fake.GroceryItem();
        await _context.GroceryItems.AddAsync(item);
        await _context.SaveChangesAsync();

        var repository = new GroceryItemRepository(_context);
        await repository.DeleteAsync(item.Id, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Act
        var deleted = await _context.GroceryItems.FindAsync(item.Id);
        
        // Assert
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_NonExistingItem_ThrowsException()
    {
        // Arrange
        var repository = new GroceryItemRepository(_context);
        
        // Act
        var act = async () => await repository.DeleteAsync(Guid.NewGuid(), CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_DeletesOnlySpecifiedItem()
    {
        // Arrange
        var item1 = Fake.GroceryItem();
        var item2 = Fake.GroceryItem();
        await _context.GroceryItems.AddRangeAsync(item1, item2);
        await _context.SaveChangesAsync();

        var repository = new GroceryItemRepository(_context);
        
        // Act
        await repository.DeleteAsync(item1.Id, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        // Assert
        (await _context.GroceryItems.FindAsync(item1.Id)).Should().BeNull();
        (await _context.GroceryItems.FindAsync(item2.Id)).Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_WithEmptyGuid_ThrowsException()
    {
        // Arrange
        var repository = new GroceryItemRepository(_context);
        
        // Act
        var act = async () => await repository.DeleteAsync(Guid.Empty, CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_DeletingAlreadyDeletedItem_ThrowsException()
    {
        // Arrange
        var item = Fake.GroceryItem();
        await _context.GroceryItems.AddAsync(item);
        await _context.SaveChangesAsync();

        // Act
        var repository = new GroceryItemRepository(_context);
        await repository.DeleteAsync(item.Id, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        var act = async () => await repository.DeleteAsync(item.Id, CancellationToken.None);
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    #endregion Delete

    #region GetById

    [Fact]
    public async Task GetByIdAsync_ReturnsItem_WhenExists()
    {
        // Arrange
        var item = Fake.GroceryItem("Produto Teste");
        await _context.GroceryItems.AddAsync(item);
        await _context.SaveChangesAsync();
        var repository = new GroceryItemRepository(_context);

        // Act
        var result = await repository.GetByIdAsync(item.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(item.Id);
        result.Name.Should().Be(item.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        var repository = new GroceryItemRepository(_context);
        
        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);
        
        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsPriceHistory_WhenExists()
    {
        // Arrange
        var store = Fake.Store("Loja Preço");
        await _context.Stores.AddAsync(store);
        var item = Fake.GroceryItem("Produto Histórico");
        await _context.GroceryItems.AddAsync(item);
        var price1 = Fake.PriceLog(item.Id, item.Barcode, store.Id, DateTime.UtcNow.AddDays(-2));
        var price2 = Fake.PriceLog(item.Id, item.Barcode, store.Id, DateTime.UtcNow.AddDays(-1));
        await _context.PriceLogs.AddRangeAsync(price1, price2);
        await _context.SaveChangesAsync();
        var repository = new GroceryItemRepository(_context);

        // Act
        var result = await repository.GetByIdAsync(item.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.PriceHistory.Should().HaveCount(2);
        result.PriceHistory[0].Price.Should().Be(price2.Price);
        result.PriceHistory[1].Price.Should().Be(price1.Price);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEmptyPriceHistory_WhenNoPrices()
    {
        // Arrange
        var item = Fake.GroceryItem("Produto Sem Preço");
        await _context.GroceryItems.AddAsync(item);
        await _context.SaveChangesAsync();
        var repository = new GroceryItemRepository(_context);

        // Act
        var result = await repository.GetByIdAsync(item.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.PriceHistory.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsStoreDataInPriceHistory()
    {
        // Arrange
        var store = Fake.Store("Loja Dados");
        await _context.Stores.AddAsync(store);
        var item = Fake.GroceryItem("Produto Loja");
        await _context.GroceryItems.AddAsync(item);
        var price = Fake.PriceLog(item.Id, item.Barcode, store.Id, DateTime.UtcNow);
        await _context.PriceLogs.AddAsync(price);
        await _context.SaveChangesAsync();
        var repository = new GroceryItemRepository(_context);

        // Act
        var result = await repository.GetByIdAsync(item.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.PriceHistory.Should().ContainSingle();
        var storeDto = result.PriceHistory[0].Store;
        storeDto.Should().NotBeNull();
        storeDto.Name.Should().Be(store.Name);
        storeDto.Cep.Should().Be(store.Cep);
        storeDto.CityName.Should().Be(store.CityName);
        storeDto.Cnpj.Should().Be(store.Cnpj);
        storeDto.Neighborhood.Should().Be(store.Neighborhood);
        storeDto.State.Should().Be(store.State.StringValue());
        storeDto.Street.Should().Be(store.Street);
        storeDto.StreetNumber.Should().Be(store.StreetNumber);
        storeDto.AltNames.Should().BeEquivalentTo(store.AltNames);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsAllFieldsCorrectly()
    {
        // Arrange
        var item = Fake.GroceryItem("Produto Completo");
        item.Description = "Descrição Teste";
        item.Barcode = "123456789";
        item.Brand = "Marca Teste";
        item.CestCode = "CEST123";
        item.ImageUrl = "http://img.com/1.png";
        item.MeasureUnit = MeasureUnitEnum.KILO;
        item.NcmCode = "NCM123";
        await _context.GroceryItems.AddAsync(item);
        await _context.SaveChangesAsync();
        var repository = new GroceryItemRepository(_context);

        // Act
        var result = await repository.GetByIdAsync(item.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(item.Id);
        result.Name.Should().Be(item.Name);
        result.Description.Should().Be(item.Description);
        result.Barcode.Should().Be(item.Barcode);
        result.Brand.Should().Be(item.Brand);
        result.CestCode.Should().Be(item.CestCode);
        result.ImageUrl.Should().Be(item.ImageUrl);
        result.MeasureUnit.Should().Be(item.MeasureUnit);
        result.NcmCode.Should().Be(item.NcmCode);
    }

    #endregion GetByIdAsync

    #region CheckIfGroceryItemExists

    [Fact]
    public async Task CheckIfGroceryItemExistsAsync_ReturnsItem_WhenBarcodeExists()
    {
        // Arrange
        var item = Fake.GroceryItem("Produto Barcode");
        item.Barcode = "123456789";
        await _context.GroceryItems.AddAsync(item);
        var store = Fake.Store();
        await _context.Stores.AddAsync(store);
        var price = Fake.PriceLog(item.Id, item.Barcode, store.Id);
        await _context.PriceLogs.AddAsync(price);
        await _context.SaveChangesAsync();
        var repository = new GroceryItemRepository(_context);

        // Act
        var result = await repository.CheckIfGroceryItemExistsAsync("123456789", "", store.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(item.Id);
    }

    [Fact]
    public async Task CheckIfGroceryItemExistsAsync_ReturnsNull_WhenBarcodeNotExists()
    {
        // Arrange
        var item = Fake.GroceryItem("Produto Barcode Ausente");
        item.Barcode = "987654321";
        await _context.GroceryItems.AddAsync(item);
        var store = Fake.Store();
        await _context.Stores.AddAsync(store);
        var price = Fake.PriceLog(item.Id, item.Barcode, store.Id);
        await _context.PriceLogs.AddAsync(price);
        await _context.SaveChangesAsync();
        var repository = new GroceryItemRepository(_context);

        // Act
        var result = await repository.CheckIfGroceryItemExistsAsync("inexistente", "", store.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CheckIfGroceryItemExistsAsync_ReturnsItem_WhenSemGtinAndProductCodeAndStoreMatch()
    {
        // Arrange
        var item = Fake.GroceryItem("Produto SEM GTIN");
        item.Barcode = "SEM GTIN";
        var store = Fake.Store();
        await _context.Stores.AddAsync(store);
        await _context.GroceryItems.AddAsync(item);
        var price = Fake.PriceLog(item.Id, item.Barcode, store.Id);
        price.ProductCode = "PROD123";
        await _context.PriceLogs.AddAsync(price);
        await _context.SaveChangesAsync();
        var repository = new GroceryItemRepository(_context);

        // Act
        var result = await repository.CheckIfGroceryItemExistsAsync("SEM GTIN", "PROD123", store.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(item.Id);
    }

    [Fact]
    public async Task CheckIfGroceryItemExistsAsync_ReturnsNull_WhenSemGtinAndProductCodeOrStoreNotMatch()
    {
        // Arrange
        var item = Fake.GroceryItem("Produto SEM GTIN");
        item.Barcode = "SEM GTIN";
        var store = Fake.Store();
        await _context.Stores.AddAsync(store);
        await _context.GroceryItems.AddAsync(item);
        var price = Fake.PriceLog(item.Id, item.Barcode, store.Id);
        price.ProductCode = "PROD123";
        await _context.PriceLogs.AddAsync(price);
        await _context.SaveChangesAsync();
        var repository = new GroceryItemRepository(_context);

        // Act
        var result1 = await repository.CheckIfGroceryItemExistsAsync("SEM GTIN", "OUTRO", store.Id, CancellationToken.None);
        var result2 = await repository.CheckIfGroceryItemExistsAsync("SEM GTIN", "PROD123", Guid.NewGuid(), CancellationToken.None);

        // Assert
        result1.Should().BeNull();
        result2.Should().BeNull();
    }

    [Fact]
    public async Task CheckIfGroceryItemExistsAsync_ReturnsNull_WhenNoPriceLog()
    {
        // Arrange
        var item = Fake.GroceryItem("Produto Sem Preço");
        item.Barcode = "123456789";
        await _context.GroceryItems.AddAsync(item);
        await _context.SaveChangesAsync();
        var repository = new GroceryItemRepository(_context);
        var store = Fake.Store();
        await _context.Stores.AddAsync(store);

        // Act
        var result = await repository.CheckIfGroceryItemExistsAsync("123456789", "", store.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CheckIfGroceryItemExistsAsync_ReturnsCorrectItem_WhenMultipleItemsExist()
    {
        // Arrange
        var item1 = Fake.GroceryItem("Produto 1");
        item1.Barcode = "111";
        var item2 = Fake.GroceryItem("Produto 2");
        item2.Barcode = "222";
        var store = Fake.Store();
        await _context.Stores.AddAsync(store);
        await _context.GroceryItems.AddRangeAsync(item1, item2);
        var price1 = Fake.PriceLog(item1.Id, item1.Barcode, store.Id);
        var price2 = Fake.PriceLog(item2.Id, item2.Barcode, store.Id);
        await _context.PriceLogs.AddRangeAsync(price1, price2);
        await _context.SaveChangesAsync();
        var repository = new GroceryItemRepository(_context);

        // Act
        var result = await repository.CheckIfGroceryItemExistsAsync("222", "", store.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(item2.Id);
    }

    #endregion CheckIfGroceryItemExists

    #region InsertPriceLog

    [Fact]
    public async Task InsertPriceLog_ShouldInsertSuccessfully()
    {
        // Arrange
        var item = Fake.GroceryItem();
        var store = Fake.Store();
        await _context.GroceryItems.AddAsync(item);
        await _context.Stores.AddAsync(store);
        await _context.SaveChangesAsync();
        var repository = new GroceryItemRepository(_context);
        var priceLog = Fake.PriceLog(item.Id, item.Barcode, store.Id, DateTime.UtcNow);

        // Act
        await repository.InsertPriceLog(priceLog, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        var dbLog = await _context.PriceLogs.FirstOrDefaultAsync(x => x.GroceryItemId == item.Id && x.StoreId == store.Id);
        dbLog.Should().NotBeNull();
        dbLog!.Price.Should().Be(priceLog.Price);
    }

    [Fact]
    public async Task InsertPriceLog_ShouldInsertMultipleLogsForSameItem()
    {
        // Arrange
        var item = Fake.GroceryItem();
        var store = Fake.Store();
        await _context.GroceryItems.AddAsync(item);
        await _context.Stores.AddAsync(store);
        await _context.SaveChangesAsync();
        var repository = new GroceryItemRepository(_context);
        var log1 = Fake.PriceLog(item.Id, item.Barcode, store.Id, DateTime.UtcNow.AddDays(-1));
        var log2 = Fake.PriceLog(item.Id, item.Barcode, store.Id, DateTime.UtcNow);

        // Act
        await repository.InsertPriceLog(log1, CancellationToken.None);
        await repository.InsertPriceLog(log2, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        var logs = await _context.PriceLogs.Where(x => x.GroceryItemId == item.Id).ToListAsync();
        logs.Should().HaveCount(2);
        logs.Should().Contain(x => x.Price == log1.Price);
        logs.Should().Contain(x => x.Price == log2.Price);
    }

    [Fact]
    public async Task InsertPriceLog_ShouldFail_WhenRequiredFieldsAreNull()
    {
        // Arrange
        var item = Fake.GroceryItem();
        var store = Fake.Store();
        await _context.GroceryItems.AddAsync(item);
        await _context.Stores.AddAsync(store);
        await _context.SaveChangesAsync();
        var repository = new GroceryItemRepository(_context);
        var priceLog = Fake.PriceLog(item.Id, item.Barcode, store.Id);
        priceLog.Barcode = null!;

        // Act
        await repository.InsertPriceLog(priceLog, CancellationToken.None);
        var act = async () => await _context.SaveChangesAsync();
        
        // Assert
        await act.Should().ThrowAsync<DbUpdateException>();
    }

    #endregion InsertPriceLog

    #region GetLastPriceLogAsync

    [Fact]
    public async Task GetLastPriceLogAsync_ReturnsMostRecentLog()
    {
        // Arrange
        var item = Fake.GroceryItem();
        var store = Fake.Store();
        await _context.GroceryItems.AddAsync(item);
        await _context.Stores.AddAsync(store);
        await _context.SaveChangesAsync();
        var repo = new GroceryItemRepository(_context);
        var log1 = Fake.PriceLog(item.Id, item.Barcode, store.Id, DateTime.UtcNow.AddDays(-2));
        var log2 = Fake.PriceLog(item.Id, item.Barcode, store.Id, DateTime.UtcNow.AddDays(-1));
        var log3 = Fake.PriceLog(item.Id, item.Barcode, store.Id, DateTime.UtcNow);
        await _context.PriceLogs.AddRangeAsync(log1, log2, log3);
        await _context.SaveChangesAsync();

        // Act
        var result = await repo.GetLastPriceLogAsync(item.Id, store.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Price.Should().Be(log3.Price);
        result.LogDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task GetLastPriceLogAsync_ReturnsNull_WhenNoLogs()
    {
        // Arrange
        var item = Fake.GroceryItem();
        var store = Fake.Store();
        await _context.GroceryItems.AddAsync(item);
        await _context.Stores.AddAsync(store);
        await _context.SaveChangesAsync();
        var repo = new GroceryItemRepository(_context);

        // Act
        var result = await repo.GetLastPriceLogAsync(item.Id, store.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetLastPriceLogAsync_ReturnsCorrectLog_WhenMultipleItemsAndStores()
    {
        // Arrange
        var item1 = Fake.GroceryItem();
        var item2 = Fake.GroceryItem();
        var store1 = Fake.Store();
        var store2 = Fake.Store();
        await _context.GroceryItems.AddRangeAsync(item1, item2);
        await _context.Stores.AddRangeAsync(store1, store2);
        await _context.SaveChangesAsync();
        var repo = new GroceryItemRepository(_context);
        var log1 = Fake.PriceLog(item1.Id, item1.Barcode, store1.Id, DateTime.UtcNow);
        var log2 = Fake.PriceLog(item2.Id, item2.Barcode, store2.Id, DateTime.UtcNow);
        await _context.PriceLogs.AddRangeAsync(log1, log2);
        await _context.SaveChangesAsync();

        // Act
        var result = await repo.GetLastPriceLogAsync(item2.Id, store2.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Price.Should().Be(log2.Price);
        result.GroceryItemId.Should().Be(item2.Id);
        result.StoreId.Should().Be(store2.Id);
    }

    [Fact]
    public async Task GetLastPriceLogAsync_ReturnsLog_WhenOnlyOneExists()
    {
        // Arrange
        var item = Fake.GroceryItem();
        var store = Fake.Store();
        await _context.GroceryItems.AddAsync(item);
        await _context.Stores.AddAsync(store);
        await _context.SaveChangesAsync();
        var repo = new GroceryItemRepository(_context);
        var log = Fake.PriceLog(item.Id, item.Barcode, store.Id, DateTime.UtcNow);
        await _context.PriceLogs.AddAsync(log);
        await _context.SaveChangesAsync();

        // Act
        var result = await repo.GetLastPriceLogAsync(item.Id, store.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Price.Should().Be(log.Price);
    }

    [Fact]
    public async Task GetLastPriceLogAsync_DoesNotReturnLog_FromOtherStoreOrItem()
    {
        // Arrange
        var item1 = Fake.GroceryItem();
        var item2 = Fake.GroceryItem();
        var store1 = Fake.Store();
        var store2 = Fake.Store();
        await _context.GroceryItems.AddRangeAsync(item1, item2);
        await _context.Stores.AddRangeAsync(store1, store2);
        await _context.SaveChangesAsync();
        var repo = new GroceryItemRepository(_context);
        var log1 = Fake.PriceLog(item1.Id, item1.Barcode, store1.Id, DateTime.UtcNow);
        var log2 = Fake.PriceLog(item2.Id, item2.Barcode, store2.Id, DateTime.UtcNow);
        await _context.PriceLogs.AddRangeAsync(log1, log2);
        await _context.SaveChangesAsync();

        // Act
        var result = await repo.GetLastPriceLogAsync(item1.Id, store2.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetByStoreAsync

    [Fact]
    public async Task GetByStoreAsync_ReturnsStoreWithItems_WhenItemsExist()
    {
        // Arrange
        var store = Fake.Store("Loja Teste");
        await _context.Stores.AddAsync(store);
        var item1 = Fake.GroceryItem("Produto 1");
        var item2 = Fake.GroceryItem("Produto 2");
        await _context.GroceryItems.AddRangeAsync(item1, item2);
        await _context.PriceLogs.AddAsync(Fake.PriceLog(item1.Id, item1.Barcode, store.Id));
        await _context.PriceLogs.AddAsync(Fake.PriceLog(item2.Id, item2.Barcode, store.Id));
        await _context.SaveChangesAsync();
        var repo = new GroceryItemRepository(_context);

        // Act
        var result = await repo.GetByStoreAsync(store.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Store.Should().NotBeNull();
        result.Store.Id.Should().Be(store.Id);
        result.Items.Should().HaveCount(2);
        result.Items.Select(i => i.Id).Should().Contain(new[] { item1.Id, item2.Id });
    }

    [Fact]
    public async Task GetByStoreAsync_ReturnsStoreWithEmptyItems_WhenNoItems()
    {
        // Arrange
        var store = Fake.Store("Loja Sem Itens");
        await _context.Stores.AddAsync(store);
        await _context.SaveChangesAsync();
        var repo = new GroceryItemRepository(_context);

        // Act
        var result = await repo.GetByStoreAsync(store.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Store.Should().NotBeNull();
        result.Store.Id.Should().Be(store.Id);
        result.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByStoreAsync_ThrowsException_WhenStoreNotFound()
    {
        // Arrange
        var repo = new GroceryItemRepository(_context);

        // Act
        Func<Task> act = async () => await repo.GetByStoreAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task GetByStoreAsync_ReturnsOnlyItemsWithPriceLogForStore()
    {
        // Arrange
        var store1 = Fake.Store("Loja 1");
        var store2 = Fake.Store("Loja 2");
        await _context.Stores.AddRangeAsync(store1, store2);
        var item1 = Fake.GroceryItem("Produto A");
        var item2 = Fake.GroceryItem("Produto B");
        await _context.GroceryItems.AddRangeAsync(item1, item2);
        await _context.PriceLogs.AddAsync(Fake.PriceLog(item1.Id, item1.Barcode, store1.Id));
        await _context.PriceLogs.AddAsync(Fake.PriceLog(item2.Id, item2.Barcode, store2.Id));
        await _context.SaveChangesAsync();
        var repo = new GroceryItemRepository(_context);

        // Act
        var result = await repo.GetByStoreAsync(store1.Id, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items[0].Id.Should().Be(item1.Id);
    }

    [Fact]
    public async Task GetByStoreAsync_ReturnsAllStoreFieldsCorrectly()
    {
        // Arrange
        var store = Fake.Store("Loja Completa");
        store.Cep = "12345678";
        store.CityName = "Cidade Teste";
        store.Cnpj = "12.345.678/0001-99";
        store.Neighborhood = "Bairro";
        store.State = StatesEnum.SP;
        store.Street = "Rua Teste";
        store.StreetNumber = "100";
        store.AltNames = ["Alternativo"];
        await _context.Stores.AddAsync(store);
        await _context.SaveChangesAsync();
        var repo = new GroceryItemRepository(_context);

        // Act
        var result = await repo.GetByStoreAsync(store.Id, CancellationToken.None);

        // Assert
        result.Store.Should().NotBeNull();
        result.Store.Name.Should().Be(store.Name);
        result.Store.Cep.Should().Be(store.Cep);
        result.Store.CityName.Should().Be(store.CityName);
        result.Store.Cnpj.Should().Be(store.Cnpj);
        result.Store.Neighborhood.Should().Be(store.Neighborhood);
        result.Store.State.Should().Be(store.State);
        result.Store.Street.Should().Be(store.Street);
        result.Store.StreetNumber.Should().Be(store.StreetNumber);
        result.Store.AltNames.Should().BeEquivalentTo(store.AltNames);
    }

    #endregion
}
