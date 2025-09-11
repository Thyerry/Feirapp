using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;

public record GetGroceryItemByIdResponse(
    Guid Id,
    string Name,
    string? Description,
    string? ImageUrl,
    string? Brand,
    string Barcode,
    string NcmCode,
    string CestCode,
    MeasureUnitEnum MeasureUnit,
    List<GetGroceryItemByIdPriceLogDto> PriceHistory
);