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
        return await GetGroceryItemById(groceryItem.Id!);
    }

    public async Task<GroceryItem> GetGroceryItemById(string groceryId)
    {
        var result = (await _collection.FindAsync(p => p.Id == groceryId)).ToList();
        return result.FirstOrDefault()!;
    }

    public async Task<GroceryItem> UpdateGroceryItem(GroceryItem groceryItem)
    {
        await _collection.UpdateOneAsync(
            g => g.Id == groceryItem.Id,
            Builders<GroceryItem>.Update
                .Set(n => n.Name, groceryItem.Name)
                .Set(n => n.Price, groceryItem.Price)
                .Set(n => n.BrandName, groceryItem.BrandName)
                .Set(n => n.PurchaseDate, groceryItem.PurchaseDate)
                .Set(n => n.GroceryCategory, groceryItem.GroceryCategory)
                .Set(n => n.GroceryImageUrl, groceryItem.GroceryImageUrl)
                .Set(n => n.GroceryStoreName, groceryItem.GroceryStoreName),
            new UpdateOptions { IsUpsert = false } );
        
        return await GetGroceryItemById(groceryItem.Id!);
    }

    public async Task DeleteGroceryItem(string groceryId)
    {
        await _collection.DeleteOneAsync(groceryItem => groceryItem.Id == groceryId);
    }
}