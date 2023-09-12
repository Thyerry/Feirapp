using Feirapp.DocumentModels;

namespace Feirapp.Domain.Contracts.Repository;

public interface IGroceryCategoryRepository
{
    Task<List<GroceryCategory>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<GroceryCategory> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<GroceryCategory> InsertAsync(GroceryCategory groceryCategory, CancellationToken cancellationToken = default);

    Task<List<GroceryCategory>> InsertBatchAsync(List<GroceryCategory> groceryCategories, CancellationToken cancellationToken = default);

    Task UpdateAsync(GroceryCategory groceryCategory, CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);

    Task<List<GroceryCategory>> SearchAsync(GroceryCategory groceryCategory, CancellationToken cancellationToken = default);
}