using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Post
{
    public class PostDTO : BaseDTO
    {

        public string? Title { get; set; }
        public string? Description { get; set; }

    }
}
