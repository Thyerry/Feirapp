namespace Feirapp.Domain.Services.DataScrapper.Dtos;

public record InvoiceGroceryItem
(
    string Name,
    decimal Price,
    string MeasureUnit,
    string Barcode,
    DateTime PurchaseDate,
    string Ncm,
    string Cest
)
{
    public decimal Quantity { get; set; }
};