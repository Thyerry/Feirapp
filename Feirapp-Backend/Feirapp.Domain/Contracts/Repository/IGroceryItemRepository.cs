using Feirapp.Entities;

namespace Feirapp.Domain.Contracts.Repository;

public interface IGroceryItemRepository
{
    Task<GroceryItem> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<List<GroceryItem>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<GroceryItem>> GetRandomGroceryItems(int quantity, CancellationToken cancellationToken = default);

    Task<GroceryItem> InsertAsync(GroceryItem groceryItem, CancellationToken cancellationToken = default);

    Task<List<GroceryItem>> InsertGroceryItemBatch(List<GroceryItem> groceryItems, CancellationToken cancellationToken = default);

    Task UpdateAsync(GroceryItem groceryItem, CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}