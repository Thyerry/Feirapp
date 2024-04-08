using Feirapp.Domain.Services.BaseRepository;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemRepository : IBaseRepository<GroceryItem>
{
    Task<List<GroceryItem>> GetRandomGroceryItems(int quantity, CancellationToken ct = default);

    Task<List<GroceryItem>> InsertBatchAsync(List<GroceryItem> groceryItems, CancellationToken ct = default);
}