using Feirapp.Domain.Services.Stores.Methods.GetStoreById;
using Feirapp.Domain.Services.Stores.Methods.InsertGroceryItem;
using Feirapp.Domain.Services.Stores.Methods.SearchStores;
using Feirapp.Domain.Services.Utils;

namespace Feirapp.Domain.Services.Stores.Interfaces;

public interface IStoreService
{
    Task<Result<bool>> InsertStoreAsync(InsertStoreRequest store, CancellationToken ct);
    Task<Result<GetStoreByIdResponse>> GetStoreByIdAsync(Guid storeId, CancellationToken ct);
    Task<Result<List<SearchStoresResponse>>> SearchStoresAsync(SearchStoresRequest request, CancellationToken ct);
}