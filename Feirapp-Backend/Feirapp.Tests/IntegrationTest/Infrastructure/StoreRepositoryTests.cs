using System;
using System.Threading;
using System.Threading.Tasks;
using Feirapp.Domain.Services.Stores.Methods.SearchStores;
using Feirapp.Entities.Enums;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Repository;
using Feirapp.Tests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Xunit;

namespace Feirapp.Tests.IntegrationTest.Infrastructure;

[Collection("integration-tests-collection")]
public class StoreRepositoryTests : IAsyncLifetime
{
    private readonly BaseContext _context;
    private const string ConnectionString = "Host=localhost;Port=5433;Database=feirapp-test-db;Username=feirapp-test-user;Password=feirapp-test-password;Include Error Detail=true;";

    public StoreRepositoryTests()
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
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM stores;");
        await _context.SaveChangesAsync();
    }
    
    #region AddIfNotExists
    
    [Fact]
    public async Task AddIfNotExistsAsync_ShouldAddStore_WhenNotExists()
    {
        // Arrange
        var repo = new StoreRepository(_context);
        var store = Fake.Store();
        var ct = CancellationToken.None;

        // Act
        var result = await repo.AddIfNotExistsAsync(s => s.Cnpj == store.Cnpj, store, ct);
        await _context.SaveChangesAsync(ct);
        
        // Assert
        result.Should().NotBeNull();
        result.Cnpj.Should().BeEquivalentTo(store.Cnpj);
        (await _context.Stores.CountAsync(ct)).Should().Be(1);
    }

    [Fact]
    public async Task AddIfNotExistsAsync_ShouldNotAddStore_WhenExists()
    {
        // Arrange
        var repo = new StoreRepository(_context);
        var store = Fake.Store();
        var ct = CancellationToken.None;
        await _context.Stores.AddAsync(store, ct);
        await _context.SaveChangesAsync(ct);
        
        var duplicate = Fake.Store(cnpj: store.Cnpj);

        // Act
        var result = await repo.AddIfNotExistsAsync(s => s.Cnpj == duplicate.Cnpj, duplicate, ct);
        await _context.SaveChangesAsync(ct);

        // Assert
        result.Should().NotBeNull();
        result.Cnpj.Should().Be(store.Cnpj);
        (await _context.Stores.CountAsync(ct)).Should().Be(1);
    }
    
    #endregion AddIfNotExistsAsync
    
    #region Insert

    [Fact]
    public async Task InsertAsync_ShouldAddStore_WhenNotExists()
    {
        // Arrange
        var store = Fake.Store();
        var repo = new StoreRepository(_context);

        // Act
        var result = await repo.InsertAsync(store, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        var dbItem = await _context.Stores.FindAsync(result.Id);
        dbItem.Should().NotBeNull();
        result.Should().BeEquivalentTo(dbItem);
    }

    [Fact]
    public async Task InsertAsync_ShouldNotInsert_AndReturnTheStoreOnDB_WhenStoreWithSameCnpjExists()
    {
        // Arrange
        var store = Fake.Store(cnpj: "11122233344455");
        var repo = new StoreRepository(_context);

        // Act
        await repo.InsertAsync(store, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        var duplicate = Fake.Store(cnpj: store.Cnpj);
        var result = await repo.InsertAsync(duplicate, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Cnpj.Should().Be(store.Cnpj);
        (await _context.Stores.CountAsync()).Should().Be(1);
    }
    
    [Fact]
    public async Task InsertAsync_ShouldNotInsert_WhenStoreWithSameCnpjExists()
    {
        // Arrange
        var store = Fake.Store();
        store.Name = null;
        var repo = new StoreRepository(_context);

        // Act
        await repo.InsertAsync(store, CancellationToken.None);
        var act = async () => await _context.SaveChangesAsync();
        
        // Assert
        var exception = await act.Should().ThrowAsync<DbUpdateException>();
        exception.Which.InnerException.Should().BeOfType<PostgresException>().Which.SqlState.Should().Be("23502");
    }

    #endregion Insert
    
    #region GetById

    [Fact]
    public async Task GetByIdAsync_ShouldReturnStore_WhenExists()
    {
        // Arrange
        var repo = new StoreRepository(_context);
        var store = Fake.Store();
        var ct = CancellationToken.None;
        await _context.Stores.AddAsync(store, ct);
        await _context.SaveChangesAsync(ct);

        // Act
        var result = await repo.GetByIdAsync(store.Id, ct);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(store.Id);
        result.Name.Should().Be(store.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var repo = new StoreRepository(_context);
        var ct = CancellationToken.None;
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repo.GetByIdAsync(nonExistentId, ct);

        // Assert
        result.Should().BeNull();
    }

    #endregion GetById
    
    #region SearchStores

    [Fact]
    public async Task SearchStoresAsync_ShouldReturnStores_WhenMatchingExists()
    {
        // Arrange
        var repo = new StoreRepository(_context);
        var store1 = Fake.Store(name: "Mercado Central");
        var store2 = Fake.Store(name: "Mercado Bairro");
        var store3 = Fake.Store(name: "Padaria Nova");
        await _context.Stores.AddRangeAsync(store1, store2, store3);
        await _context.SaveChangesAsync();
        
        var request = new SearchStoresRequest { Name = "Mercado", PageIndex = 0, PageSize = 10 };

        // Act
        var result = await repo.SearchStoresAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(s => s.Name.Contains("Mercado"));
    }
    
    [Fact]
    public async Task SearchStoresAsync_ShouldReturnStores_WhenMatchingNameExistsCaseInsensitive()
    {
        // Arrange
        var repo = new StoreRepository(_context);
        var store1 = Fake.Store(name: "Mercado Central");
        var store2 = Fake.Store(name: "Mercado Bairro");
        var store3 = Fake.Store(name: "Padaria Nova");
        await _context.Stores.AddRangeAsync(store1, store2, store3);
        await _context.SaveChangesAsync();
        
        var request = new SearchStoresRequest { Name = "mercado", PageIndex = 0, PageSize = 10 };

        // Act
        var result = await repo.SearchStoresAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(s => s.Name.Contains("Mercado"));
    }
    
    [Fact]
    public async Task SearchStoresAsync_ShouldReturnStores_WhenMatchingCityExists()
    {
        // Arrange
        var repo = new StoreRepository(_context);
        var store1 = Fake.Store(name: "Mercado Central", city: "Recife");
        var store2 = Fake.Store(name: "Mercado Bairro", city: "Recife");
        var store3 = Fake.Store(name: "Padaria Nova", city: "Rio de Janeiro");
        await _context.Stores.AddRangeAsync(store1, store2, store3);
        await _context.SaveChangesAsync();
        
        var request = new SearchStoresRequest { CityName = "Recife", PageIndex = 0, PageSize = 10 };

        // Act
        var result = await repo.SearchStoresAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(s => s.CityName != null && s.CityName.Contains("Recife"));
    }
    
    [Fact]
    public async Task SearchStoresAsync_ShouldReturnStores_WhenMatchingStateExists()
    {
        // Arrange
        var repo = new StoreRepository(_context);
        var store1 = Fake.Store(name: "Mercado Central", state: StatesEnum.CE);
        var store2 = Fake.Store(name: "Mercado Bairro", state: StatesEnum.SP);
        var store3 = Fake.Store(name: "Padaria Nova", state: StatesEnum.SP);
        await _context.Stores.AddRangeAsync(store1, store2, store3);
        await _context.SaveChangesAsync();
        
        var request = new SearchStoresRequest { State = StatesEnum.CE, PageIndex = 0, PageSize = 10 };

        // Act
        var result = await repo.SearchStoresAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Should().OnlyContain(s => s.State == StatesEnum.CE);
    }

    [Fact]
    public async Task SearchStoresAsync_ShouldReturnEmpty_WhenNoMatch()
    {
        // Arrange
        var repo = new StoreRepository(_context);
        var ct = CancellationToken.None;
        var store1 = Fake.Store(name: "Mercado Central");
        await _context.Stores.AddAsync(store1, ct);
        await _context.SaveChangesAsync(ct);
        var request = new SearchStoresRequest
        {
            Name = "Padaria",
            PageIndex = 0,
            PageSize = 10
        };

        // Act
        var result = await repo.SearchStoresAsync(request, ct);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchStoresAsync_ShouldReturnPaginatedResults()
    {
        // Arrange
        var repo = new StoreRepository(_context);
        for (var i = 0; i < 15; i++)
        {
            await _context.Stores.AddAsync(Fake.Store(name: $"Loja {i}"));
        }
        await _context.SaveChangesAsync();
        var request = new SearchStoresRequest
        {
            PageIndex = 0,
            PageSize = 10
        };
        // Act
        var result1 = await repo.SearchStoresAsync(request, CancellationToken.None);
        var result2 = await repo.SearchStoresAsync(request with { PageIndex = 1 }, CancellationToken.None);

        // Assert
        result1.Should().NotBeNull();
        result1.Should().HaveCount(10); 
        result2.Should().NotBeNull();
        result2.Should().HaveCount(5);
    }
    
    [Fact]
    public async Task SearchStoresAsync_PageSizeGreaterThanTotal_ReturnsAll()
    {
        // Arrange
        var repo = new StoreRepository(_context);
        for (var i = 0; i < 10; i++)
        {
            await _context.Stores.AddAsync(Fake.Store(name: $"Loja {i}"));
        }
        await _context.SaveChangesAsync();
        var request = new SearchStoresRequest
        {
            PageIndex = 0,
            PageSize = 15
        };

        // Act
        var result = await repo.SearchStoresAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(10);
    }

    #endregion SearchStores
}