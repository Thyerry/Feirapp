using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemsByStore;

public class GetGroceryItemsByStoreGroceryItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string Barcode { get; set; }
    public MeasureUnitEnum MeasureUnit { get; set; }
}