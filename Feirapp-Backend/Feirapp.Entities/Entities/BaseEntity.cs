using MongoDB.Bson.Serialization.Attributes;

namespace Feirapp.Entities;

public class BaseEntity
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string? Id { get; set; }
}