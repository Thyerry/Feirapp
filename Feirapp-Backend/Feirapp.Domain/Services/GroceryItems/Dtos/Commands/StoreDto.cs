using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Dtos.Commands;

public record StoreDto
(
    string? StoreName = null,
    string? StoreCnpj = null,
    string? StoreCep = null,
    string? StoreStreet = null,
    string? StoreStreetNumber = null,
    string? StoreNeighborhood = null,
    string? StoreCityName = null,
    StatesEnum StoreStateEnum = StatesEnum.PE
);