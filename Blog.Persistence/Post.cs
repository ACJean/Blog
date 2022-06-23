using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Persistence
{
    public class Post : Base
    {

        [BsonElement("name")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

    }
}
