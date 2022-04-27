﻿using BussinessLayer.Interface;
using CommonDatabaseLayer;
using Microsoft.AspNetCore.Authorization;
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
        }
}
