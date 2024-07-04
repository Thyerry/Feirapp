namespace Feirapp.Domain.Services.Stores.Dtos.Commands;

public record InsertStoreCommand(
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