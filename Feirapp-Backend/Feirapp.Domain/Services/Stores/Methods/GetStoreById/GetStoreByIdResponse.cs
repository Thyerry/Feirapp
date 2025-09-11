namespace Feirapp.Domain.Services.Stores.Methods.GetStoreById;

public class GetStoreByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<string>? AltNames { get; set; }
    public string? Cnpj { get; set; }
    public string? Cep { get; set; }
    public string? Street { get; set; }
    public string? StreetNumber { get; set; }
    public string? Neighborhood { get; set; }
    public string? CityName { get; set; }
    public string? State { get; set; }
}