using System.ComponentModel.DataAnnotations.Schema;

namespace Feirapp.Entities.Entities;

[Table("PriceLogs")]
public class PriceLog
{
    public long Id { get; set; }
    public decimal Price { get; set; }
    public DateTime LogDate { get; set; }
    public long GroceryItemId { get; set; }
    public string Barcode { get; set; }
    public string ProductCode { get; set; }
    public GroceryItem GroceryItem { get; set; }
    public long StoreId;
    public Store Store { get; set; }
    public long InvoiceId { get; set; }
    public Invoice Invoice { get; set; }
}
