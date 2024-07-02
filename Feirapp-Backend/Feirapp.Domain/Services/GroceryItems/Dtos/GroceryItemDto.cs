using Feirapp.Entities.Enums;
using FluentValidation.Results;

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
    List<PriceLogDto>? PriceHistory
)
{
    public List<ValidationFailure> ValidationForImportation()
    {
        var errors = new List<ValidationFailure>();
        if (string.IsNullOrWhiteSpace(Name))
            errors.Add(new ValidationFailure(nameof(Name), "Name cannot be null or empty.", Name));
        if (Price <= 0)
            errors.Add(new ValidationFailure(nameof(Price), "Price must be greater than 0.", Price));
        if (string.IsNullOrWhiteSpace(Barcode))
            errors.Add(new ValidationFailure(nameof(Barcode), "Barcode cannot be null or empty.", Barcode));
        if (Barcode.Length <= 13)
            errors.Add(new ValidationFailure(nameof(Barcode), "Barcode must have 13 characters.", Barcode));
        if (Barcode.Any(c => !char.IsDigit(c)))
            errors.Add(new ValidationFailure(nameof(Barcode), "Barcode must contain only digits.", Barcode));
        if (MeasureUnit == MeasureUnitEnum.EMPTY)
            errors.Add(new ValidationFailure(nameof(MeasureUnit), "MeasureUnit cannot be undefined.", MeasureUnit));

        return errors;
    }

    public List<ValidationFailure> ValidationToManualInsert()
    {
        var errors = new List<ValidationFailure>();
        if (string.IsNullOrWhiteSpace(Name))
            errors.Add(new ValidationFailure(nameof(Name), "Name cannot be null or empty.", Name));
        
        return errors;
    }
}