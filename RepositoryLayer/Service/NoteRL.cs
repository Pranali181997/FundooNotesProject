using CommonDatabaseLayer;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooNoteContex;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NoteRL : INoteRL
    {
        // Created The User Repository Layer Class To Implement IUserRL Methods
        // Reference Object For FundooContext And IConfiguration
        FundooContext fundoo;
        private readonly IConfiguration Toolsettings;

        //Created Constructor To Initialize Fundoocontext For Each Instance
        public NoteRL(FundooContext fundoo, IConfiguration Toolsettings)
        {
            this.fundoo = fundoo;
            this.Toolsettings = Toolsettings;
        }
        public async Task AddNote(NotePostModel notePostModel, int userId)
        {
            // throw new NotImplementedException();
            try
            {
                var user = fundoo.Users.FirstOrDefault(u => u.UserId == userId);
                Note note = new Note
                {
                    User = user
                };
                note.Title = notePostModel.Title;
                note.Description = notePostModel.Description;
                note.BGColor = notePostModel.BGColor;
                note.IsArchive = false;
                note.IsReminder = false;
                note.IsPin = false;
                note.IsTrash = false;
                note.ModifiedDate = DateTime.Now;

                fundoo.Add(note);
                await fundoo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
