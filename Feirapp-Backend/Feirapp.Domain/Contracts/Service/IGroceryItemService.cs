using Feirapp.Domain.Models;

namespace Feirapp.Domain.Contracts.Service;

public interface IGroceryItemService
{
    Task<List<GroceryItemModel>> GetAllGroceryItems();

    Task<List<GroceryItemModel>> GetRandomGroceryItems(int quantity);

    Task<GroceryItemModel> GetGroceryItemById(string groceryId);

    Task<GroceryItemModel> CreateGroceryItem(GroceryItemModel groceryItem);

    Task<GroceryItemModel> UpdateGroceryItem(GroceryItemModel groceryItem);

    Task DeleteGroceryItem(string groceryId);
}