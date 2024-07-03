namespace Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

public record GetGroceryItemsFromInvoiceResponse(StoreDto Store, List<GroceryItemDto> Items);