using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Command;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemService
{
    Task<List<GetAllGroceryItemsResponse>> GetAllAsync(CancellationToken ct);
    Task InsertBatchAsync(List<InvoiceGroceryItem> items, InvoiceStore invoiceStore, CancellationToken ct);
    Task<GetGroceryItemResponse> GetByStoreAsync(long storeId, CancellationToken ct);
    Task<List<GetAllGroceryItemsResponse>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct);
    Task<GroceryItemDto> InsertAsync(GroceryItemDto groceryItem, CancellationToken ct);
    Task<GroceryItemDto> GetByIdAsync(long id, CancellationToken ct);
    Task UpdateAsync(UpdateGroceryItemCommand command, CancellationToken ct);
}