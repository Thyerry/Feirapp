namespace Feirapp.Domain.Services.GroceryItems.Misc;

public class GenericGroceryItemDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string MeasureUnit { get; set; }
    public string Barcode { get; set; }
    public string ProductCode { get; set; }
    public string? Brand { get; set; }
    public List<string>? AltNames { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string NcmCode { get; set; }
    public string? CestCode { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
}