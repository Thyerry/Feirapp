using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemsByStore;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemService
{
    Task<List<SearchGroceryItemsResponse>> SearchAsync(SearchGroceryItemsRequest request, CancellationToken ct);
    Task<GetGroceryItemByIdResponse?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<GetGroceryItemsByStoreIdResponse> GetByStoreAsync(Guid storeId, CancellationToken ct);
    Task InsertAsync(InsertGroceryItemsRequest request, CancellationToken ct);
    Task DeleteAsync(Guid groceryId, CancellationToken ct);
}