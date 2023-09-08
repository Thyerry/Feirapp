using Feirapp.Domain.Models;

namespace Feirapp.Domain.Contracts.Service;

public interface IGroceryCategoryService
{
    Task<GroceryCategoryModel> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<List<GroceryCategoryModel>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<GroceryCategoryModel> InsertAsync(GroceryCategoryModel groceryCategory, CancellationToken cancellationToken = default);

    Task UpdateAsync(GroceryCategoryModel groceryCategory, CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}