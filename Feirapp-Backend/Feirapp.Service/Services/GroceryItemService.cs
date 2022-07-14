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
        // TODO: Validate the groceryItem fields here and apply
        return await _repository.CreateGroceryItem(groceryItem);
    }

    public async Task<GroceryItem> GetGroceryItemById(string groceryId)
    {
        return await _repository.GetGroceryItemById(groceryId);
    }
}