using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Queries;
using Feirapp.Entities.Dtos;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemRepository
{
    Task<List<SearchGroceryItemsDto>> SearchGroceryItemsAsync(SearchGroceryItemsQuery query, CancellationToken ct);
    Task<GroceryItem?> CheckIfGroceryItemExistsAsync(GroceryItem groceryItem, Guid storeId, CancellationToken ct);
    Task InsertPriceLog(PriceLog priceLog, CancellationToken ct);
    Task<PriceLog?> GetLastPriceLogAsync(Guid groceryItemId, Guid storeId, CancellationToken ct);
    Task<StoreWithItems> GetByStoreAsync(Guid storeId, CancellationToken ct);
    Task<List<SearchGroceryItemsDto>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct);
    Task<GroceryItem> InsertAsync(GroceryItem groceryItem, CancellationToken ct);
    Task<GroceryItem?> GetByIdAsync(Guid id, CancellationToken ct);
}