using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.Stores.Interfaces;

public interface IStoreRepository
{
    Task<Store?> GetByCnpjAsync(string? storeCnpj, CancellationToken ct);
    Task<Store> AddIfNotExistsAsync(Func<Store, bool> query, Store store, CancellationToken ct);
    Task UpdateAsync(Store store, CancellationToken ct);
    Task<Store> InsertAsync(Store storeEntity, CancellationToken ct);
    Task<List<Store>> GetAllAsync(CancellationToken ct);
    Task<Store?> GetByIdAsync(long storeId, CancellationToken ct);
}