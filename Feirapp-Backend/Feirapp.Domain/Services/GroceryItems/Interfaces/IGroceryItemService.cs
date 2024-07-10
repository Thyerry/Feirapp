using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Domain.Services.GroceryItems.Queries;
using Feirapp.Domain.Services.GroceryItems.Responses;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemService
{
    Task<List<SearchGroceryItemsResponse>> SearchGroceryItemsAsync(SearchGroceryItemsQuery query, CancellationToken ct);
    Task<GetGroceryItemByIdResponse?> GetByIdAsync(long id, CancellationToken ct);
    Task InsertBatchAsync(List<InsertGroceryItem> invoiceItems, InvoiceStore store, CancellationToken ct);
    Task<GetGroceryItemFromStoreIdResponse> GetByStoreAsync(long storeId, CancellationToken ct);
    Task<List<SearchGroceryItemsResponse>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct);
    Task InsertAsync(InsertGroceryItemCommand command, CancellationToken ct);
    Task InsertListAsync(InsertListOfGroceryItemsCommand command, CancellationToken ct);
}