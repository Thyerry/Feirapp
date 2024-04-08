using Feirapp.Domain.Services.GroceryItems.Dtos.Commands;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemService
{
    Task<List<GetGroceryItemResponse>> GetAllAsync(CancellationToken ct = default);

    //Task<List<GroceryItemDto>> GetRandomGroceryItemsAsync(int quantity, CancellationToken cancellationToken = default);

    //Task<GroceryItemDto> GetById(long groceryId, CancellationToken cancellationToken = default);

    //Task<GroceryItemDto> InsertAsync(GroceryItemDto groceryItemDto, CancellationToken cancellationToken = default);

    Task<List<InsertGroceryItemResponse>> InsertBatchAsync(List<InsertGroceryItemCommand> insertCommands, CancellationToken ct = default);

    //Task UpdateAsync(GroceryItemDto groceryItemDto, CancellationToken cancellationToken = default);

    //Task DeleteAsync(long groceryId, CancellationToken cancellationToken = default);
}