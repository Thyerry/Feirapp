using MongoDB.Bson.Serialization.Attributes;

namespace Feirapp.DocumentModels.Documents;

public class GroceryItem : BaseDocumentModel
{
    [BsonElement("name")] public string Name { get; set; } = string.Empty;
    [BsonElement("price")] public decimal Price { get; set; }
    [BsonElement("barcode")] public string Barcode { get; set; } = string.Empty;
    [BsonElement("brand")] public string Brand { get; set; } = string.Empty;
    [BsonElement("storeName")] public string StoreName { get; set; } = string.Empty;
    [BsonElement("purchaseDate")] public DateTime PurchaseDate { get; set; }
    [BsonElement("imageUrl")] public string ImageUrl { get; set; } = string.Empty;
    [BsonElement("ncm")] public string Ncm { get; set; } = string.Empty;
    [BsonElement("cest")] public string Cest { get; set; } = string.Empty;  
    [BsonElement("priceHistory")] public List<PriceLog> PriceHistory { get; set; } = new List<PriceLog>();

    public override string ToString()
    {
        return $"{Name} - {StoreName}";
    }
}