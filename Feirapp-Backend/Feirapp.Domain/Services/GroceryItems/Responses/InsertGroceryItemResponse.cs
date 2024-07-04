using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

public record InsertGroceryItemResponse(
    bool Success,
    List<string> Messages
);