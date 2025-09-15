using Feirapp.Domain.Services.Stores.Methods.SearchStores;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.Stores.Interfaces;

public interface IStoreRepository
{
    Task<Store> AddIfNotExistsAsync(Func<Store, bool> query, Store store, CancellationToken ct);
    Task<Store> InsertAsync(Store storeEntity, CancellationToken ct);
    Task<Store?> GetByIdAsync(Guid storeId, CancellationToken ct);
    Task<List<Store>> SearchStoresAsync(SearchStoresRequest request, CancellationToken ct);
}