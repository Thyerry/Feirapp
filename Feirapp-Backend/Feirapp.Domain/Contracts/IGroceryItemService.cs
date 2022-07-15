using Feirapp.Domain.Models;

namespace Feirapp.Domain.Contracts;

public interface IGroceryItemService
{
    Task<List<GroceryItem>> GetAllGroceryItems();
    Task<List<GroceryItem>> GetGroceryItemByName(string groceryName);
    Task<GroceryItem> CreateGroceryItem(GroceryItem groceryItem);
    Task<GroceryItem> GetGroceryItemById(string groceryId);
    Task<GroceryItem> UpdateGroceryItem(GroceryItem groceryItem);
    Task DeleteGroceryItem(string groceryId);
}