using System.Text.RegularExpressions;
using Feirapp.DAL.DataContext;
using Feirapp.Domain.Contracts;
using Feirapp.Domain.Models;
using MongoDB.Bson;
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
        return result.ToList();
    }

    public async Task<List<GroceryItem>> GetByName(string groceryName)
    {
        var groceryItems =
            await _groceryItemCollection.FindAsync(g => g.Name!.Contains(groceryName));
        return groceryItems.ToList();
    }
}