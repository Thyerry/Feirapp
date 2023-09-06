using Feirapp.Domain.Models;

namespace Feirapp.Domain.Contracts.Service;

public interface IGroceryItemService
{
    Task<List<GroceryItemModel>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<GroceryItemModel>> GetRandomGroceryItemsAsync(int quantity, CancellationToken cancellationToken = default);

    Task<GroceryItemModel> GetById(string groceryId, CancellationToken cancellationToken = default);

    Task<GroceryItemModel> InsertAsync(GroceryItemModel groceryItem, CancellationToken cancellationToken = default);

    Task UpdateAsync(GroceryItemModel groceryItem, CancellationToken cancellationToken = default);

    Task DeleteAsync(string groceryId, CancellationToken cancellationToken = default);
}