namespace Feirapp.Domain.Services.GroceryItems.Dtos;

public record PriceLogDto
{
    private decimal Price { get; set; }
    private DateTime LogDate { get; set; }
    private StoreDto Store { get; set; }
}