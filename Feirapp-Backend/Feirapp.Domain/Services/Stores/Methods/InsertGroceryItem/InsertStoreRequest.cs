namespace Feirapp.Domain.Services.Stores.Methods.InsertGroceryItem;

public record InsertStoreRequest()
{
    public string Name { get; set; }
    public List<string>? AltNames { get; set; }
    public string? Cnpj { get; set; }
    public string? Cep { get; set; }
    public string? Street { get; set; }
    public string? StreetNumber { get; set; }
    public string? Neighborhood { get; set; }
    public string? CityName { get; set; }
    public string? States { get; set; }
}
