using Feirapp.Domain.Services.BaseRepository;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Queries;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Entities.Dtos;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemRepository : IBaseRepository<GroceryItem>
{
    Task<List<SearchGroceryItemsDto>> SearchGroceryItemsAsync(SearchGroceryItemsQuery query, CancellationToken ct);
    Task<(GroceryItem?, Store?)> CheckIfGroceryItemExistsAsync(GroceryItem groceryItem, long storeId,
        CancellationToken ct);
    Task InsertPriceLog(PriceLog? priceLog, CancellationToken ct);
    Task<PriceLog?> GetLastPriceLogAsync(long groceryItemId, long storeId, CancellationToken ct);
    Task<StoreWithItems> GetByStoreAsync(long storeId, CancellationToken ct);
    Task<List<SearchGroceryItemsDto>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct);
}