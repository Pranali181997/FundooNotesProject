using CommonDatabaseLayer;
using Experimental.System.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.FundooNoteContex;
using RepositoryLayer.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        FundooContext fundoo;
        public IConfiguration Configuration { get; }
        public UserRL(FundooContext fundoo, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.fundoo = fundoo;
        }
        public void AddUser(UserPostModel user)
        {
            try
            {
                Entity.User user1 = new Entity.User();
                user1.UserId = new Entity.User().UserId;
                user1.FirstName = user.FirstName;
                user1.LastName = user.LastName;
                user1.Email = user.Email;
                user1.Adress = user.Adress;
                user1.Password = EncryptPassword(user.Password);
                user1.RegisterdDate = DateTime.Now;
                fundoo.Users.Add(user1);
                fundoo.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string EncryptPassword(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    return null;
                }
                else
                {
                    byte[] b = Encoding.ASCII.GetBytes(password);
                    string encrypted = Convert.ToBase64String(b);
                    return encrypted;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string LoginUser(string Email, string Password)
        {
            try
            {
                string encrypt = EncryptPassword(Password);
                var result = fundoo.Users.Where(u => u.Email == Email && u.Password == encrypt).FirstOrDefault();
                if (result == null)
                {
                    return null;
                }
                return GetJWTToken(Email, result.UserId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Generate JWT Token
        public static string GetJWTToken(string Email, int UserId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Email", Email),
                    new Claim("UserId",UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
