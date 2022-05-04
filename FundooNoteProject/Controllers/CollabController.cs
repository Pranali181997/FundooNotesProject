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
    public class CollabController : ControllerBase
    {
        ICollabBL collabBL;
        FundooContext fundoo;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly string key = "Pranali";
        public CollabController(ICollabBL collabBL, FundooContext fundoo, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.collabBL = collabBL;
            this.fundoo = fundoo;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }
        [Authorize]
        [HttpPost("AddCollaborator")]
        public async Task<ActionResult> AddCollab(int NoteId, CollabPostModel collabPostModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);

                var Id = fundoo.Note.Where(x => x.NoteId == NoteId && x.UserId == UserId).FirstOrDefault();
                if (Id == null)
                {
                    return this.BadRequest(new { success = false, message = $"Note doesn't exists" });
                }
                var result = await this.collabBL.AddCollab(UserId, NoteId, collabPostModel);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = $"Collaborator added successfully", data = result });
                }
                return this.BadRequest(new { success = false, message = $"Failed to add collaborator", data = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //HTTP method to handle delete collaborator request
        [Authorize]
        [HttpDelete("RemoveCollaborator/{NoteId}")]
        public async Task<ActionResult> RemoveCollaborator(int NoteId, int collaboratorId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var re = fundoo.Collabs.Where(x => x.userId == userId && x.NoteId == NoteId).FirstOrDefault();
                if (re == null)
                {
                    return this.BadRequest(new { success = false, message = $"Note doesn't exists" });
                }
                bool result = await this.collabBL.RemoveCollaborator(userId, NoteId, collaboratorId);
                if (result == true)
                {
                    return this.Ok(new { success = true, message = $"Collaborator removed successfully" });
                }
                return this.BadRequest(new { success = false, message = $"Failed to remove collaborator" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //HTTP method to handle get collaborator request
        [Authorize]
        [HttpGet("GetCollaboratorByUserId")]
        public async Task<ActionResult> GetCollaboratorByUserId()
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var Id = fundoo.Collabs.Where(x => x.userId == userId).FirstOrDefault();
                if (Id == null)
                {
                    return this.BadRequest(new { success = false, message = $"User doesn't exists" });
                }
                List<Collab> result = await this.collabBL.GetCollaboratorByUserId(userId);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = $"Collaborator got successfully", data = result });
                }
                return this.BadRequest(new { success = false, message = $"Failed to get collaborator" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //HTTP method to handle get collaborator request
        [Authorize]
        [HttpGet("GetCollaboratorByNoteId/{NoteId}")]
        public async Task<ActionResult> GetCollaboratorByNoteId(int NoteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var Id = fundoo.Collabs.FirstOrDefault(x => x.NoteId == NoteId && x.userId == userId);
                if (Id == null)
                {
                    return this.BadRequest(new { success = false, message = $"Note doesn't exists" });
                }
                List<Collab> result = await this.collabBL.GetCollaboratorByUserId(userId);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = $"Collaborator got successfully", data = result });
                }
                return this.BadRequest(new { success = false, message = $"Failed to get collaborator" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("GetAllNotes_ByRadisCache")]
        public async Task<ActionResult> GetCollaboratorByRedisCache()
        {
            try
            {
                string serializeCollabList;
                var collabList = new List<Collab>();
                var redisCollabList = await distributedCache.GetAsync(key);
                if (redisCollabList != null)
                {
                    serializeCollabList = Encoding.UTF8.GetString(redisCollabList);
                    collabList = JsonConvert.DeserializeObject<List<Collab>>(serializeCollabList);
                }
                else
                {
                    collabList = await this.collabBL.GetCollaboratorByRedisCache();
                    serializeCollabList = JsonConvert.SerializeObject(collabList);
                    redisCollabList = Encoding.UTF8.GetBytes(serializeCollabList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(key, redisCollabList, options);
                }
                return  this.Ok(new { success = true, message = "Get note successful!!!", data = collabList });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}