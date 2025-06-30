using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Dtos;

public record GroceryItemByStore(
    Guid Id,
    string Name,
    string? Description,
    string? ImageUrl,
    string Barcode,
    MeasureUnitEnum MeasureUnit
);