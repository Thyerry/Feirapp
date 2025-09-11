using System.ComponentModel.DataAnnotations.Schema;
using Feirapp.Entities.Enums;

namespace Feirapp.Entities.Entities;

[Table("stores")]
public class Store
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    [Column("alt_names")]
    public List<string>? AltNames { get; set; }
    [Column("cnpj")]
    public string? Cnpj { get; set; }
    [Column("cep")]
    public string? Cep { get; set; }
    [Column("street")]
    public string? Street { get; set; }
    [Column("street_number")]
    public string? StreetNumber { get; set; }
    [Column("neighborhood")]
    public string? Neighborhood { get; set; }
    [Column("city_name")]
    public string? CityName { get; set; }
    [Column("state")]
    public StatesEnum? State { get; set; }
}