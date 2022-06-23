using AutoMapper;
using Blog.Persistence;
using Blog.Service.Security;
using FluentValidation.Results;
using Lavel.STD.Entities.NoSQL;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Service.Post
{
    public class PostService
    {

        private readonly IMapper Mapper;
        private readonly INoSQLClient Db;
        private readonly UserService UserService;

        public PostService(IMapper mapper, INoSQLClient db, UserService userService)
        {
            Mapper = mapper;
            Db = db;
            UserService = userService;
        }

        public ServiceResponse<PostDTO> Get(string uuid)
        {
            try
            {
                var filter = Builders<Persistence.Post>.Filter.Where(user => user.Uuid.Equals(uuid));
                Persistence.Post post = Db.Get(filter).FirstOrDefault();

                if (post == null) return ServiceResponse<PostDTO>.Completed(false, "No se encontró la publicación");

                PostDTO postDTO = Mapper.Map<PostDTO>(post);

                return ServiceResponse<PostDTO>.Completed(true, null, postDTO);
            }
            catch (Exception ex)
            {
                return ServiceResponse<PostDTO>.Error(ex);
            }
        }

        public ServiceResponse<List<PostDTO>> GetAll()
        {
            try
            {
                var filter = Builders<Persistence.Post>.Filter.Where(post => true);
                var sort = Builders<Persistence.Post>.Sort.Descending(post => post.CreationDate);
                List<Persistence.Post> posts = Db.Get(filter, sort);

                if (posts == null || (posts != null && posts.Count == 0)) return ServiceResponse<List<PostDTO>>.Completed(false, "No hay publicaciones");

                List<PostDTO> postsDTO = Mapper.Map<List<Persistence.Post>, List<PostDTO>>(posts);

                return ServiceResponse<List<PostDTO>>.Completed(true, null, postsDTO);
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<PostDTO>>.Error(ex);
            }
        }

        public ServiceResponse<PostDTO> Create(PostDTO postDTO, string userUuid)
        {
            try
            {
                PostDTOValidator validator = new PostDTOValidator();
                ValidationResult validationResult = validator.Validate(postDTO);
                if (!validationResult.IsValid)
                {
                    string[] validationMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToArray();
                    return ServiceResponse<PostDTO>.Invalid(validationMessages);
                }

                UserDTO user = UserService.GetUser(userUuid);

                postDTO.Uuid = null;
                postDTO.State = 1;
                postDTO.CreationDate = DateTime.Now;
                postDTO.CreationUser = UserUtil.GetUserReference(user);
                postDTO.ModificationDate = DateTime.Now;
                postDTO.ModificationUser = UserUtil.GetUserReference(user);

                Persistence.Post post = Mapper.Map<Persistence.Post>(postDTO);

                post = Db.Insert(post);

                postDTO = Mapper.Map<PostDTO>(post);

                bool created = !string.IsNullOrEmpty(postDTO.Uuid);
                string message = created ? "Publicación creada" : "No se pudo crear la publicación";

                return ServiceResponse<PostDTO>.Completed(created, message, postDTO);
            }
            catch (Exception ex)
            {
                return ServiceResponse<PostDTO>.Error(ex);
            }
        }

        public ServiceResponse<PostDTO> Update(PostDTO postDTO, string userUuid)
        {
            try
            {
                PostDTOValidator validator = new PostDTOValidator();
                ValidationResult validationResult = validator.Validate(postDTO);
                if (!validationResult.IsValid)
                {
                    string[] validationMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToArray();
                    return ServiceResponse<PostDTO>.Invalid(validationMessages);
                }

                UserDTO user = UserService.GetUser(userUuid);

                postDTO.ModificationDate = DateTime.Now;
                postDTO.ModificationUser = UserUtil.GetUserReference(user);

                Persistence.Post post = Mapper.Map<Persistence.Post>(postDTO);

                var filter = Builders<Persistence.Post>.Filter.Where(post => post.Uuid.Equals(postDTO.Uuid));
                
                bool updated = Db.Update(filter, post);
                string message = updated ? "Publicación actualizada" : "No se pudo actualizar la publicación";

                return ServiceResponse<PostDTO>.Completed(updated, message, postDTO);
            }
            catch (Exception ex)
            {
                return ServiceResponse<PostDTO>.Error(ex);
            }
        }

        public ServiceResponse<PostDTO> Delete(string uuid)
        {
            try
            {
                var filter = Builders<Persistence.Post>.Filter.Where(post => post.Uuid.Equals(uuid));
                bool deleted = Db.Delete(filter);
                string message = deleted ? "Publicación eliminada" : "No se pudo eliminar la publicación";

                return ServiceResponse<PostDTO>.Completed(deleted, message);
            }
            catch (Exception ex)
            {
                return ServiceResponse<PostDTO>.Error(ex);
            }
        }

    }
}
