using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

public record GetGroceryItemByIdResponse(
    long Id,
    string Name,
    string? Description,
    string? ImageUrl,
    string? Brand,
    string Barcode,
    string NcmCode,
    string CestCode,
    MeasureUnitEnum MeasureUnit,
    List<PriceLogDto> PriceHistory
);