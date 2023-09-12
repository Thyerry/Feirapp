using Feirapp.Domain.Contracts.Repository;
using Feirapp.Domain.Contracts.Service;
using Feirapp.Domain.Dtos;
using Feirapp.Domain.Mappers;
using Feirapp.Domain.Validators;
using FluentValidation;

namespace Feirapp.Domain.Services;

public class GroceryItemService : IGroceryItemService
{
    private readonly IGroceryItemRepository _groceryItemRepository;
    private readonly IGroceryCategoryRepository _groceryCategoryRepository;

    public GroceryItemService(IGroceryItemRepository groceryItemRepository, IGroceryCategoryRepository groceryCategoryRepository)
    {
        _groceryItemRepository = groceryItemRepository;
        _groceryCategoryRepository = groceryCategoryRepository;
    }

    public async Task<List<GroceryItemDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return (await _groceryItemRepository.GetAllAsync(cancellationToken)).ToDtoList();
    }

    public async Task<List<GroceryItemDto>> GetRandomGroceryItemsAsync(int quantity, CancellationToken cancellationToken)
    {
        return (await _groceryItemRepository.GetRandomGroceryItems(quantity, cancellationToken)).ToDtoList();
    }

    public async Task<GroceryItemDto> InsertAsync(GroceryItemDto groceryItemDto, CancellationToken cancellationToken)
    {
        var validator = new InsertGroceryItemValidator();
        await validator.ValidateAndThrowAsync(groceryItemDto, cancellationToken);

        var groceryCategory =
            (await _groceryCategoryRepository.SearchAsync(groceryItemDto.Category.ToModel(), cancellationToken))
            .FirstOrDefault();

        return (await _groceryItemRepository.InsertAsync(groceryItemDto.ToModel(), cancellationToken)).ToDto();
    }

    public async Task<GroceryItemDto> GetById(string groceryId, CancellationToken cancellationToken)
    {
        return (await _groceryItemRepository.GetByIdAsync(groceryId, cancellationToken)).ToDto();
    }

    public async Task UpdateAsync(GroceryItemDto groceryItem, CancellationToken cancellationToken)
    {
        var validator = new UpdateGroceryItemValidator();
        await validator.ValidateAndThrowAsync(groceryItem, cancellationToken);

        await _groceryItemRepository.UpdateAsync(groceryItem.ToModel(), cancellationToken);
    }

    public async Task DeleteAsync(string groceryId, CancellationToken cancellationToken)
    {
        await _groceryItemRepository.DeleteAsync(groceryId, cancellationToken);
    }
}