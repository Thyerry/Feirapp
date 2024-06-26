using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Commands;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemService
{
    Task<GetGroceryItemResponse> GetAllAsync(CancellationToken ct);
    Task InsertBatchAsync(List<InvoiceGroceryItem> items, InvoiceStore invoiceStore, CancellationToken ct);
    Task<GetGroceryItemResponse> GetFromStoreAsync(long storeId, CancellationToken ct);
}