using Feirapp.Domain.Contracts.Repository;
using Feirapp.Domain.Models;
using Feirapp.Entities;

namespace Feirapp.Domain.Contracts.Service;

public interface IGroceryItemService
{
    Task<List<GroceryItemModel>> GetAllGroceryItems();

    Task<List<GroceryItemModel>> GetRandomGroceryItems(int quantity);

    Task<GroceryItemModel> GetGroceryItemById(string groceryId);

    Task<GroceryItemModel> CreateGroceryItem(GroceryItemModel groceryItem);

    Task UpdateGroceryItem(GroceryItemModel groceryItem);

    Task DeleteGroceryItem(string groceryId);
}