using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Mappers;
using Feirapp.Domain.Services.GroceryItems.Validators;
using FluentValidation;

namespace Feirapp.Domain.Services.GroceryItems.Implementations;

public class GroceryItemService : IGroceryItemService
{
    private readonly IGroceryItemRepository _groceryItemRepository;

    public GroceryItemService(IGroceryItemRepository groceryItemRepository)
    {
        _groceryItemRepository = groceryItemRepository;
    }

    public async Task<List<GroceryItemDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return (await _groceryItemRepository.GetAllAsync(cancellationToken)).ToDtoList();
    }

    public async Task<List<GroceryItemDto>> GetRandomGroceryItemsAsync(int quantity,
        CancellationToken cancellationToken)
    {
        return (await _groceryItemRepository.GetRandomGroceryItems(quantity, cancellationToken)).ToDtoList();
    }

    public async Task<GroceryItemDto> InsertAsync(GroceryItemDto groceryItemDto, CancellationToken cancellationToken)
    {
        var validator = new InsertGroceryItemValidator();
        await validator.ValidateAndThrowAsync(groceryItemDto, cancellationToken);

        var groceryItem = groceryItemDto.ToModel();

        groceryItem.Creation = DateTime.UtcNow;
        groceryItem.LastUpdate = DateTime.UtcNow;

        return (await _groceryItemRepository.InsertAsync(groceryItem, cancellationToken)).ToDto();
    }

    public async Task<List<GroceryItemDto>> InsertBatchAsync(List<GroceryItemDto> groceryItemDtos,
        CancellationToken cancellationToken = default)
    {
        var validator = new InsertGroceryItemValidator();
        foreach (var item in groceryItemDtos)
            await validator.ValidateAndThrowAsync(item, cancellationToken);

        var groceryItems = groceryItemDtos.ToModelList().Select(g =>
        {
            g.Creation = DateTime.UtcNow;
            g.LastUpdate = DateTime.UtcNow;
            return g;
        }).ToList();

        return (await _groceryItemRepository.InsertBatchAsync(groceryItems, cancellationToken)).ToDtoList();
    }

    public async Task<GroceryItemDto> GetById(string groceryId, CancellationToken cancellationToken)
    {
        return (await _groceryItemRepository.GetByIdAsync(groceryId, cancellationToken)).ToDto();
    }

    public async Task UpdateAsync(GroceryItemDto groceryItemDto, CancellationToken cancellationToken)
    {
        var validator = new UpdateGroceryItemValidator();
        await validator.ValidateAndThrowAsync(groceryItemDto, cancellationToken);

        var groceryItem = groceryItemDto.ToModel();
        groceryItem.LastUpdate = DateTime.UtcNow;

        await _groceryItemRepository.UpdateAsync(groceryItem, cancellationToken);
    }

    public async Task DeleteAsync(string groceryId, CancellationToken cancellationToken)
    {
        await _groceryItemRepository.DeleteAsync(groceryId, cancellationToken);
    }
}