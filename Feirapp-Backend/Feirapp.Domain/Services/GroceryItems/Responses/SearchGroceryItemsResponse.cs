using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Responses;

public record SearchGroceryItemsResponse(
    Guid Id,
    string Name,
    string? Description,
    decimal LastPrice,
    string? ImageUrl,
    string Barcode,
    DateTime LastUpdate,
    MeasureUnitEnum MeasureUnit,
    Guid StoreId,
    string StoreName
);