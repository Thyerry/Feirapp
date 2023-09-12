using Feirapp.DocumentModels.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Feirapp.DocumentModels;

public class GroceryItem : BaseDocumentModel
{
    [BsonElement("name")] public string Name { get; set; } = string.Empty;
    [BsonElement("price")] public decimal Price { get; set; }
    [BsonElement("cean")] public string Cean { get; set; } = string.Empty;
    [BsonElement("brand")] public string Brand { get; set; } = string.Empty;
    [BsonElement("storeName")] public string StoreName { get; set; } = string.Empty;
    [BsonElement("purchaseDate")] public DateTime PurchaseDate { get; set; }
    [BsonElement("measureUnit")] public MeasureUnitEnum? MeasureUnit { get; set; } = new MeasureUnitEnum?();
    [BsonElement("imageUrl")] public string ImageUrl { get; set; } = string.Empty;
    [BsonElement("category")] public GroceryCategory Category { get; set; } = new GroceryCategory();
    [BsonElement("priceHistory")] public List<PriceLog> PriceHistory { get; set; } = new List<PriceLog>();

    public override string ToString()
    {
        return $"{Name}";
    }
}