using FluentValidation;
using FluentValidation.Results;

namespace Feirapp.Domain.Services.GroceryItems.Dtos.Command;

public record InsertListOfGroceryItems(List<GroceryItemDto> GroceryItems, StoreDto Store, bool IsManualInsert = false)
{
    public void Validate()
    {
        var errors = new List<ValidationFailure>();
        if (GroceryItems.Count == 0) 
            errors.Add(new ValidationFailure(nameof(GroceryItems), "No Grocery Items Found", GroceryItems));

        var storeValidation = Store.ValidateForInsertGroceryItem();
        var groceryValidation = GroceryItems
            .Select(g => g.ValidationForImportation())
            .ToList();
        
        if (storeValidation.Count > 1)
            errors.AddRange(storeValidation);
        
        if (groceryValidation.Count > 1)
            errors.AddRange(groceryValidation.SelectMany(x => x));
            
        if (errors.Count > 0)
        {
            throw new ValidationException("Insert failed", errors);
        }
    }
}