namespace Feirapp.Domain.Services.GroceryItems.Command;

public record InsertListOfGroceryItemsCommand(List<InsertGroceryItem> GroceryItems, InsertStore Store);