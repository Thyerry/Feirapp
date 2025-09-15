namespace Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItems;

public record InsertGroceryItemsRequest(List<InsertGroceryItemsDto> GroceryItems, InsertGroceryItemsStoreDto Store);