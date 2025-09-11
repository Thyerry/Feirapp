namespace Feirapp.Domain.Services.Stores.Methods.InsertGroceryItem;

public record InsertStoreRequest(
    string Name,
    List<string>? AltNames,
    string? Cnpj,
    string? Cep,
    string? Street,
    string? StreetNumber,
    string? Neighborhood,
    string? CityName,
    string? State
);