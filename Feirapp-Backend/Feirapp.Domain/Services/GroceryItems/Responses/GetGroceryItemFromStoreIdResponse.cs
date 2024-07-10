using Feirapp.Domain.Services.GroceryItems.Dtos;

namespace Feirapp.Domain.Services.GroceryItems.Responses;

public record GetGroceryItemFromStoreIdResponse(StoreDto Store, List<GroceryItemDto> Items);