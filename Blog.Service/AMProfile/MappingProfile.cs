using AutoMapper;
using Blog.Persistence;
using Blog.Service.Post;
using Blog.Service.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.AMProfile
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            #region User
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();

            CreateMap<UserReferenceDTO, UserReference>();
            CreateMap<UserReference, UserReferenceDTO>();
            #endregion

            #region Post
            CreateMap<PostDTO, Persistence.Post>();
            CreateMap<Persistence.Post, PostDTO>();
            #endregion
        }

    }
}
