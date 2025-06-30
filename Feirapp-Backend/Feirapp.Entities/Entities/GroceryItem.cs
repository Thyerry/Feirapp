using Feirapp.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Feirapp.Entities.Entities;

[Table("grocery_items")]
public class GroceryItem
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    [Column("brand")]
    public string? Brand { get; set; }
    [Column("description")]
    public string? Description { get; set; }
    [Column("image_url")]
    public string? ImageUrl { get; set; }
    [Column("barcode")]
    public string Barcode { get; set; } = "SEM GTIN";
    [Column("measure_unit")]
    public MeasureUnitEnum MeasureUnit { get; set; }
    [Column("ncm_code")]
    public string? NcmCode { get; set; }
    public Ncm Ncm { get; set; }
    [Column("cest_code")]
    public string? CestCode { get; set; }
    public Cest Cest { get; set; }

    [Column("alt_names")]
    public List<string>? AltNames { get; set; }

    public ICollection<PriceLog>? PriceHistory { get; set; }

    public override string ToString() => $"{Name}";
    
}