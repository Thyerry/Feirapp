namespace Feirapp.Domain.Services.GroceryItems.Responses;

public record InsertGroceryItemResponse(
    bool Success,
    List<string> Messages
);