using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Queries;
using Feirapp.Entities.Dtos;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemRepository
{
    Task<List<SearchGroceryItemsDto>> SearchGroceryItemsAsync(SearchGroceryItemsQuery query, CancellationToken ct);
    Task<GroceryItem?> CheckIfGroceryItemExistsAsync(GroceryItem groceryItem, long storeId, CancellationToken ct);
    Task InsertPriceLog(PriceLog? priceLog, CancellationToken ct);
    Task<PriceLog?> GetLastPriceLogAsync(long groceryItemId, long storeId, CancellationToken ct);
    Task<StoreWithItems> GetByStoreAsync(long storeId, CancellationToken ct);
    Task<List<SearchGroceryItemsDto>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct);
    Task<GroceryItem> InsertAsync(GroceryItem groceryItem, CancellationToken ct);
    Task UpdateAsync(GroceryItem groceryItem, CancellationToken ct);
    Task<GroceryItem> GetByIdAsync(long id, CancellationToken ct);
}