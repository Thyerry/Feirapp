using Feirapp.Domain.Models;

namespace Feirapp.Domain.Contracts;

public interface IGroceryItemService
{
    Task<List<GroceryItem>> GetAllGroceryItems();
    Task<List<GroceryItem>> GetByName(string groceryName);
    Task<GroceryItem> CreateGroceryItem(GroceryItem groceryItem);
}