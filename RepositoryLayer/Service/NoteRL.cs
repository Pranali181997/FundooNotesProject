using CommonDatabaseLayer;
using Microsoft.EntityFrameworkCore;
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
                note.RegisterdDate = DateTime.Now;

                fundoo.Add(note);
                await fundoo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Note> GetNote(int noteId, int userId)
        {
            try
            {
                return await fundoo.Note.Where(u => u.NoteId == noteId && u.UserId == userId)
                .Include(u => u.User).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteNote(int noteId, int userId)
        {
            try
            {
                Note res = fundoo.Note.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                fundoo.Note.Remove(res);
                await fundoo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Note>> GetAllNote( int userId)
        {
            try
            {
                return await fundoo.Note.Where(u => u.UserId == userId).Include(u => u.User).ToListAsync();               
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Note> UpdateNote(NotePostModel notePostModel, int noteId, int userId)
        {
            try
            {
                var res = fundoo.Note.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (res != null)
                {
                    res.Title = notePostModel.Title;
                    res.Description = notePostModel.Description;
                    res.BGColor = notePostModel.BGColor;
                    res.IsArchive = notePostModel.IsArchive;
                    res.IsReminder = notePostModel.IsReminder;
                    res.IsPin = notePostModel.IsPin;
                    res.IsTrash = notePostModel.IsTrash;
                    res.ModifiedDate = DateTime.Now;
                    await fundoo.SaveChangesAsync();

                    return await fundoo.Note.Where(a => a.NoteId == noteId).FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Note> ArchieveNote(int noteId, int userId)
        {
            try
            {
                var res = fundoo.Note.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (res != null)
                {
                    if (res.IsArchive == false)
                    {
                        res.IsArchive = true;
                    }
                    else
                    {
                        res.IsArchive = false;
                    }
                    await fundoo.SaveChangesAsync();
                    return await fundoo.Note.Where(a => a.NoteId == noteId).FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Note> PinNote(int noteId, int userId)
        {
            try
            {
                var res = fundoo.Note.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (res != null)
                {
                    if (res.IsPin == false)
                    {
                        res.IsPin = true;
                    }
                    if (res.IsPin == true)
                    {
                        res.IsPin = false;
                    }
                    await fundoo.SaveChangesAsync();
                    return await fundoo.Note.Where(a => a.NoteId == noteId).FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Note> TrashNote(int noteId, int userId)
        {
            try
            {
                var res = fundoo.Note.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (res != null)
                {
                    if (res.IsTrash == false)
                    {
                        res.IsTrash = true;
                    }
                    if (res.IsTrash == true)
                    {
                        res.IsTrash = false;
                    }
                    await fundoo.SaveChangesAsync();
                    return await fundoo.Note.Where(a => a.NoteId == noteId).FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Note> ChangeColor(int noteId, int userId, string newColor)
        {
            try
            {
                var res = fundoo.Note.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (res != null)
                {
                    res.BGColor = newColor;
                    await fundoo.SaveChangesAsync();
                    return await fundoo.Note.Where(a => a.NoteId == noteId).FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }
}