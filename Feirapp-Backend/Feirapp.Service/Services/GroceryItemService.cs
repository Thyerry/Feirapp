using Feirapp.Domain.Contracts;
using Feirapp.Domain.Models;
using MongoDB.Bson;

namespace Feirapp.Service.Services;

public class GroceryItemService : IGroceryItemService
{
    private readonly IGroceryItemRepository _repository;

    public GroceryItemService(IGroceryItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<GroceryItem>> GetAllGroceryItems()
    {
        return await _repository.GetAllGroceryItems();
    }

    public async Task<List<GroceryItem>> GetGroceryItemByName(string groceryName)
    {
        return await _repository.GetGroceryItemsByName(groceryName.ToUpper());
    }

    public async Task<GroceryItem> CreateGroceryItem(GroceryItem groceryItem)
    {
        // TODO: Validate the groceryItem fields here before calling the repository
        return await _repository.CreateGroceryItem(groceryItem);
    }

    public async Task<GroceryItem> GetGroceryItemById(string groceryId)
    {
        return await _repository.GetGroceryItemById(groceryId);
    }

    public async Task<GroceryItem> UpdateGroceryItem(GroceryItem groceryItem)
    {        
        // TODO: Validate the groceryItem fields here before calling the repository
        return await _repository.UpdateGroceryItem(groceryItem);
    }

    public async Task DeleteGroceryItem(string groceryId)
    {
        await _repository.DeleteGroceryItem(groceryId);
    }
}