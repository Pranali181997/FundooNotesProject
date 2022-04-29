using BussinessLayer.Interface;
using CommonDatabaseLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.FundooNoteContex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooNoteProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LableController : ControllerBase
    { 
        ILableBL lableBL;
        FundooContext fundoo;
        public LableController(ILableBL lableBL, FundooContext fundoo)
        {
        this.lableBL = lableBL;
        this.fundoo = fundoo;
        }
        [Authorize]
        [HttpPost("AddNote")]
        public async Task<ActionResult> AddLable(LablePostModel lablePostModel, int UserId, int NoteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);

                await this.lableBL.AddLable(lablePostModel, userId,NoteId);
                return this.Ok(new { success = true, message = "Lable Added Successfully!!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
