using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

public record InsertGroceryItemResponse
(
    long Id,
    string Name,
    decimal Price,
    string Barcode = null,
    string? Description = null,
    string? ImageUrl = null,
    string? NcmCode = null,
    string? CestCode = null,
    string? StoreName = null,
    string? StoreCnpj = null,
    string? StoreCep = null,
    string? StoreStreet = null,
    string? StoreStreetNumber = null,
    string? StoreNeighborhood = null,
    string? StoreCityName = null,
    StatesEnum StoreStateEnum = StatesEnum.PE,
    string? Category = null
)
{
    public InsertGroceryItemResponse() : this(0, "Empty Grocery Item", 0)
    {
    }
}