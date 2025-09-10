using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Dtos;

public class InvoiceScanGroceryItem
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string MeasureUnit { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public string NcmCode { get; set; } = string.Empty;
    public string CestCode { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public List<string> ImportIssues { get; set; } = [];
};