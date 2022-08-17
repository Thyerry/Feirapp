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
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
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
        var validationResult = await validator.ValidateAsync(groceryItem);
        if (validationResult.Errors.Count > 0)
            throw new ValidationException(validationResult.Errors);
        groceryItem.PriceHistory = new List<PriceLog>
        {
            new()
            {
                Price = groceryItem.Price,
                LogDate = groceryItem.PurchaseDate
            }
        };
        return await _repository.CreateGroceryItem(FormatTextFields(groceryItem));
    }

    public async Task<GroceryItem> GetGroceryItemById(string groceryId)
    {
        return await _repository.GetGroceryItemById(groceryId);
    }

    public async Task<GroceryItem> UpdateGroceryItem(GroceryItem groceryItem)
    {
        var validator = new UpdateGroceryItemValidator();
        var validationResult = await validator.ValidateAsync(groceryItem);
        if (validationResult.Errors.Count > 0)
            throw new ValidationException(validationResult.Errors);

        return await _repository.UpdateGroceryItem(FormatTextFields(groceryItem));
    }

    public async Task DeleteGroceryItem(string groceryId)
    {
        await _repository.DeleteGroceryItem(groceryId);
    }

    private GroceryItem FormatTextFields(GroceryItem groceryItem)
    {
        return new GroceryItem()
        {
            Id = groceryItem.Id ?? null,
            Name = groceryItem.Name!.ToUpper(),
            Price = groceryItem.Price,
            BrandName = groceryItem.BrandName!.ToUpper(),
            GroceryCategory = groceryItem.GroceryCategory,
            GroceryImageUrl = groceryItem.GroceryImageUrl,
            PurchaseDate = groceryItem.PurchaseDate,
            GroceryStoreName = groceryItem.GroceryStoreName!.ToUpper(),
            PriceHistory = groceryItem.PriceHistory
        };
    }
}