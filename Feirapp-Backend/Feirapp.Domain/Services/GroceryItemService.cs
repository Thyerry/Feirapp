using Feirapp.Domain.Contracts.Repository;
using Feirapp.Domain.Contracts.Service;
using Feirapp.Domain.Mappers;
using Feirapp.Domain.Models;
using Feirapp.Domain.Validators.GroceryItemValidators;
using Feirapp.Entities;
using FluentValidation;

namespace Feirapp.Service.Services;

public class GroceryItemService : IGroceryItemService
{
    private readonly IGroceryItemRepository _repository;

    public GroceryItemService(IGroceryItemRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<List<GroceryItemModel>> GetAllGroceryItems()
    { 
        return (await _repository.GetAllGroceryItems()).ToModelList();
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
        groceryItem.PriceHistory = new List<PriceLogModel>
        {
            new()
            {
                Price = groceryItem.Price,
                LogDate = groceryItem.PurchaseDate
            }
        };
        return (await _repository.CreateGroceryItem(groceryItem.ToEntity())).ToModel();
    }

    public async Task<GroceryItemModel> GetGroceryItemById(string groceryId)
    {
        return (await _repository.GetGroceryItemById(groceryId)).ToModel();
    }

    public async Task<GroceryItemModel> UpdateGroceryItem(GroceryItemModel groceryItem)
    {
        var validator = new UpdateGroceryItemValidator();
        var validationResult = await validator.ValidateAsync(groceryItem);
        if (validationResult.Errors.Count > 0)
            throw new ValidationException(validationResult.Errors);

        return (await _repository.UpdateGroceryItem(FormatTextFields(groceryItem.ToEntity()))).ToModel();
    }

    public async Task DeleteGroceryItem(string groceryId)
    {
        await _repository.DeleteGroceryItem(groceryId);
    }

    private static GroceryItem FormatTextFields(GroceryItem groceryItem)
    {
        return new GroceryItem()
        {
            Id = groceryItem.Id ?? null,
            Name = groceryItem.Name!.ToUpper(),
            Price = groceryItem.Price,
            Brand = groceryItem.Brand!.ToUpper(),
            Category = groceryItem.Category,
            MeasureUnit = groceryItem.MeasureUnit,
            ImageUrl = groceryItem.ImageUrl,
            PurchaseDate = groceryItem.PurchaseDate,
            GroceryStore = groceryItem.GroceryStore!.ToUpper(),
            PriceHistory = groceryItem.PriceHistory
        };
    }
}