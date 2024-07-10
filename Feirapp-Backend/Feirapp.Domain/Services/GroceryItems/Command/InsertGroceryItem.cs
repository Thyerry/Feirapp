namespace Feirapp.Domain.Services.GroceryItems.Command;

public record InsertGroceryItem(
    string Name,
    decimal Price,
    string MeasureUnit,
    string Barcode,
    DateTime PurchaseDate,
    string NcmCode,
    string CestCode
)
{
    public decimal Quantity { get; set; }
};