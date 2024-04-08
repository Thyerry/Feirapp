using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Dtos.Commands;

public record InsertGroceryItemCommand
(
    string Name,
    decimal Price,
    string? Barcode = null,
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
    States StoreState = States.PE,
    string? Category = null
)
{
    public InsertGroceryItemCommand() : this("Empty Grocery Item", 0)
    {
    }
}