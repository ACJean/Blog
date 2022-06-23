using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Security
{
    public static class UserUtil
    {

        public static UserReferenceDTO GetUserReference(UserDTO userDTO)
        {
            return new UserReferenceDTO()
            {
                Uuid = userDTO.Uuid,
                Name = userDTO.Name,
            };
        }

    }
}
