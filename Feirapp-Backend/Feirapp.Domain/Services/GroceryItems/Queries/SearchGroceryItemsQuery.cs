namespace Feirapp.Domain.Services.GroceryItems.Queries;

public record SearchGroceryItemsQuery(string? Name, long StoreId, int Page, int PageSize);

