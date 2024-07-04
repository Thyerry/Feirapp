namespace Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

public record GetGroceryItemFromStoreIdResponse(StoreDto Store, List<GroceryItemDto> Items);