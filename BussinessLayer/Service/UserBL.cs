using BussinessLayer.Interface;
using CommonDatabaseLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Service
{
    public class UserBL : IUserBL
    {
        IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }
        public void AddUser(UserPostModel user)
        {
            try
            {
                this.userRL.AddUser(user);
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

                return userRL.LoginUser(Email, Password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }   
    }
}