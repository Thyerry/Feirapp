using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.Stores.Methods.SearchStores;

public record SearchStoresResponse(Guid Id, string Name, string? Cnpj, List<string>? AltNames, string? Cep, string? Street, string? StreetNumber, string? Neighborhood, string? CityName, StatesEnum? State);
