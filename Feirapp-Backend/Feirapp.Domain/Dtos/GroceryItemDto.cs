namespace Feirapp.Domain.Dtos;

public class GroceryItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    public string Cean { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public GroceryCategoryDto? Category { get; set; }
    public List<PriceLogDto>? PriceHistory { get; set; }

    public override string ToString()
    {
        return $"{Name}";
    }
}