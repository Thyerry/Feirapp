using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Responses;

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