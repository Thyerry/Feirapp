using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Domain.Services.GroceryItems.Queries;
using Feirapp.Domain.Services.GroceryItems.Responses;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemService
{
    Task<List<SearchGroceryItemsResponse>> SearchGroceryItemsAsync(SearchGroceryItemsQuery query, CancellationToken ct);
    Task<GetGroceryItemByIdResponse?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<GetGroceryItemFromStoreIdResponse> GetByStoreAsync(Guid storeId, CancellationToken ct);
    Task<List<SearchGroceryItemsResponse>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct);
    Task InsertAsync(InsertGroceryItemCommand command, CancellationToken ct);
    Task InsertListAsync(InsertListOfGroceryItemsCommand command, CancellationToken ct);
    Task UpdateAsync(UpdateGroceryItemCommand groceryItem, CancellationToken ct);
    Task DeleteAsync(Guid groceryId, CancellationToken ct);
}