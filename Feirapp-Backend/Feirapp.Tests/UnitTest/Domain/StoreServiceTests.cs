using System;
using System.Threading;
using System.Threading.Tasks;
using Feirapp.Domain.Services.Stores.Implementations;
using Feirapp.Domain.Services.Stores.Methods.InsertGroceryItem;
using Feirapp.Domain.Services.Stores.Methods.SearchStores;
using Feirapp.Domain.Services.UnitOfWork;
using Feirapp.Entities.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Feirapp.Tests.UnitTest.Domain;

public class StoreServiceTests
{
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();

    #region InsertStore

    [Fact]
    public async Task InsertStoreAsync_ShouldCallRepositoryAndSaveChanges()
    {
        // Arrange
        var request = new InsertStoreRequest();
        var ct = CancellationToken.None;
        var storeRepository = _uow.StoreRepository;
        
        var service = new StoreService(_uow);

        // Act
        var result = await service.InsertStoreAsync(request, ct);

        // Assert
        await storeRepository.Received(1).InsertAsync(Arg.Any<Store>(), Arg.Any<CancellationToken>());
        await _uow.Received(1).SaveChangesAsync(ct);
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
    }
    
    #endregion InsertStore

    #region SearchStoresAsync

    [Fact]
    public async Task SearchStoresAsync_ShouldReturnConvertedList_WhenRepositoryReturnsStores()
    {
        // Arrange
        var storeRepository = _uow.StoreRepository;
        var service = new StoreService(_uow);
        var store1 = new Store { Name = "store 1" };
        var store2 = new Store { Name = "store 2" };
        
        storeRepository
            .SearchStoresAsync(Arg.Any<SearchStoresRequest>(), Arg.Any<CancellationToken>())
            .Returns([store1, store2]);

        // Act
        var result = await service.SearchStoresAsync(new SearchStoresRequest(), CancellationToken.None);

        // Assert
        await storeRepository.Received(1).SearchStoresAsync(Arg.Any<SearchStoresRequest>(), Arg.Any<CancellationToken>());
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Count.Should().Be(2);
        result.Value![0].Name.Should().Be(store1.Name);
        result.Value![1].Name.Should().Be(store2.Name);
    }

    [Fact]
    public async Task SearchStoresAsync_ShouldReturnEmptyList_WhenRepositoryReturnsEmpty()
    {
        // Arrange
        var storeRepository = _uow.StoreRepository;
        var service = new StoreService(_uow);
        storeRepository
            .SearchStoresAsync(Arg.Any<SearchStoresRequest>(), Arg.Any<CancellationToken>())
            .Returns([]);
        
        // Act
        var result = await service.SearchStoresAsync(new SearchStoresRequest(), CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchStoresAsync_WhenRepositoryThrows_ShouldPropagate()
    {
        // Arrange
        var storeRepository = _uow.StoreRepository;
        var service = new StoreService(_uow);
        storeRepository
            .SearchStoresAsync(Arg.Any<SearchStoresRequest>(), Arg.Any<CancellationToken>())
            .Throws(new Exception("fail"));

        // Act
        var act = async () => await  service.SearchStoresAsync(new SearchStoresRequest(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("fail");
    }

    #endregion SearchStoresAsync
    
    #region GetStoreById

    [Fact]
    public async Task GetStoreByIdAsync_ShouldReturnResponse_WhenStoreExists()
    {
        // Arrange
        var storeRepository = _uow.StoreRepository;
        var service = new StoreService(_uow);
        var storeId = Guid.NewGuid();
        var store = new Store { Id = storeId, Name = "Test Store" };
        storeRepository.GetByIdAsync(storeId, Arg.Any<CancellationToken>()).Returns(store);

        // Act
        var result = await service.GetStoreByIdAsync(storeId, CancellationToken.None);

        // Assert
        await storeRepository.Received(1).GetByIdAsync(storeId, Arg.Any<CancellationToken>());
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Name.Should().Be(store.Name);
    }

    [Fact]
    public async Task GetStoreByIdAsync_ShouldReturnFail_WhenStoreDoesNotExist()
    {
        // Arrange
        var storeRepository = _uow.StoreRepository;
        var service = new StoreService(_uow);
        var storeId = Guid.NewGuid();
        storeRepository.GetByIdAsync(storeId, Arg.Any<CancellationToken>()).Returns((Store?)null);

        // Act
        var result = await service.GetStoreByIdAsync(storeId, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetStoreByIdAsync_WhenRepositoryThrows_ShouldPropagate()
    {
        // Arrange
        var storeRepository = _uow.StoreRepository;
        var service = new StoreService(_uow);
        var storeId = Guid.NewGuid();
        storeRepository.GetByIdAsync(storeId, Arg.Any<CancellationToken>()).Throws(new Exception("fail"));

        // Act
        var act = async () => await service.GetStoreByIdAsync(storeId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("fail");
    }

    #endregion GetStoreById
}