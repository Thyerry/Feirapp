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
}