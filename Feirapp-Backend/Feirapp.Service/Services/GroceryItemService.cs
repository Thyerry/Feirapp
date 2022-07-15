using Feirapp.Domain.Contracts;
using Feirapp.Domain.Models;
using Feirapp.Service.Validators.GroceryItemValidators;
using FluentValidation;

namespace Feirapp.Service.Services;

public class GroceryItemService : IGroceryItemService
{
    private readonly IGroceryItemRepository _repository;

    public GroceryItemService(IGroceryItemRepository repository)
    {
        // TODO: Throw ArgumentException when IGroceryItemRepository is null
        _repository = repository;
    }

    public async Task<List<GroceryItem>> GetAllGroceryItems()
    {
        return await _repository.GetAllGroceryItems();
    }

    public async Task<List<GroceryItem>> GetGroceryItemByName(string groceryName)
    {
        return await _repository.GetGroceryItemsByName(groceryName.ToUpper());
    }

    public async Task<GroceryItem> CreateGroceryItem(GroceryItem groceryItem)
    {
        var validator = new CreateGroceryItemValidator();
        var isValid = await validator.ValidateAsync(groceryItem);
        if (isValid.Errors.Count > 0)
            throw new ValidationException(isValid.Errors);
        return await _repository.CreateGroceryItem(FormatTextFields(groceryItem));
    }

    public async Task<GroceryItem> GetGroceryItemById(string groceryId)
    {
        return await _repository.GetGroceryItemById(groceryId);
    }

    public async Task<GroceryItem> UpdateGroceryItem(GroceryItem groceryItem)
    {        
        // TODO: Validate the groceryItem fields here before calling the repository
        return await _repository.UpdateGroceryItem(groceryItem);
    }

    public async Task DeleteGroceryItem(string groceryId)
    {
        await _repository.DeleteGroceryItem(groceryId);
    }

    private GroceryItem FormatTextFields(GroceryItem groceryItem)
    {
        return new GroceryItem()
        {
            Name = groceryItem.Name!.ToUpper(),
            Price = groceryItem.Price,
            BrandName = groceryItem.BrandName!.ToUpper(),
            GroceryCategory = groceryItem.GroceryCategory,
            PurchaseDate = groceryItem.PurchaseDate,
            GroceryStoreName = groceryItem.GroceryStoreName!.ToUpper()
        };
    }
}