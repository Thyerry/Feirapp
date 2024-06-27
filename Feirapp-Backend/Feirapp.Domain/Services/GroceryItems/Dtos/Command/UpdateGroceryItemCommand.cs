using FluentValidation;
using FluentValidation.Results;

namespace Feirapp.Domain.Services.GroceryItems.Dtos.Command;

public record UpdateGroceryItemCommand(
    string Barcode,
    long StoreId,
    string? Brand,
    string? Description,
    string? ImageUrl,
    decimal Price,
    DateTime PurchaseDate)
{
    public void Validate()
    {
        var errors = new List<ValidationFailure>();
        if (string.IsNullOrWhiteSpace(Barcode))
            errors.Add(new ValidationFailure(nameof(Barcode), "Barcode cannot be null or empty.", Barcode));
        if (Barcode.Length <= 13)
            errors.Add(new ValidationFailure(nameof(Barcode), "Barcode must have 13 characters.", Barcode));
        if (Barcode.Any(c => !char.IsDigit(c)))
            errors.Add(new ValidationFailure(nameof(Barcode), "Barcode must contain only digits.", Barcode));
        if (StoreId <= 0)
            errors.Add(new ValidationFailure(nameof(StoreId), "StoreId must be greater than 0.", StoreId));
        if (Price <= 0)
            errors.Add(new ValidationFailure(nameof(Price), "Price must be greater than 0.", Price));

        if (errors.Count > 0)
        {
            throw new ValidationException("Update failed", errors);
        }
    }
}