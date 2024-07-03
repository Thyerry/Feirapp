using Feirapp.Entities.Entities;
using FluentValidation;

namespace Feirapp.Domain.Services.GroceryItems.Dtos.Command;

public record InsertGroceryItemCommand(GroceryItemDto groceryItem, StoreDto? store);