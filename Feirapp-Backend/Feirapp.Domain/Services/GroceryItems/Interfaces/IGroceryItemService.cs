using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Command;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

namespace Feirapp.Domain.Services.GroceryItems.Interfaces;

public interface IGroceryItemService
{
    Task<List<GetAllGroceryItemsResponse>> GetAllAsync(CancellationToken ct);
}