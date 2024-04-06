﻿using MongoDB.Bson.Serialization.Attributes;

namespace Feirapp.DocumentModels.Documents;

public class PriceLog
{
    [BsonElement("price")] public decimal Price { get; set; }
    [BsonElement("logDate")] public DateTime LogDate { get; set; }
}