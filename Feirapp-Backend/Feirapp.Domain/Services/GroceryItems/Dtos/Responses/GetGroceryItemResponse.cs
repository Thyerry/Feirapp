namespace Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

public record GetGroceryItemResponse(StoreResponse Store, List<GroceryItemResponse> Items);