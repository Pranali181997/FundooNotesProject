using BussinessLayer.Interface;
using CommonDatabaseLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooNoteContex;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace FundooNoteProject.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {
        //instance variable
        IUserBL userBL;
        FundooContext fundoo;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private string keyName = "Pranali";
        public UserController(IUserBL userBL, FundooContext fundoo,IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.userBL = userBL;
            this.fundoo = fundoo;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
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
        [HttpPost("ForgetPassword/{email}")]
        public IActionResult ForgetPassword(string email)
        {
            try
            {
                var result = this.userBL.ForgetPassword(email);
                if (result != false)
                {
                    return this.Ok(new
                    {
                        success = true,
                        message = $"Mail Sent Successfully " +
                        $" token:  {result}"
                    });

                }
                return this.BadRequest(new { success = false, message = $"mail not sent" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPut("ChangePassword/{confirmpassword}")]
        public IActionResult ChangePassword(string password, string confirmpassword)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var email = (currentUser.Claims.FirstOrDefault(c => c.Type == "Email").Value);

                bool res = userBL.ChangePassword(email, password, confirmpassword);

                if (!res)
                {
                    return this.BadRequest(new { success = false, message = "enter valid password" });

                }
                else
                {
                    return this.Ok(new { success = true, message = "reset password set successfully" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getallusers")]
        public ActionResult GetAllUsers()
        {
            try
            {
                var result = this.userBL.GetAllUsers();
                return this.Ok(new { success = true, message = $"Below are the User data", data = result });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [HttpDelete("Delete")]
        public ActionResult DeleteUser(string email)
        {
            try
            {
                if (userBL.DeleteUser(email))
                    return this.Ok(new { Success = true, message = "User deleted successful", data = userBL.DeleteUser(email) });
                else
                    return this.BadRequest(new { Success = false, message = "User not deleted " });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet("getallusers_Radis")]
        public ActionResult GetAllUsers_Redis()
        {
            try
            {
                string serializeUserList;
                var userList = new List<User>();
                var redisUserList = distributedCache.Get(keyName);
                if (redisUserList != null)
                {
                    serializeUserList = Encoding.UTF8.GetString(redisUserList);
                    userList = JsonConvert.DeserializeObject<List<User>>(serializeUserList);
                }
                else
                {
                    userList = this.userBL.GetAllUsers();
                    serializeUserList = JsonConvert.SerializeObject(userList);
                    redisUserList = Encoding.UTF8.GetBytes(serializeUserList);
                    var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                     distributedCache.Set(keyName, redisUserList, options);
                }
                return this.Ok(new { success = true, message = "Get note successful!!!", data = userList });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}