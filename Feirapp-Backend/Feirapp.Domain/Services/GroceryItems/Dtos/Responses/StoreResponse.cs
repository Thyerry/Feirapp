using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Dtos.Responses;

public record StoreResponse(
    string Name,
    string? Cnpj = null,
    string? Cep = null,
    string? Street = null,
    string? StreetNumber = null,
    string? Neighborhood = null,
    string? CityName = null,
    StatesEnum? State = StatesEnum.PE);
