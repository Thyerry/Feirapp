using Feirapp.Entities;

namespace Feirapp.Domain.Contracts.Repository;

public interface IGroceryCategoryRepository
{
    Task<List<GroceryCategory>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<GroceryCategory> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<GroceryCategory> InsertAsync(GroceryCategory groceryCategory, CancellationToken cancellationToken = default);

    Task UpdateAsync(GroceryCategory groceryCategory, CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}