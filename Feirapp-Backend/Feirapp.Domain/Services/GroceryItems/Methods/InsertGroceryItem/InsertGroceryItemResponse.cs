namespace Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItem;

public record InsertGroceryItemResponse(
    bool Success,
    List<string> Messages
);