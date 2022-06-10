using Feirapp.Domain.Contracts;
using Feirapp.Domain.Models;

namespace Feirapp.Service.Services;

public class GroceryItemService : IGroceryItemService
{
    private readonly IGroceryItemRepository _groceryItemRepository;

    public GroceryItemService(IGroceryItemRepository groceryItemRepository)
    {
        _groceryItemRepository = groceryItemRepository;
    }

    public async Task<List<GroceryItem>> GetAllGroceryItems()
    {
        return await _groceryItemRepository.GetAllGroceryItems();
    }
}