using FluentValidation;
using FluentValidation.Results;

namespace Feirapp.Domain.Services.GroceryItems.Dtos.Command;

public record InsertListOfGroceryItems(List<GroceryItemDto> GroceryItems, StoreDto Store, bool IsManualInsert = false);