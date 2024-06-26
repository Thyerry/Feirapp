using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

public record GetAllGroceryItemsResponse(
    long Id,
    string Name,
    string Description,
    decimal Price,
    string ImageUrl,
    string Barcode,
    DateTime LastUpdate,
    DateTime PurchaseDate,
    StoreDto Store,
    MeasureUnitEnum MeasureUnit,
    List<PriceLogDto> PriceHistory
);