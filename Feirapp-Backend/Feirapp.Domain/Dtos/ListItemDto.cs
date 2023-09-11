using Feirapp.DocumentModels;
using Feirapp.DocumentModels.Enums;

namespace Feirapp.Domain.Dtos;

public class ListItemDto
{
    public decimal Quantity { get; set; }
    public decimal UnitaryPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public GroceryItem GroceryItem { get; set; }
    public MeasureUnitEnum MeasureUnit { get; set; }
}