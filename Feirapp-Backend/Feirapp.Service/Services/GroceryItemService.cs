using Feirapp.Domain.Contracts;
using Feirapp.Domain.Models;

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

    public async Task<List<GroceryItem>> GetByName(string groceryName)
    {
        return await _repository.GetByName(groceryName.ToUpper());
    }

    public async Task<GroceryItem> CreateGroceryItem(GroceryItem groceryItem)
    {
        // TODO: Validate the groceryItem fields here and apply
        return await _repository.CreateGroceryItem(groceryItem);
    }
}