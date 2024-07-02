using Feirapp.Domain.Mappers;
using Feirapp.Domain.Services.Cests.Interfaces;
using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Command;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.Ncms.Interfaces;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public sealed class GroceryItemService(
    IGroceryItemRepository groceryItemRepository,
    IStoreRepository storeRepository,
    INcmRepository ncmRepository,
    ICestRepository cestRepository)
    : IGroceryItemService
{
    public async Task<List<GetAllGroceryItemsResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = await groceryItemRepository.GetAllAsync(ct);
        return entities.MapToGetAllResponse();
    }
}