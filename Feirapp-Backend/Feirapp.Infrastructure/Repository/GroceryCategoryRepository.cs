using Feirapp.Domain.Contracts.Repository;
using Feirapp.Entities;
using Feirapp.Infrastructure.DataContext;
using MongoDB.Driver;

namespace Feirapp.Infrastructure.Repository;

public class GroceryCategoryRepository : IGroceryCategoryRepository
{
    private readonly IMongoCollection<GroceryCategory> _collection;

    public GroceryCategoryRepository(IMongoFeirappContext context)
    {
        _collection = context.GetCollection<GroceryCategory>(nameof(GroceryCategory));
    }

    public async Task<List<GroceryCategory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return (await _collection.FindAsync(q => true,
            cancellationToken: cancellationToken))
            .ToList();
    }

    public async Task<GroceryCategory> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return (await _collection.FindAsync(q => q.Id == id,
            cancellationToken: cancellationToken))
            .FirstOrDefault();
    }

    public async Task<GroceryCategory> InsertAsync(GroceryCategory groceryCategory, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(groceryCategory, cancellationToken: cancellationToken);
        return groceryCategory;
    }

    public async Task<List<GroceryCategory>> InsertBatchAsync(List<GroceryCategory> groceryCategories, CancellationToken cancellationToken = default)
    {
        await _collection.InsertManyAsync(groceryCategories, cancellationToken: cancellationToken);
        return groceryCategories;
    }

    public async Task UpdateAsync(GroceryCategory groceryCategory, CancellationToken cancellationToken = default)
    {
        await _collection.ReplaceOneAsync(
            Builders<GroceryCategory>.Filter.Eq(g => g.Id, groceryCategory.Id),
            groceryCategory,
            cancellationToken: cancellationToken
            );
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        await _collection.DeleteOneAsync(q => q.Id == id, cancellationToken);
    }
}