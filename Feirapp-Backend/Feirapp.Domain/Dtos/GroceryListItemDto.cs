using Feirapp.DocumentModels.Enums;
using Feirapp.Domain.Services.GroceryItems.Dtos;

namespace Feirapp.Domain.Dtos;

public class GroceryListItemDto
{
    public decimal Quantity { get; set; }
    public decimal UnitaryPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public GroceryItemDto Item { get; set; } = null!;
    public MeasureUnitEnum MeasureUnit { get; set; }

    public override string ToString()
    {
        return $"{Item.Name}";
    }
}