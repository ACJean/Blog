using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Blog.Persistence
{
    public class Base
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Uuid { get; set; }

        [BsonElement("state")]
        public int State { get; set; }

        [BsonElement("creation_date")]
        public DateTime CreationDate { get; set; }

        [BsonElement("creation_user")]
        public UserReference CreationUser { get; set; }

        [BsonElement("modification_date")]
        public DateTime ModificationDate { get; set; }

        [BsonElement("modification_user")]
        public UserReference ModificationUser { get; set; }
    }
}
