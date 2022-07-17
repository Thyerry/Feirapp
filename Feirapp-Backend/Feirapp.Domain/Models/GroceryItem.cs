using System.Buffers.Text;
using Feirapp.Domain.Enums;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Feirapp.Domain.Models;

public class GroceryItem
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonElement("Name")]
    public string? Name { get; set; }
    [BsonElement("Price")]
    public double? Price { get; set; }
    [BsonElement("GroceryCategory")]
    public GroceryCategoryEnum GroceryCategory { get; set; }
    [BsonElement("BrandName")]
    public string? BrandName { get; set; }
    [BsonElement("GroceryStoreName")]
    public string? GroceryStoreName { get; set; }
    [BsonElement("PurchaseDate")]
    public DateTime? PurchaseDate { get; set; }
    [BsonElement("GroceryImageUrl")]
    public string? GroceryImageUrl { get; set; }
}