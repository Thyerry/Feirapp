namespace Feirapp.Domain.Services.DataScrapper.Dtos;

public record InvoiceStore
(
    string Name,
    string Cnpj,
    string Cep,
    string Street,
    string StreetNumber,
    string Neighborhood,
    string CityName,
    string State
);