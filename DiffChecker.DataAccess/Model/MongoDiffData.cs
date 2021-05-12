using DiffChecker.Domain.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DiffChecker.DataAccess.Model
{
    public class MongoDiffData : DiffData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}