namespace Feirapp.Domain.Models;

public class PriceLogModel
{
    public string? Id { get; set; }
    public string? GroceryItemId { get; set; }
    public decimal Price { get; set; }
    public DateTime LogDate { get; set; }
}