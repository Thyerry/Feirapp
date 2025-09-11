using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemsByStore;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItem;
using Feirapp.Domain.Services.GroceryItems.Methods.InsertListOfGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;
using Feirapp.Domain.Services.GroceryItems.Methods.UpdateGroceryItemCommand;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemService
{
    Task<List<SearchGroceryItemsResponse>> SearchGroceryItemsAsync(SearchGroceryItemsRequest request, CancellationToken ct);
    Task<GetGroceryItemByIdResponse?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<GetGroceryItemFromStoreIdResponse> GetByStoreAsync(Guid storeId, CancellationToken ct);
    Task<List<SearchGroceryItemsResponse>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct);
    Task InsertAsync(InsertGroceryItemRequest request, CancellationToken ct);
    Task InsertListAsync(InsertListOfGroceryItemsRequest request, CancellationToken ct);
    Task UpdateAsync(UpdateGroceryItemCommand groceryItem, CancellationToken ct);
    Task DeleteAsync(Guid groceryId, CancellationToken ct);
}