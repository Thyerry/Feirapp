namespace Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;

public record SearchGroceryItemsRequest(string? Name = null, Guid? StoreId = null, int PageIndex = 0, int PageSize = 10);
