using Feirapp.DAL.DataContext;
using Feirapp.Domain.Contracts;
using Feirapp.Domain.Models;
using MongoDB.Driver;

namespace Feirapp.DAL.Repositories;

public class GroceryItemRepository : IGroceryItemRepository, IDisposable
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

    public async Task<List<GroceryItem>> GetRandomGroceryItems(int quantity)
    {
        var result = await _groceryItemCollection.AggregateAsync(PipelineDefinition<GroceryItem, GroceryItem>.Create($@"
        {{
            $sample: {{ size: {quantity} }}
        }}
        "));
        return result.ToList();
    }

    public async Task<List<GroceryItem>> GetGroceryItemsByName(string groceryName)
    {
        var groceryItems =
            await _groceryItemCollection.FindAsync(g => g.Name.Contains(groceryName));
        return groceryItems.ToList();
    }

    public async Task<GroceryItem> CreateGroceryItem(GroceryItem groceryItem)
    {
        await _groceryItemCollection.InsertOneAsync(groceryItem);
        return await GetGroceryItemById(groceryItem.Id);
    }

    public async Task<GroceryItem> GetGroceryItemById(string groceryId)
    {
        var result = (await _groceryItemCollection.FindAsync(p => p.Id == groceryId)).ToList();
        return result.FirstOrDefault();
    }

    public async Task<GroceryItem> UpdateGroceryItem(GroceryItem groceryItem)
    {
        var groceryItemToUpdate = await GetGroceryItemById(groceryItem.Id);

        groceryItem.PriceHistory = UpdatePriceHistory(groceryItem, groceryItemToUpdate);
        
        await _groceryItemCollection.UpdateOneAsync(
            g => g.Id == groceryItem.Id,
            Builders<GroceryItem>.Update
                .Set(n => n.Name, groceryItem.Name)
                .Set(n => n.Price, groceryItem.Price)
                .Set(n => n.BrandName, groceryItem.BrandName)
                .Set(n => n.PurchaseDate, groceryItem.PurchaseDate)
                .Set(n => n.GroceryCategory, groceryItem.GroceryCategory)
                .Set(n => n.GroceryImageUrl, groceryItem.GroceryImageUrl)
                .Set(n => n.GroceryStoreName, groceryItem.GroceryStoreName)
                .Set(n => n.PriceHistory, groceryItem.PriceHistory),
            new UpdateOptions { IsUpsert = false });

        return groceryItem;
    }

    public async Task DeleteGroceryItem(string groceryId)
    {
        await _groceryItemCollection.DeleteOneAsync(groceryItem => groceryItem.Id == groceryId);
    }

    public async Task CreateGroceryItemBatch(List<GroceryItem> groceryItems)
    {
        await _groceryItemCollection.InsertManyAsync(groceryItems);
    }

    private static List<PriceLog>? UpdatePriceHistory(GroceryItem groceryItem, GroceryItem groceryItemToUpdate)
    {
        groceryItem.PriceHistory = groceryItemToUpdate.PriceHistory;

        if (groceryItem.PurchaseDate != groceryItemToUpdate.PurchaseDate)
        {
            var newPriceLog = new PriceLog() { Price = groceryItem.Price, LogDate = groceryItem.PurchaseDate };

            if (groceryItem.PriceHistory.FirstOrDefault().Price == 0.0)
                groceryItem.PriceHistory = new List<PriceLog>() { newPriceLog };
            else
                groceryItem.PriceHistory.Add(newPriceLog);
        }

        return groceryItem.PriceHistory.OrderByDescending(pl => pl.LogDate).ToList();
    }

    public void Dispose()
    {
    }
}