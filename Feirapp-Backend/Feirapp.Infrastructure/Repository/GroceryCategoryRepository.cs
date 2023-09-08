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
        throw new NotImplementedException();
    }

    public async Task<GroceryCategory> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<GroceryCategory> InsertAsync(GroceryCategory groceryCategory, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(GroceryCategory groceryCategory, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}