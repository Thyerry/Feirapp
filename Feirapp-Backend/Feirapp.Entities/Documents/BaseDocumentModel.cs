using MongoDB.Bson.Serialization.Attributes;

namespace Feirapp.DocumentModels;

public class BaseDocumentModel
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonDateTimeOptions]
    public DateTime Creation { get; set; }

    [BsonDateTimeOptions]
    public DateTime LastUpdate { get; set; }
}