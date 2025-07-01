namespace Feirapp.Domain.Services.GroceryItems.Queries;

public record SearchGroceryItemsQuery(string? Name, Guid? StoreId, int Page = 0, int PageSize = 10);

