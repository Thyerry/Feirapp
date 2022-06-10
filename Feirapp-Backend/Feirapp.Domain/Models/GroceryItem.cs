using Feirapp.Domain.Enums;

namespace Feirapp.Domain.Models;

public class GroceryItem
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public double? Price { get; set; }
    public GroceryCategoryEnum GroceryCategory { get; set; }
    public string? BrandName { get; set; }
    public string GroceryStoreName { get; set; }
    public DateTime? PurchaseDate { get; set; }
}