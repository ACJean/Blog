using AutoMapper;
using Blog.Persistence;
using Blog.Util;
using Lavel.STD.Entities.NoSQL;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Blog.Service.Security
{
    public class UserService
    {

        private readonly IMapper Mapper;
        private readonly INoSQLClient Db;
        private readonly BlogConfig BlogConfig;

        public UserService(IMapper mapper, INoSQLClient db, IOptions<BlogConfig> blogConfig)
        {
            Mapper = mapper;
            Db = db;
            BlogConfig = blogConfig.Value;
        }

        public UserDTO GetUser(string uuid)
        {
            try
            {
                var filter = Builders<User>.Filter.Where(user => user.Uuid == uuid);
                List<User> result = Db.Get(filter);
                UserDTO user = Mapper.Map<UserDTO>(result.FirstOrDefault());
                if (user != null) user.Password = null;
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ServiceResponse<TokenDTO> Login(UserDTO userDTO)
        {
            try
            {
                #region Validación
                var filter = Builders<User>.Filter.Where(user => user.Name.Equals(userDTO.Name));
                User user = Db.Get(filter).FirstOrDefault();
                if (user == null) throw new Exception("El usuario no existe.");
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(userDTO.Password, user.Password);
                if (!isValidPassword) throw new Exception("Contraseña incorrecta.");

                userDTO = Mapper.Map<UserDTO>(user);
                userDTO.Password = null;
                #endregion

                #region Generación de Token
                TokenDTO token = GetJWT(userDTO);
                #endregion

                return ServiceResponse<TokenDTO>.Completed(true, null, token);
            }
            catch (Exception ex)
            {
                return ServiceResponse<TokenDTO>.Error(ex);
            }
        }

        public ServiceResponse<UserDTO> Signup(UserDTO userDto)
        {
            try
            {
                UserDTO userSystem = new UserDTO() { Name = "SYSTEM" };

                userDto.Uuid = null;
                userDto.State = 1;
                userDto.CreationDate = DateTime.Now;
                userDto.CreationUser = UserUtil.GetUserReference(userSystem);
                userDto.ModificationDate = DateTime.Now;
                userDto.ModificationUser = UserUtil.GetUserReference(userSystem);

                User user = Mapper.Map<User>(userDto);
                user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

                user = Db.Insert(user);
                user.Password = null;

                userDto = Mapper.Map<UserDTO>(user);

                return ServiceResponse<UserDTO>.Completed(!string.IsNullOrEmpty(user.Uuid), null, userDto);
            }
            catch (Exception ex)
            {
                return ServiceResponse<UserDTO>.Error(ex);
            }
        }

        public TokenDTO GetJWT(UserDTO user)
        {
            byte[] key = Encoding.UTF8.GetBytes(BlogConfig.Key);

            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Uuid),
            });
            var expires = DateTime.UtcNow.AddHours(3);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = expires,
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenDTO() { AccessToken = tokenHandler.WriteToken(token), Type = "Bearer" };
        }

    }
}
