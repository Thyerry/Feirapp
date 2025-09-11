namespace Feirapp.Domain.Services.GroceryItems.Misc;

public record InvoiceScanStore(
    string Cep,
    string CityName,
    string Cnpj,
    string Name,
    string Neighborhood,
    string State,
    string Street,
    string StreetNumber);