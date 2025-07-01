using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Dtos;

public record GroceryItemDto(
    Guid Id,
    string Name,
    string Description,
    string ImageUrl,
    string Barcode,
    MeasureUnitEnum MeasureUnit,
    List<PriceLogDto>? PriceHistory
)
{
    public decimal Price { get; set; }
};
