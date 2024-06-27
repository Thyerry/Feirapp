using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Dtos;

public record GroceryItemDto(
    long Id,
    string Name,
    string Description,
    decimal Price,
    string ImageUrl,
    string Barcode,
    DateTime LastUpdate,
    DateTime PurchaseDate,
    MeasureUnitEnum MeasureUnit,
    List<PriceLogDto> PriceHistory
);