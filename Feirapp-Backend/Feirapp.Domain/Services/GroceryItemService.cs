using Feirapp.Domain.Contracts.Repository;
using Feirapp.Domain.Contracts.Service;
using Feirapp.Domain.Mappers;
using Feirapp.Domain.Models;
using Feirapp.Domain.Validators.GroceryItemValidators;
using FluentValidation;

namespace Feirapp.Domain.Services;

public class GroceryItemService : IGroceryItemService
{
    private readonly IGroceryItemRepository _groceryItemRepository;

    public GroceryItemService(IGroceryItemRepository groceryItemRepository)
    {
        _groceryItemRepository = groceryItemRepository;
    }

    public async Task<List<GroceryItemModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        return (await _groceryItemRepository.GetAllAsync(cancellationToken)).ToModelList();
    }

    public async Task<List<GroceryItemModel>> GetRandomGroceryItemsAsync(int quantity, CancellationToken cancellationToken)
    {
        return (await _groceryItemRepository.GetRandomGroceryItems(quantity, cancellationToken)).ToModelList();
    }

    public async Task<GroceryItemModel> InsertAsync(GroceryItemModel groceryItem, CancellationToken cancellationToken)
    {
        var validator = new CreateGroceryItemValidator();
        var validationResult = await validator.ValidateAsync(groceryItem, cancellationToken);
        if (validationResult.Errors.Count > 0)
            throw new ValidationException(validationResult.Errors);

        return (await _groceryItemRepository.InsertAsync(groceryItem.ToEntity(), cancellationToken)).ToModel();
    }

    public async Task<GroceryItemModel> GetById(string groceryId, CancellationToken cancellationToken)
    {
        return (await _groceryItemRepository.GetByIdAsync(groceryId, cancellationToken)).ToModel();
    }

    public async Task UpdateAsync(GroceryItemModel groceryItem, CancellationToken cancellationToken)
    {
        var validator = new UpdateGroceryItemValidator();
        var validationResult = await validator.ValidateAsync(groceryItem, cancellationToken);
        if (validationResult.Errors.Count > 0)
            throw new ValidationException(validationResult.Errors);

        await _groceryItemRepository.UpdateAsync(groceryItem.ToEntity(), cancellationToken);
    }

    public async Task DeleteAsync(string groceryId, CancellationToken cancellationToken)
    {
        await _groceryItemRepository.DeleteAsync(groceryId, cancellationToken);
    }
}