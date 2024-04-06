using Feirapp.Domain.Contracts.Repository;
using Feirapp.Domain.Contracts.Service;
using Feirapp.Domain.Dtos;
using Feirapp.Domain.Mappers;
using Feirapp.Domain.Validators.GroceryItemValidators;
using FluentValidation;
using System.Diagnostics;

namespace Feirapp.Domain.Services;

public class GroceryItemService : IGroceryItemService
{
    private readonly IGroceryItemRepository _groceryItemRepository;
    private readonly IGroceryCategoryRepository _groceryCategoryRepository;

    public GroceryItemService(IGroceryItemRepository groceryItemRepository,
        IGroceryCategoryRepository groceryCategoryRepository)
    {
        _groceryItemRepository = groceryItemRepository;
        _groceryCategoryRepository = groceryCategoryRepository;
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

        var category =
            (await _groceryCategoryRepository.SearchAsync(groceryItem.Category, cancellationToken))
            .FirstOrDefault()!;
        if (category != null)
            groceryItem.Category = category;
        else
            Debug.WriteLine(
                $"Category with cest = {groceryItemDto.Category.Cest}, ncm = {groceryItemDto.Category.Ncm} and {groceryItemDto.Category.ItemNumber}, not found in database!");

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