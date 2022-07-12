using Feirapp.DAL.DataContext;
using Feirapp.Domain.Contracts;
using Feirapp.Domain.Models;
using MongoDB.Driver;

namespace Feirapp.DAL.Repositories;

public class GroceryItemRepository : IGroceryItemRepository
{
    private readonly IMongoCollection<GroceryItem> _groceryItemCollection;
    public GroceryItemRepository(IMongoFeirappContext context)
    {
        _groceryItemCollection = context.GetCollection<GroceryItem>(nameof(GroceryItem));
    }

    public async Task<List<GroceryItem>> GetAllGroceryItems()
    {
        var result = await _groceryItemCollection.FindAsync(q => true);
        return await result.ToListAsync();
    }
}