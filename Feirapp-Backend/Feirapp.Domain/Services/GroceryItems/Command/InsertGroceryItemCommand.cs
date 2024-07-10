using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Command;

public record InsertGroceryItemCommand(
    string Name,
    decimal Price,
    string? Description,
    string? ImageUrl,
    string Barcode,
    MeasureUnitEnum MeasureUnit,
    StoreDto? Store,
    DateTime? PurchaseDate = null);