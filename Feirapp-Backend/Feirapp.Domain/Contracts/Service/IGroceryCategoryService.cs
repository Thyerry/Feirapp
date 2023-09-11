using Feirapp.Domain.Dtos;

namespace Feirapp.Domain.Contracts.Service;

public interface IGroceryCategoryService
{
    Task<GroceryCategoryDto> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<List<GroceryCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<GroceryCategoryDto> InsertAsync(GroceryCategoryDto groceryCategory, CancellationToken cancellationToken = default);

    Task UpdateAsync(GroceryCategoryDto groceryCategory, CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}