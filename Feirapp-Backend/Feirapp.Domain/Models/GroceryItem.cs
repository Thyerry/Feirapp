using Feirapp.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Feirapp.Domain.Models;

public class GroceryItem
{
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double? Price { get; set; }
    public ProductSectionEnum ProductSection { get; set; }
    public string? BrandName { get; set; }
    public string GroceryStoreName { get; set; }
    public DateTime? PurchaseDate { get; set; }
}