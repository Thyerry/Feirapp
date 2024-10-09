namespace Feirapp.Domain.Services.GroceryItems.Queries;

public record CreateGroceryItemQuery(int Quantity = 1, int ProductSeed = 0, int StoreSeed = 0);