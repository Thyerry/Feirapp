namespace Feirapp.Domain.Services.GroceryItems.Command;

public record InsertStore(
    string Cep,
    string CityName,
    string Cnpj,
    string Name,
    string Neighborhood,
    string State,
    string Street,
    string StreetNumber,
    List<string>? AltNames);