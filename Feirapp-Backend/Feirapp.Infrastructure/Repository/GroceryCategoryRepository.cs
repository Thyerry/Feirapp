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
    public Task<List<GroceryCategory>> GetAllGroceryCategories()
    {
        throw new NotImplementedException();
    }

    public Task<GroceryCategory> GetGroceryCategoryById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<GroceryCategory> CreateGroceryCategory(GroceryCategory groceryCategory)
    {
        throw new NotImplementedException();
    }

    public Task UpdateGroceryCategory(GroceryCategory groceryCategory)
    {
        throw new NotImplementedException();
    }

    public Task DeleteGroceryCategory(string id)
    {
        throw new NotImplementedException();
    }
}