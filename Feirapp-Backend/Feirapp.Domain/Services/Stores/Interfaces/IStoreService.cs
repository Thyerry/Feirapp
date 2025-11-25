using Feirapp.Domain.Services.Stores.Methods.GetStoreById;
using Feirapp.Domain.Services.Stores.Methods.InsertGroceryItem;
using Feirapp.Domain.Services.Stores.Methods.SearchStores;

namespace Feirapp.Domain.Services.Stores.Interfaces;

public interface IStoreService
{
    Task InsertStoreAsync(InsertStoreRequest store, CancellationToken ct);
    Task<GetStoreByIdResponse?> GetStoreByIdAsync(Guid storeId, CancellationToken ct);
    Task<List<SearchStoresResponse>> SearchStoresAsync(SearchStoresRequest request, CancellationToken ct);
}