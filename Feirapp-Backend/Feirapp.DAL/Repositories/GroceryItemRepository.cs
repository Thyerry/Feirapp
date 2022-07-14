using Feirapp.DAL.DataContext;
using Feirapp.Domain.Contracts;
using Feirapp.Domain.Models;
using MongoDB.Driver;

namespace Feirapp.DAL.Repositories;

public class GroceryItemRepository : IGroceryItemRepository
{
    private readonly IMongoCollection<GroceryItem> _collection;
    public GroceryItemRepository(IMongoFeirappContext context)
    {
        _collection = context.GetCollection<GroceryItem>(nameof(GroceryItem));
    }

    public async Task<List<GroceryItem>> GetAllGroceryItems()
    {
        var result = await _collection.FindAsync(q => true);
        return result.ToList();
    }

    public async Task<List<GroceryItem>> GetGroceryItemsByName(string groceryName)
    {
        var groceryItems =
            await _collection.FindAsync(g => g.Name!.Contains(groceryName));
        return groceryItems.ToList();
    }

    public async Task<GroceryItem> CreateGroceryItem(GroceryItem groceryItem)
    {
        await _collection.InsertOneAsync(groceryItem);
        // TODO: Change this for GetById once the method is created
        return (await GetGroceryItemsByName(groceryItem.Name)).FirstOrDefault();
    }

    public async Task<GroceryItem> GetGroceryItemById(string groceryId)
    {
        var result = await _collection.FindAsync(p => p.Id == groceryId);
        return result.Current.FirstOrDefault()!;
    }
}