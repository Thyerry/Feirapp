using Feirapp.Entities.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Feirapp.Entities;

public class GroceryItem : BaseEntity
{
    [BsonElement("Name")]
    public string? Name { get; set; }

    [BsonElement("Price")]
    public decimal? Price { get; set; }

    [BsonElement("Category")]
    public GroceryCategory? Category { get; set; }

    [BsonElement("Brand")]
    public string? Brand { get; set; }

    [BsonElement("GroceryStore")]
    public string? GroceryStore { get; set; }

    [BsonElement("PurchaseDate")]
    public DateTime? PurchaseDate { get; set; }

    [BsonElement("MeasureUnit")]
    public MeasureUnitEnum? MeasureUnit { get; set; }

    [BsonElement("ImageUrl")]
    public string? ImageUrl { get; set; }

    [BsonElement("PriceHistory")]
    public List<PriceLog>? PriceHistory { get; set; }
}