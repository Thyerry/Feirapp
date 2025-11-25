namespace Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemsByStore;

public class GetGroceryItemsByStoreIdResponse
{
    public GetGroceryItemsByStoreStoreDto Store { get; set; }
    public List<GetGroceryItemsByStoreGroceryItemDto> Items { get; set; }
}