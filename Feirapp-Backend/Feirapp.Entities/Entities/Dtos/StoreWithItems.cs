namespace Feirapp.Entities.Entities.Dtos;

public class StoreWithItems
{
    public Store Store { get; set; }
    public List<GroceryItem> Items { get; set; }
}