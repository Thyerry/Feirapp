using Feirapp.Domain.Models;

namespace Feirapp.Domain.Contracts;

public interface IGroceryItemRepository
{
    Task<List<GroceryItem>> GetAllGroceryItems();
}