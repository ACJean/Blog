using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Security
{
    public class UserDTO : BaseDTO
    {

        public string Name { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }

    }

    public class UserReferenceDTO
    {

        public string Uuid { get; set; }
        public string Name { get; set; }

    }
}
