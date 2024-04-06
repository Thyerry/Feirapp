using MongoDB.Bson.Serialization.Attributes;

namespace Feirapp.DocumentModels.Documents;

public class GroceryCategory : BaseDocumentModel
{
    [BsonElement("name")] public string Name { get; set; } = string.Empty;
    [BsonElement("description")] public string Description { get; set; } = string.Empty;
    [BsonElement("cest")] public string Cest { get; set; } = string.Empty;
    [BsonElement("item")] public string ItemNumber { get; set; } = string.Empty;
    [BsonElement("ncm")] public string Ncm { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Cest} - {Name}";
    }
}