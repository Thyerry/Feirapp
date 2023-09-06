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

    public async Task<List<GroceryItemModel>> GetAllGroceryItems()
    {
        return (await _groceryItemRepository.GetAllAsync()).ToModelList();
    }

    public async Task<List<GroceryItemModel>> GetRandomGroceryItems(int quantity)
    {
        return (await _groceryItemRepository.GetRandomGroceryItems(quantity)).ToModelList();
    }

    public async Task<GroceryItemModel> CreateGroceryItem(GroceryItemModel groceryItem)
    {
        var validator = new CreateGroceryItemValidator();
        var validationResult = await validator.ValidateAsync(groceryItem);
        if (validationResult.Errors.Count > 0)
            throw new ValidationException(validationResult.Errors);

        return (await _groceryItemRepository.InsertAsync(groceryItem.ToEntity())).ToModel();
    }

    public async Task<GroceryItemModel> GetGroceryItemById(string groceryId)
    {
        return (await _groceryItemRepository.GetByIdAsync(groceryId)).ToModel();
    }

    public async Task UpdateGroceryItem(GroceryItemModel groceryItem)
    {
        var validator = new UpdateGroceryItemValidator();
        var validationResult = await validator.ValidateAsync(groceryItem);
        if (validationResult.Errors.Count > 0)
            throw new ValidationException(validationResult.Errors);

        await _groceryItemRepository.UpdateAsync(groceryItem.ToEntity());
    }

    public async Task DeleteGroceryItem(string groceryId)
    {
        await _groceryItemRepository.DeleteAsync(groceryId);
    }
}