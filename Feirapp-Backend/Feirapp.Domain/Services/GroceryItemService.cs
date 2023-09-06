using Feirapp.Domain.Contracts.Repository;
using Feirapp.Domain.Contracts.Service;
using Feirapp.Domain.Mappers;
using Feirapp.Domain.Models;
using Feirapp.Domain.Validators.GroceryItemValidators;
using Feirapp.Entities;
using FluentValidation;

namespace Feirapp.Domain.Services;

public class GroceryItemService : IGroceryItemService
{
    private readonly IGroceryItemRepository _repository;

    public GroceryItemService(IGroceryItemRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<List<GroceryItemModel>> GetAllGroceryItems()
    {
        return (await _repository.GetAllAsync()).ToModelList();
    }

    public async Task<List<GroceryItemModel>> GetRandomGroceryItems(int quantity)
    {
        return (await _repository.GetRandomGroceryItems(quantity)).ToModelList();
    }

    public async Task<GroceryItemModel> CreateGroceryItem(GroceryItemModel groceryItem)
    {
        var validator = new CreateGroceryItemValidator();
        var validationResult = await validator.ValidateAsync(groceryItem);
        if (validationResult.Errors.Count > 0)
            throw new ValidationException(validationResult.Errors);

        return (await _repository.InsertAsync(groceryItem.ToEntity())).ToModel();
    }

    public async Task<GroceryItemModel> GetGroceryItemById(string groceryId)
    {
        return (await _repository.GetByIdAsync(groceryId)).ToModel();
    }

    public async Task UpdateGroceryItem(GroceryItemModel groceryItem)
    {
        var validator = new UpdateGroceryItemValidator();
        var validationResult = await validator.ValidateAsync(groceryItem);
        if (validationResult.Errors.Count > 0)
            throw new ValidationException(validationResult.Errors);

        await _repository.UpdateAsync(groceryItem.ToEntity());
    }

    public async Task DeleteGroceryItem(string groceryId)
    {
        await _repository.DeleteAsync(groceryId);
    }
}