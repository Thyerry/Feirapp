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

    public async Task<List<GroceryItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _collection.FindAsync(q => true, cancellationToken: cancellationToken);
        return result.ToList();
    }

    public async Task<List<GroceryItem>> GetRandomGroceryItems(int quantity, CancellationToken cancellationToken)
    {
        var result = await _collection.AggregateAsync(PipelineDefinition<GroceryItem, GroceryItem>.Create($@"
        {{
            $sample: {{ size: {quantity} }}
        }}
        "), cancellationToken: cancellationToken);
        return result.ToList();
    }

    public async Task<GroceryItem> InsertAsync(GroceryItem groceryItem, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(groceryItem, cancellationToken: cancellationToken);
        return groceryItem;
    }

    public async Task<GroceryItem> GetByIdAsync(string groceryId, CancellationToken cancellationToken)
    {
        var result = (await _collection.FindAsync(p => p.Id == groceryId, cancellationToken: cancellationToken)).ToList();
        return result.FirstOrDefault();
    }

    public async Task UpdateAsync(GroceryItem groceryItem, CancellationToken cancellationToken)
    {
        await _collection.ReplaceOneAsync(
            Builders<GroceryItem>.Filter.Eq(g => g.Id, groceryItem.Id),
            groceryItem,
            cancellationToken: cancellationToken
        );
    }

    public async Task DeleteAsync(string groceryId, CancellationToken cancellationToken)
    {
        await _collection.DeleteOneAsync(groceryItem => groceryItem.Id == groceryId, cancellationToken: cancellationToken);
    }

    public async Task<List<GroceryItem>> InsertBatchAsync(List<GroceryItem> groceryItems, CancellationToken cancellationToken)
    {
        await _collection.InsertManyAsync(groceryItems, cancellationToken: cancellationToken);
        return groceryItems;
    }

    public void Dispose()
    {
    }
}