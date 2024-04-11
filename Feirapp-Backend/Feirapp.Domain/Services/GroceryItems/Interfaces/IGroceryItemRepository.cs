using Feirapp.Domain.Services.BaseRepository;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemRepository : IBaseRepository<GroceryItem>
{
    Task<List<GroceryItem>> GetRandomGroceryItems(int quantity, CancellationToken ct = default);
    Task<GroceryItem?> GetByBarcodeAndStoreIdAsync(string itemBarcode, long itemStoreId, CancellationToken ct);
    Task InsertPriceLogAsync(PriceLog priceLog, CancellationToken ct);
    Task<PriceLog> GetLastPriceLogAsync(long groceryItemId, CancellationToken ct);
}