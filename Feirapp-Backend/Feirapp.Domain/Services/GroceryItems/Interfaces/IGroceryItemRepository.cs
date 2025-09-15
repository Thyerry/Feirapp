using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;
using Feirapp.Entities.Dtos;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemRepository
{
    Task<List<SearchGroceryItemsDto>> SearchGroceryItemsAsync(SearchGroceryItemsRequest request, CancellationToken ct);
    Task<GroceryItem?> CheckIfGroceryItemExistsAsync(string barcode, string productCode, Guid storeId, CancellationToken ct);
    Task InsertPriceLog(PriceLog priceLog, CancellationToken ct);
    Task<PriceLog?> GetLastPriceLogAsync(Guid groceryItemId, Guid storeId, CancellationToken ct);
    Task<StoreWithItems> GetByStoreAsync(Guid storeId, CancellationToken ct);
    Task<List<SearchGroceryItemsDto>> GetRandomAsync(int quantity, CancellationToken ct);
    Task<GroceryItem> InsertAsync(GroceryItem groceryItem, CancellationToken ct);
    Task<GetGroceryItemByIdDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task DeleteAsync(Guid groceryId, CancellationToken ct);
}