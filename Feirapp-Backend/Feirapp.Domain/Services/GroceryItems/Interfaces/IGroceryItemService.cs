using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Command;
using Feirapp.Domain.Services.GroceryItems.Dtos.Queries;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemService
{
    Task<List<ListGroceryItemsResponse>> ListGroceryItemsAsync(ListGroceryItemsQuery query, CancellationToken ct);
    Task<GetGroceryItemResponse> GetByIdAsync(long id, CancellationToken ct);
    Task InsertBatchAsync(List<InvoiceGroceryItem> invoiceItems, InvoiceStore store, CancellationToken ct);
}