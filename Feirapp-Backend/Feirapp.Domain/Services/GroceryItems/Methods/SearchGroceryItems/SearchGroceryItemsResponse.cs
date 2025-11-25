using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;

public class SearchGroceryItemsResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal LastPrice { get; set; }
    public string? ImageUrl { get; set; }
    public string Barcode { get; set; }
    public DateTime LastUpdate { get; set; }
    public MeasureUnitEnum MeasureUnit { get; set; }
    public Guid StoreId { get; set; }
    public string StoreName { get; set; }
}