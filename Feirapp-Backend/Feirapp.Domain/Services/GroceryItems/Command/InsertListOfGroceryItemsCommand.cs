using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos;

namespace Feirapp.Domain.Services.GroceryItems.Command;

public record InsertListOfGroceryItemsCommand(
    List<InsertGroceryItem> GroceryItems,
    StoreDto Store);