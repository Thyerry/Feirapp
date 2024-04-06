using Feirapp.DocumentModels.Documents;
using Feirapp.Domain.Contracts.Repository;
using Feirapp.Infrastructure.DataContext;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
    
namespace Feirapp.Infrastructure.Repository;

public class GroceryCategoryRepository : IGroceryCategoryRepository
{
    private readonly IMongoCollection<GroceryCategory> _collection;

    public GroceryCategoryRepository(IMongoFeirappContext context)
    {
        _collection = context.GetCollection<GroceryCategory>(nameof(GroceryCategory));

        FieldDefinition<GroceryCategory> cestField = "cest";

        var indexedKeyDefinition = Builders<GroceryCategory>.IndexKeys.Ascending(cestField);
        _collection.Indexes.CreateOne(new CreateIndexModel<GroceryCategory>(indexedKeyDefinition));
    }

    public async Task<List<GroceryCategory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return (await _collection.FindAsync(q => true, cancellationToken: cancellationToken))
            .ToList();
    }

    public async Task<GroceryCategory> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return (await _collection.FindAsync(q => q.Id == id,
                cancellationToken: cancellationToken))
            .FirstOrDefault();
    }

    public async Task<GroceryCategory> InsertAsync(GroceryCategory groceryCategory,
        CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(groceryCategory, cancellationToken: cancellationToken);
        return groceryCategory;
    }

    public async Task<List<GroceryCategory>> InsertBatchAsync(List<GroceryCategory> groceryCategories,
        CancellationToken cancellationToken = default)
    {
        await _collection.InsertManyAsync(groceryCategories, cancellationToken: cancellationToken);
        return groceryCategories;
    }

    public async Task UpdateAsync(GroceryCategory groceryCategory, CancellationToken cancellationToken = default)
    {
        await _collection.ReplaceOneAsync(
            gc => gc.Id == groceryCategory.Id,
            groceryCategory,
            cancellationToken: cancellationToken
        );
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        await _collection.DeleteOneAsync(q => q.Id == id, cancellationToken);
    }

    public async Task<List<GroceryCategory>> SearchAsync(GroceryCategory groceryCategory,
        CancellationToken cancellationToken = default)
    {
        var query = _collection.AsQueryable();

        if (!string.IsNullOrWhiteSpace(groceryCategory.Name))
            return await query.Where(gc => gc.Name == groceryCategory.Name).ToListAsync(CancellationToken.None);

        if (!string.IsNullOrWhiteSpace(groceryCategory.Cest))
        {
            query = query.Where(gc => gc.Cest == groceryCategory.Cest);

            if (!string.IsNullOrWhiteSpace(groceryCategory.ItemNumber))
                query = query.Where(gc => gc.ItemNumber == groceryCategory.ItemNumber);
        }

        if (!string.IsNullOrWhiteSpace(groceryCategory.Ncm))
            query = query.Where(gc => gc.Ncm == groceryCategory.Ncm);

        var result = await query.ToListAsync(cancellationToken);
        return result;
    }
}