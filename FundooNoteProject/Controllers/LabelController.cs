using BussinessLayer.Interface;
using CommonDatabaseLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooNoteContex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FundooNoteProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        ILabelBL labelBL;
        FundooContext fundoo;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private string keyName = "Pranali";
        public LabelController(ILabelBL lableBL, FundooContext fundoo, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.labelBL = lableBL;
            this.fundoo = fundoo;
            this.memoryCache=memoryCache;
            this.distributedCache = distributedCache;
        }
        [Authorize]
        [HttpPost("AddNote")]
        public async Task<ActionResult> AddLable(LablePostModel lablePostModel, int UserId, int NoteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);

                await this.labelBL.AddLable(lablePostModel, userId, NoteId);
                return this.Ok(new { success = true, message = "Lable Added Successfully!!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //HTTP method to handle get label request
        //[Authorize]
        //[HttpGet("Getlabel")]
        //public async Task<ActionResult> GetLabelByNoteId(int NoteId)
        //{
        //    try
        //    {
        //        var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
        //        int userId = Int32.Parse(userid.Value);
        //        List<Label> list = new List<Label>();
        //         list = await this.labelBL.GetAllLabelsByNoteId(NoteId, userId);
        //        if (list == null)
        //        {
        //            return this.BadRequest(new { success = true, message = "Failed to get label" });
        //        }
        //        return this.Ok(new { success = true, message = $"Label get successfully", data = list });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        [Authorize]
        [HttpPut("UpdateLable/{lableId}")]
        public async Task<ActionResult> UpdateLable(int userId, int lableId, LablePostModel lablePostModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);
                var res = await this.labelBL.UpdateLable(UserId, lableId, lablePostModel);
                if (res != null)
                    return this.Ok(new { success = true, message = "Lable Updated successfully!!!" });
                else
                    return this.BadRequest(new { success = false, message = "Failed to update lable or Id does not exists" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //HTTP method to handle delete label request
        [Authorize]
        [HttpDelete("DeleteLabel/{LabelId}")]
        public async Task<ActionResult> DeleteLabel(int LabelId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                await this.labelBL.DeleteLabel(LabelId, userId);
                return this.Ok(new { success = true, message = $"Label Deleted successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //HTTP method to handle get label request
        [Authorize]
        [HttpGet("Getlabel")]
        public async Task<ActionResult> GetLabel()
        {
            try
            {           
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                List<Label> list = new List<Label>();
                list = await this.labelBL.Getlabel(userId);
                if (list == null)
                {
                    return this.BadRequest(new { success = false, message = "Failed to get label" });
                }
                return this.Ok(new { success = true, message = $"Label get successfully", data = list});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //HTTP method to handle get label request
        [Authorize]
        [HttpGet("GetlabelByNoteId/{NoteId}")]
        public async Task<ActionResult> GetLabelByNoteId(int NoteId)
        {
            try
            {                
                var list = await this.labelBL.Getlabel(NoteId);
                if (list == null)
                {
                    return this.BadRequest(new { success = true, message = "Failed to get label" });
                }
                return this.Ok(new { success = true, message = $"Label get successfully", data = list});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("GetlabelByRedisCache")]
        public async Task<ActionResult> GetlabelByRedisCache()
        {
            try
            {
                string serializeLabelList;
                var labelList = new List<RepositoryLayer.Entity.Label>();
                var redisLabelList = await distributedCache.GetAsync(keyName);
                if (redisLabelList != null)
                {
                    serializeLabelList = Encoding.UTF8.GetString(redisLabelList);
                    labelList = JsonConvert.DeserializeObject<List<RepositoryLayer.Entity.Label>>(serializeLabelList);
                }
                else
                {
                    labelList = await this.labelBL.GetlabelByRedisCache();
                    serializeLabelList = JsonConvert.SerializeObject(labelList);
                    redisLabelList = Encoding.UTF8.GetBytes(serializeLabelList);
                    var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(keyName, redisLabelList, options);
                }
                var result = this.Ok(new { success = true, message = $"Get note successful!!!", data = labelList });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
