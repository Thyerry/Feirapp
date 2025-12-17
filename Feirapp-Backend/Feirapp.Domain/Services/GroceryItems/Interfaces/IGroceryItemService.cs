using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemsByStore;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;
using Feirapp.Domain.Services.Utils;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemService
{
    Task<Result<List<SearchGroceryItemsResponse>>> SearchAsync(SearchGroceryItemsRequest request, CancellationToken ct);
    Task<Result<GetGroceryItemByIdResponse>> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Result<GetGroceryItemsByStoreIdResponse>> GetByStoreAsync(Guid storeId, CancellationToken ct);
    Task<Result<int>> InsertAsync(InsertGroceryItemsRequest request, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(Guid groceryId, CancellationToken ct);
}