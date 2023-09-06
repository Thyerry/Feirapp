using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Models;

public class GroceryItemModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public string? CEAN { get; set; }
    public GroceryCategoryModel? Category { get; set; }
    public string? Brand { get; set; }
    public string? GroceryStore { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public MeasureUnitEnum? MeasureUnit { get; set; }
    public string? ImageUrl { get; set; }
    public List<PriceLogModel>? PriceHistory { get; set; }
}