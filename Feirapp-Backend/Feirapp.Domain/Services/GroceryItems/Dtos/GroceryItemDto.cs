using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;
using FluentValidation.Results;

namespace Feirapp.Domain.Services.GroceryItems.Dtos;

public record GroceryItemDto(
    long Id,
    string Name,
    string Description,
    string ImageUrl,
    string Barcode,
    MeasureUnitEnum MeasureUnit,
    List<PriceLogDto>? PriceHistory
);
