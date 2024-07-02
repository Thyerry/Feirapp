using Feirapp.Entities.Entities;
using FluentValidation;

namespace Feirapp.Domain.Services.GroceryItems.Dtos.Command;

public record InsertGroceryItemCommand(GroceryItemDto groceryItem, StoreDto? store)
{
    public void Validate()
    {
        var errors = groceryItem.ValidationForImportation();
        if (store != null)
        {
            var storeValidation = store.ValidateForInsertGroceryItem();
            if (storeValidation.Count > 0)
                errors.AddRange(storeValidation);
        }

        if (errors.Count > 0)
        {
            throw new ValidationException("Insert failed", errors);
        }
    }
};