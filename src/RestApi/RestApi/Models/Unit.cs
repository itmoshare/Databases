using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Linq;

namespace RestApi.Models
{
    public class Unit
    {
        [BsonId]
        public int Id { get; set; }

        [BsonExtraElements]
        public BsonDocument ExtraData { get; set; }
    }
}

