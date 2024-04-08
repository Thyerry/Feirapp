using Feirapp.Domain.Services.GroceryItems.Dtos.Commands;
using Feirapp.Domain.Services.GroceryItems.Dtos.Responses;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Mappers;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public class GroceryItemService : IGroceryItemService
{
    private readonly IGroceryItemRepository _groceryItemRepository;

    public GroceryItemService(IGroceryItemRepository groceryItemRepository)
    {
        _groceryItemRepository = groceryItemRepository;
    }

    public async Task<List<GetGroceryItemResponse>> GetAllAsync(CancellationToken ct)
    {
        var groceryItems = await _groceryItemRepository.GetAllAsync(ct);
        return groceryItems.MapToGetAllResponse();
    }

    public async Task<List<InsertGroceryItemResponse>> InsertBatchAsync(List<InsertGroceryItemCommand> insertCommands, CancellationToken ct = default)
    {
        var groceryItemsEntities = insertCommands.MapToEntity();
        var insertedGroceryItems = await _groceryItemRepository.InsertBatchAsync(groceryItemsEntities, ct);
        return insertedGroceryItems.MapToInsertResponse();
    }

    //public async Task<List<GroceryItemDto>> GetRandomGroceryItemsAsync(int quantity,
    //    CancellationToken ct)
    //{
    //    return (await _groceryItemRepository.GetRandomGroceryItems(quantity, ct)).ToDtoList();
    //}

    //public async Task<GroceryItemDto> InsertAsync(GroceryItemDto groceryItemDto, CancellationToken ct)
    //{
    //    var validator = new InsertGroceryItemValidator();
    //    await validator.ValidateAndThrowAsync(groceryItemDto, ct);

    //    var groceryItem = groceryItemDto.ToEntity();

    //    return (await _groceryItemRepository.InsertAsync(groceryItem, ct)).ToDto();
    //}

    //public async Task<GroceryItemDto> GetById(long groceryId, CancellationToken ct)
    //{
    //    return (await _groceryItemRepository.GetByIdAsync(groceryId, ct)).ToDto();
    //}

    //public async Task UpdateAsync(GroceryItemDto groceryItemDto, CancellationToken ct)
    //{
    //    var validator = new UpdateGroceryItemValidator();
    //    await validator.ValidateAndThrowAsync(groceryItemDto, ct);

    //    var groceryItem = groceryItemDto.ToEntity();

    //    await _groceryItemRepository.UpdateAsync(groceryItem, ct);
    //}

    //public async Task DeleteAsync(long groceryId, CancellationToken ct)
    //{
    //    await _groceryItemRepository.DeleteAsync(groceryId, ct);
    //}
}