using Feirapp.Domain.Services.GroceryItems.Dtos.Commands;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemService
{
    Task<List<GetGroceryItemResponse>> GetAllAsync(CancellationToken ct = default);
    Task InsertBatchAsync(InsertGroceryItemCommand insertCommand, CancellationToken ct = default);
}