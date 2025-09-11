namespace Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;

public class GetGroceryItemByIdPriceLogDto
{
    public decimal Price { get; set; }
    public DateTime LogDate { get; set; }
    public GetGroceryItemByIdStoreDto Store { get; set; }
}