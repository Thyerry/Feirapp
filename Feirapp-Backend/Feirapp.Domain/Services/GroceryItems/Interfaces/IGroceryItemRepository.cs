using Feirapp.Domain.Services.BaseRepository;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Queries;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemRepository : IBaseRepository<GroceryItem>
{
    Task<List<GroceryItemList>> ListGroceryItemsAsync(ListGroceryItemsQuery query, CancellationToken ct);
    Task<GroceryItem?> CheckIfGroceryItemExistsAsync(GroceryItem groceryItem, long storeId, CancellationToken ct);
    Task InsertPriceLog(PriceLog priceLog, CancellationToken ct);
    Task<PriceLog> GetLastPriceLogAsync(long groceryItemId, CancellationToken ct);
}