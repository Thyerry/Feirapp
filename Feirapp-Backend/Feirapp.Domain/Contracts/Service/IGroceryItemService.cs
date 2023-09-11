using Feirapp.Domain.Dtos;

namespace Feirapp.Domain.Contracts.Service;

public interface IGroceryItemService
{
    Task<List<GroceryItemDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<GroceryItemDto>> GetRandomGroceryItemsAsync(int quantity, CancellationToken cancellationToken = default);

    Task<GroceryItemDto> GetById(string groceryId, CancellationToken cancellationToken = default);

    Task<GroceryItemDto> InsertAsync(GroceryItemDto groceryItemModel, CancellationToken cancellationToken = default);

    Task UpdateAsync(GroceryItemDto groceryItem, CancellationToken cancellationToken = default);

    Task DeleteAsync(string groceryId, CancellationToken cancellationToken = default);
}