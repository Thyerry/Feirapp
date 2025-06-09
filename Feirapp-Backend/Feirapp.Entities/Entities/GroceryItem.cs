using Feirapp.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Feirapp.Entities.Entities;

/// <summary>
/// Represents a grocery item in the store.
/// </summary>
[Table("GroceryItems")]
public class GroceryItem
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string Barcode { get; set; } = "SEM GTIN";
    public MeasureUnitEnum MeasureUnit { get; set; }
    public string? NcmCode { get; set; }
    public Ncm Ncm { get; set; }
    public string? CestCode { get; set; }
    public Cest Cest { get; set; }
    
    public string? AltNames { get; set; }

    public ICollection<PriceLog>? PriceHistory { get; set; }

    public override string ToString() => $"{Name}";
    
}