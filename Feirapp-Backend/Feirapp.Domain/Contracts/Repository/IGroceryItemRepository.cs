using Feirapp.Entities;

namespace Feirapp.Domain.Contracts.Repository;

public interface IGroceryItemRepository
{
    Task<List<GroceryItem>> GetAllGroceryItems();

    Task<List<GroceryItem>> GetRandomGroceryItems(int quantity);

    Task<GroceryItem> CreateGroceryItem(GroceryItem groceryItem);

    Task<GroceryItem> GetGroceryItemById(string groceryId);

    Task<GroceryItem> UpdateGroceryItem(GroceryItem groceryItem);

    Task DeleteGroceryItem(string groceryId);
}