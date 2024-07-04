using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.Stores.Dtos.Commands;

namespace Feirapp.Domain.Services.Stores.Interfaces;

public interface IStoreService
{
    Task InsertStoreAsync(InsertStoreCommand store, CancellationToken ct);
    Task<List<StoreDto>?> GetAllStoresAsync(CancellationToken ct);
    Task<StoreDto?> GetStoreById(long storeId, CancellationToken ct);
}