using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Dtos;

public record SearchGroceryItemsDto(
    long Id,
    string Name,
    string? Description,
    decimal LastPrice,
    string? ImageUrl,
    string Barcode,
    DateTime LastUpdate,
    MeasureUnitEnum MeasureUnit,
    long StoreId,
    string StoreName);
