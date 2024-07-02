using System.ComponentModel.DataAnnotations.Schema;

namespace Feirapp.Entities.Entities;

[Table("PriceLogs")]
public class PriceLog
{
    /// <summary>
    /// Gets or sets the unique identifier for the price log.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Gets or sets the price of the grocery item.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the date of the log.
    /// </summary>
    public DateTime LogDate { get; set; }

    /// <summary>
    /// Gets or sets the grocery item's ID.
    /// </summary>
    public long GroceryItemId { get; set; }
    
    /// <summary>
    /// Gets or sets the grocery item barcode.
    /// </summary>
    public string Barcode { get; set; }

    /// <summary>
    /// Gets or sets the grocery item.
    /// </summary>
    public GroceryItem GroceryItem { get; set; }

    /// <summary>
    /// Gets or sets the store's ID.
    /// </summary>
    public long StoreId;

    /// <summary>
    /// Gets or sets the store.
    /// </summary>
    public Store Store { get; set; }
}
