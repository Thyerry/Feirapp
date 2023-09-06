using MongoDB.Bson.Serialization.Attributes;

namespace Feirapp.Entities;

public class GroceryCategory : BaseEntity
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("cest")]
    public string CEST { get; set; }

    [BsonElement("item")]
    public string ItemNumber { get; set; }

    [BsonElement("ncm")]
    public string NCM { get; set; }
}