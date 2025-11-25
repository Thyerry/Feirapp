namespace Feirapp.Domain.Services.GroceryItems.Methods.InsertGroceryItems;

public class InsertGroceryItemsRequest
{
    public List<InsertGroceryItemsDto> GroceryItems { get; set; }
    public InsertGroceryItemsStoreDto Store { get; set; }
}