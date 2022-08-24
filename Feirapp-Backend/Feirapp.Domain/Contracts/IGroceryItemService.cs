using Feirapp.Domain.Models;

namespace Feirapp.Domain.Contracts;

public interface IGroceryItemService
{
    Task<List<GroceryItem>> GetAllGroceryItems();
    Task<List<GroceryItem>> GetRandomGroceryItems(int quantity);
    Task<List<GroceryItem>> GetGroceryItemByName(string groceryName);
    Task<GroceryItem> GetGroceryItemById(string groceryId);
    Task<GroceryItem> CreateGroceryItem(GroceryItem groceryItem);
    Task<GroceryItem> UpdateGroceryItem(GroceryItem groceryItem);
    Task DeleteGroceryItem(string groceryId);
}