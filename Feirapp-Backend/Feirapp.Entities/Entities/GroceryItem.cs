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
    public string Name { get; set; }

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
    public string? Barcode { get; set; }

    /// <summary>
    /// Gets or sets the price of the grocery item.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the date of the last update to the grocery item's information.
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets the date the grocery item was purchased.
    /// </summary>
    public DateTime PurchaseDate { get; set; }

    /// <summary>
    /// Gets or sets the measure unit of the grocery item.
    /// </summary>
    public MeasureUnitEnum MeasureUnit { get; set; }

    /// <summary>
    /// Gets or sets the NCM code of the grocery item.
    /// </summary>
    public string? NcmCode { get; set; }

    /// <summary>
    /// Gets or sets the NCM information of the grocery item.
    /// </summary>
    public Ncm Ncm { get; set; }

    /// <summary>
    /// Gets or sets the CEST code of the grocery item.
    /// </summary>
    public string? CestCode { get; set; }

    /// <summary>
    /// Gets or sets the CEST information of the grocery item.
    /// </summary>
    public Cest Cest { get; set; }

    /// <summary>
    /// Gets or sets the store identifier where the grocery item is available.
    /// </summary>
    public long StoreId { get; set; }

    /// <summary>
    /// Gets or sets the store where the grocery item is available.
    /// </summary>
    public Store Store { get; set; }

    /// <summary>
    /// Gets or sets the price history of the grocery item.
    /// </summary>
    public ICollection<PriceLog> PriceHistory { get; set; }

    /// <summary>
    /// Returns a string that represents the current grocery item.
    /// </summary>
    /// <returns>
    /// A string that represents the current grocery item.
    /// </returns>
    public override string ToString() => $"{Name}";
}