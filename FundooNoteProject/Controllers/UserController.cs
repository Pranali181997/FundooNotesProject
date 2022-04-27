using BussinessLayer.Interface;
using CommonDatabaseLayer;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.FundooNoteContex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooNoteProject.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {
        //instance variable
        IUserBL userBL;
        FundooContext fundoo;
        public UserController(IUserBL userBL, FundooContext fundoo)
        {
            this.userBL = userBL;
            this.fundoo = fundoo;
        }
        [HttpPost("Register")]
        public ActionResult RegisterUser(UserPostModel user)
        {
            try
            {
                var getUserData = fundoo.Users.FirstOrDefault(u => u.Email == user.Email);
                if (getUserData != null)
                {
                    return this.Ok(new { success = false, message = $"{user.Email} is Already Exists" });
                }
                this.userBL.AddUser(user);
                return this.Ok(new { success = true, message = $"Registration Successfull { user.Email}" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("Login/{Email}/{Password}")]
        public ActionResult LoginUser(string Email, string Password)
        {
            try
            {
                var result = this.userBL.LoginUser(Email, Password);
                if (result != null)
                {
                    return this.Ok(new
                    {
                        success = true,
                        message = $"Login Successful " +
                        $" token:  {result}"
                    });
                }
                return this.BadRequest(new { success = false, message = $"Login Failed" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}