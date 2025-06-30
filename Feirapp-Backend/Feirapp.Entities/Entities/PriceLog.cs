using System.ComponentModel.DataAnnotations.Schema;

namespace Feirapp.Entities.Entities;

[Table("price_logs")]
public class PriceLog
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("log_date")]
    public DateTime LogDate { get; set; }

    [Column("grocery_item_id")]
    public Guid GroceryItemId { get; set; }

    [Column("barcode")]
    public string Barcode { get; set; }

    [Column("product_code")]
    public string? ProductCode { get; set; }

    public GroceryItem GroceryItem { get; set; }

    [Column("store_id")]
    public Guid StoreId;

    public Store Store { get; set; }

    [Column("invoice_id")]
    public Guid? InvoiceId { get; set; }

    public Invoice Invoice { get; set; }
}
