using Feirapp.Domain.Contracts.Repository;
using Feirapp.Entities;
using Feirapp.Infrastructure.DataContext;
using MongoDB.Driver;

namespace Feirapp.Infrastructure.Repository;

public class GroceryItemRepository : IGroceryItemRepository, IDisposable
{
    private readonly IMongoCollection<GroceryItem> _collection;

    public GroceryItemRepository(IMongoFeirappContext context)
    {
        _collection = context.GetCollection<GroceryItem>(nameof(GroceryItem));
    }

    public async Task<List<GroceryItem>> GetAllAsync()
    {
        var result = await _collection.FindAsync(q => true);
        return result.ToList();
    }

    public async Task<List<GroceryItem>> GetRandomGroceryItems(int quantity)
    {
        var result = await _collection.AggregateAsync(PipelineDefinition<GroceryItem, GroceryItem>.Create($@"
        {{
            $sample: {{ size: {quantity} }}
        }}
        "));
        return result.ToList();
    }

    public async Task<GroceryItem> InsertAsync(GroceryItem groceryItem)
    {
        await _collection.InsertOneAsync(groceryItem);
        return groceryItem;
    }

    public async Task<GroceryItem> GetByIdAsync(string groceryId)
    {
        var result = (await _collection.FindAsync(p => p.Id == groceryId)).ToList();
        return result.FirstOrDefault();
    }

    public async Task UpdateAsync(GroceryItem groceryItem)
    {
        var groceryItemToUpdate = await GetByIdAsync(groceryItem.Id);
        await _collection.UpdateOneAsync(
            g => g.Id == groceryItem.Id,
            Builders<GroceryItem>.Update
                .Set(n => n.Name, groceryItem.Name)
                .Set(n => n.Price, groceryItem.Price)
                .Set(n => n.Brand, groceryItem.Brand)
                .Set(n => n.PurchaseDate, groceryItem.PurchaseDate)
                .Set(n => n.MeasureUnit, groceryItem.MeasureUnit)
                .Set(n => n.ImageUrl, groceryItem.ImageUrl)
                .Set(n => n.GroceryStore, groceryItem.GroceryStore),
            new UpdateOptions { IsUpsert = false });
    }

    public async Task DeleteAsync(string groceryId)
    {
        await _collection.DeleteOneAsync(groceryItem => groceryItem.Id == groceryId);
    }

    public async Task<List<GroceryItem>> InsertGroceryItemBatch(List<GroceryItem> groceryItems)
    {
        await _collection.InsertManyAsync(groceryItems);
        return groceryItems;
    }

    public void Dispose()
    {
    }
}