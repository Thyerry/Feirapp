using MongoDB.Bson.Serialization.Attributes;

namespace Feirapp.Entities;

public class PriceLog : BaseEntity
{
    [BsonElement("GroceryItemId")]
    public string GroceryItemId { get; set; }
    [BsonElement("Price")]
    public decimal? Price { get; set; }
    [BsonElement("LogDate")]
    public DateTime? LogDate { get; set; }
}