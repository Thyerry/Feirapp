namespace Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

public record GetGroceryItemResponse(StoreDto Store, List<GroceryItemDto> Items);