using Feirapp.Domain.Contracts;
using Feirapp.Domain.Models;

namespace Feirapp.DAL.Repositories;

public class GroceryItemRepository : IGroceryItemRepository
{
    public async Task<List<GroceryItem>> GetAllGroceryItems()
    {
        return new List<GroceryItem>();
    }
}