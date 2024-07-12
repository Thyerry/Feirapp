using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Responses;

public record SearchGroceryItemsResponse(
    long Id,
    string Name,
    string? Description,
    decimal LastPrice,
    string? ImageUrl,
    string Barcode,
    DateTime LastUpdate,
    MeasureUnitEnum MeasureUnit,
    long StoreId,
    string StoreName
);