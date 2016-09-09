using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DroneSoftFramework.Database.Mongo.Entity
{
    public abstract class DroneEntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
    }
}
