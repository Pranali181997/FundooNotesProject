using BussinessLayer.Interface;
using CommonDatabaseLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooNoteContex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooNoteProject.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class NoteController : ControllerBase
    {
        INoteBL noteBL;
        FundooContext fundoo;
        public NoteController(INoteBL noteBL, FundooContext fundoo)
        {
            this.noteBL = noteBL;
            this.fundoo = fundoo;
        }
        [Authorize]
        [HttpPost("AddNote")]
        public async Task<ActionResult> AddNote(NotePostModel notePostModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);

                await this.noteBL.AddNote(notePostModel, userId);
                return this.Ok(new { success = true, message = "Note Added Successfully!!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpGet("Getnote/{noteId}")]
        public async Task<ActionResult> GetNote(int noteId, int userId)
        {
            try
            {
                var result = await this.noteBL.GetNote(noteId, userId);
                return this.Ok(new { success = true, message = $"Below are the Note data", data = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }  
        [Authorize]
        [HttpGet("GetAllNotes")]
        public async Task<ActionResult> GetAllNotes()
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                List<Note> result = await noteBL.GetAllNote(userId);
                return this.Ok(new { success = true, message = $"Below are all notes", data = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPut("UpdateNote/{noteId}")]
        public async Task<IActionResult> UpdateNote(NotePostModel notePostModel, int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var result = await this.noteBL.UpdateNote(notePostModel, noteId, userId);
                return this.Ok(new { success = true, message = $"Note updated successfully!!!", data = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpDelete("DeleteNote/{noteId}")]
        public async Task<ActionResult> DeleteNote(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                await this.noteBL.DeleteNote(noteId, userId);
                return this.Ok(new { success = true, message = "Note deleted successfully!!!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut("ArchieveNote/{noteId}")]
        public async Task<ActionResult> IsArchieveNote(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var res = await this.noteBL.ArchieveNote(noteId, userId);
                if (res != null)
                    return this.Ok(new { success = true, message = "Note Archieved successfully!!!" });
                else
                    return this.BadRequest(new { success = false, message = "Failed to archieve note or Id does not exists" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPut("IsPinned/{noteId}")]
        public async Task<ActionResult> IsPinned(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var res = await this.noteBL.PinNote(noteId, userId);
                if (res != null)
                    return this.Ok(new { success = true, message = "Note pinned successfully!!!" });
                else
                    return this.BadRequest(new { success = false, message = "Failed to pin note or Id does not exists" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPut("IsTrash{noteId}")]
        public async Task<ActionResult> IsTrash(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var res = await this.noteBL.TrashNote(noteId, userId);
                if (res != null)
                    return this.Ok(new { success = true, message = "Note trashed successfully!!!" });
                else
                    return this.BadRequest(new { success = false, message = "Failed to trash note or Id does not exists" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPut("ChangeColorNote/{noteId}")]
        public async Task<ActionResult> ChangeColorNote(int noteId, string color)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var res = await this.noteBL.ChangeColor(noteId, userId, color);
                if (res != null)
                    return this.Ok(new { success = true, message = "Note color changed successfully!!!" });
                else
                    return this.BadRequest(new { success = false, message = "Failed to change color note or Id does not exists" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}