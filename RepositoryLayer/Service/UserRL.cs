using CommonDatabaseLayer;
using Experimental.System.Messaging;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.FundooNoteContex;
using RepositoryLayer.Interface;
using System;
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
    }
}