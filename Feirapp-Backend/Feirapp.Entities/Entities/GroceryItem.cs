using Feirapp.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Feirapp.Entities.Entities;

[Table("GroceryItems")]
public class GroceryItem
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Brand { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? Barcode { get; set; }
    public decimal Price { get; set; }
    public DateTime LastUpdate { get; set; }
    public DateTime PurchaseDate { get; set; }
    public MeasureUnitEnum MeasureUnit { get; set; }
    public string NcmCode { get; set; }
    public Ncm Ncm { get; set; }
    public string CestCode { get; set; }
    public Cest Cest { get; set; }
    public long StoreId { get; set; }
    public Store Store { get; set; }
    public ICollection<PriceLog> PriceHistory { get; set; }

    public override string ToString() => $"{Name}";
}