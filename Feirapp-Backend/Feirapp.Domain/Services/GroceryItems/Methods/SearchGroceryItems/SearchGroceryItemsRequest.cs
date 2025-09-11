namespace Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;

public record SearchGroceryItemsRequest(string? Name, Guid? StoreId, int Page = 0, int PageSize = 10);
