using Feirapp.Domain.Enums;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Feirapp.Domain.Models;

public class GroceryItem
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("Name")]
    public string Name { get; set; }
    public double? Price { get; set; }
    public GroceryCategoryEnum GroceryCategory { get; set; }
    public string? BrandName { get; set; }
    public string GroceryStoreName { get; set; }
    public DateTime? PurchaseDate { get; set; }
}