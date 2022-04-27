using CommonDatabaseLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Interface
{
    public interface IUserBL
    {
        public void AddUser(UserPostModel user);
        public string LoginUser(string Email, string Password);
        public bool ForgetPassword(string Email);
        public bool ChangePassword(string email, string password, string confirmPassword);
    }
}
