using Feirapp.Domain.Models;

namespace Feirapp.Domain.Contracts;

public interface IGroceryItemService
{
    Task<List<GroceryItem>> GetAllGroceryItems();
}