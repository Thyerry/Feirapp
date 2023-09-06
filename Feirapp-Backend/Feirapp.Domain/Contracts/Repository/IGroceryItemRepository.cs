using Feirapp.Entities;

namespace Feirapp.Domain.Contracts.Repository;

public interface IGroceryItemRepository
{
    Task<GroceryItem> GetByIdAsync(string id);

    Task<List<GroceryItem>> GetAllAsync();

    Task<List<GroceryItem>> GetRandomGroceryItems(int quantity);

    Task<GroceryItem> InsertAsync(GroceryItem groceryItem);

    Task<List<GroceryItem>> InsertGroceryItemBatch(List<GroceryItem> groceryItems);

    Task UpdateAsync(GroceryItem groceryItem);

    Task DeleteAsync(string id);
}