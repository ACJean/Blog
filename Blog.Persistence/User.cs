using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Persistence
{
    public class User : Base
    {

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("mail")]
        public string Mail { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

    }

    public class UserReference
    {
        [BsonElement("uuid")]
        public string Uuid { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

    }
}
