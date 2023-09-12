using Feirapp.DocumentModels;
using Feirapp.DocumentModels.Enums;
using System.Xml.Linq;

namespace Feirapp.Domain.Dtos;

public class ListItemDto
{
    public decimal Quantity { get; set; }
    public decimal UnitaryPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public GroceryItem GroceryItem { get; set; }
    public MeasureUnitEnum MeasureUnit { get; set; }

    public override string ToString()
    {
        return $"{GroceryItem.Name}";
    }
}