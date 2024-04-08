using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

public record GetGroceryItemResponse(
    long Id,
    string Name,
    string Description,
    decimal Price,
    GetStoreResponse Store,
    string ImageUrl,
    string Barcode,
    DateTime LastUpdate,
    DateTime PurchaseDate,
    MeasureUnitEnum MeasureUnit,
    List<GetPriceLogResponse> PriceHistory,
    string? Category = null
)
{
    public GetGroceryItemResponse() : this(0, "Empty Grocery Item", "", 0, new GetStoreResponse(), "", "", DateTime.Now, DateTime.Now, MeasureUnitEnum.EMPTY, new List<GetPriceLogResponse>(), "")
    {
    }
}