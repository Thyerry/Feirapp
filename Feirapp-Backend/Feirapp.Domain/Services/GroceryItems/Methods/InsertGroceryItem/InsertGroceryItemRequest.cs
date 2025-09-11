using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItem;

public class InsertGroceryItemRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string Barcode { get; set; }
    public string ProductCode { get; set; }
    public MeasureUnitEnum MeasureUnit { get; set; }
    public InsertGroceryItemStoreDto? Store { get; set; }
    public DateTime? PurchaseDate { get; set; }
}