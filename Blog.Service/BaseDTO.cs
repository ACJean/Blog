using Blog.Service.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service
{
    public class BaseDTO
    {
        public string? Uuid { get; set; }
        public int? State { get; set; }
        public DateTime? CreationDate { get; set; }
        public UserReferenceDTO? CreationUser { get; set; }
        public DateTime? ModificationDate { get; set; }
        public UserReferenceDTO? ModificationUser { get; set; }

    }
}
