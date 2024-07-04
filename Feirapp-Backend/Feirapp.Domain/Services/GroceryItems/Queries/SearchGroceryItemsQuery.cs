using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

namespace Feirapp.Domain.Services.GroceryItems.Dtos.Queries;

public record SearchGroceryItemsQuery(string? Name, long StoreId, int Page, int PageSize);

