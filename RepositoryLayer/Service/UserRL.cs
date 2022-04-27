using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Experimental.System.Messaging;
using RepositoryLayer.FundooNoteContex;
using CommonDatabaseLayer;

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
        public bool ForgetPassword(string Email)
        {
            try
            {
                var res = fundoo.Users.FirstOrDefault(x => x.Email == Email);
                if (res == null)
                    return false;

                //Add message queue
                MessageQueue queue;
                if (MessageQueue.Exists(@".\Private$\FundooQueue"))
                {
                    queue = new MessageQueue(@".\Private$\FundooQueue");
                }
                else
                {
                    queue = MessageQueue.Create(@".\Private$\FundooQueue");
                }
                Message message = new Message();
                message.Formatter = new BinaryMessageFormatter();
                message.Body = GetJWTToken(Email, res.UserId);
                message.Label = "Forget Password Email";
                queue.Send(message);
                Message msg = queue.Receive();
                msg.Formatter = new BinaryMessageFormatter();
                EmailServices.SendMail(Email, msg.Body.ToString());
                queue.ReceiveCompleted += new ReceiveCompletedEventHandler(msmqQueue_ReceiveCompleted);
                queue.BeginReceive();
                queue.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //GENERATE TOKEN WITH EMAIL
        public string GenerateToken(string email)
        {
            if (email == null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Email",email)
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
        private void msmqQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                MessageQueue queue = (MessageQueue)sender;
                Message msg = queue.EndReceive(e.AsyncResult);
                EmailServices.SendMail(e.Message.ToString(), GenerateToken(e.Message.ToString()));
                queue.BeginReceive();
            }
            catch (MessageQueueException ex)
            {
                if (ex.MessageQueueErrorCode ==
                    MessageQueueErrorCode.AccessDenied)
                {
                    Console.WriteLine("Access is denied. " +
                        "Queue might be a system queue.");
                }
                // Handle other sources of MessageQueueException.
            }
        }
        public bool ChangePassword(string email, string password, string confirmPassword)
        {
            try
            {
                if (password.Equals(confirmPassword))
                {
                    var user = fundoo.Users.Where(x => x.Email == email).FirstOrDefault();

                    user.Password = EncryptPassword(confirmPassword);

                    fundoo.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<User> GetAllUsers()
        {
            try
            {
                var result = fundoo.Users.ToList();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool DeleteUser(string email)
        {
            // throw new NotImplementedException();
            try
            {
                var result = fundoo.Users.Where(e => e.Email == email).FirstOrDefault();

                if (result != null)
                {
                    fundoo.Users.Remove(result);
                    fundoo.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}   

