using Feirapp.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Feirapp.Entities.Entities;

/// <summary>
/// Represents a grocery item in the store.
/// </summary>
[Table("GroceryItems")]
public class GroceryItem
{
    /// <summary>
    /// Gets or sets the unique identifier for the grocery item.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the grocery item.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the brand of the grocery item.
    /// </summary>
    public string? Brand { get; set; }

    /// <summary>
    /// Gets or sets the description of the grocery item.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the URL of the grocery item's image.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the barcode of the grocery item.
    /// </summary>
    public string Barcode { get; set; } = "SEM GTIN";

    /// <summary>
    /// Gets or sets the measure unit of the grocery item.
    /// </summary>
    public MeasureUnitEnum MeasureUnit { get; set; }

    public string? NcmCode { get; set; }
    public Ncm Ncm { get; set; }
    public string? CestCode { get; set; }
    public Cest Cest { get; set; }

    /// <summary>
    /// Gets or sets the price history of the grocery item.
    /// </summary>
    public ICollection<PriceLog>? PriceHistory { get; set; }

    public override string ToString() => $"{Name}";
    
}