namespace Feirapp.Domain.Services.GroceryItems.Dtos.Commands;

public record GroceryItemDto(
    string Name,
    decimal Price,
    string? Barcode = null,
    string? Description = null,
    string? ImageUrl = null,
    string? NcmCode = null,
    string? CestCode = null
);