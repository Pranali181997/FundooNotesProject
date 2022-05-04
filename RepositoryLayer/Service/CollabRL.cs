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
        public class CollabRL : ICollabRL
        {
            FundooContext fundoo;
            public IConfiguration Configuration { get; }

            //Creating constructor for initialization
            public CollabRL(FundooContext fundoo, IConfiguration configuration)
            {
                this.fundoo= fundoo;
                this.Configuration = configuration;
            }
            public async Task<Collab> AddCollab(int userId, int NoteId, CollabPostModel collabPostModel)
            {
                try
                {
                    var user = fundoo.Users.FirstOrDefault(u => u.UserId == userId);
                    var note = fundoo.Note.FirstOrDefault(b => b.NoteId == NoteId);

                    Collab collab = new Collab
                    {
                        User = user,
                        Note=note
                    };
                    collab.CollabEmail = collabPostModel.Email;
                    fundoo.Collabs.Add(collab);
                    await fundoo.SaveChangesAsync();
                    return collab;                  
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        public async Task<bool> RemoveCollaborator(int userId, int NoteId, int collaboratorId)
        {
            try
            {
                var result = fundoo.Collabs.FirstOrDefault(u => u.userId == userId && u.NoteId == NoteId && u.CollabId == collaboratorId);
                if (result != null)
                {
                    fundoo.Collabs.Remove(result);
                    await fundoo.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<Collab>> GetCollaboratorByUserId(int userId)
        {
            try
            {
                List<Collab> result = await fundoo.Collabs.Where(u => u.userId == userId).Include(u => u.User).Include(u => u.Note).Include(u => u.User).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<Collab>> GetCollaboratorByNoteId(int userId, int NoteId)
        {
            try
            {
                List<Collab> result = await fundoo.Collabs.Where(u => u.userId == userId && u.NoteId == NoteId).Include(u => u.User).Include(U => U.Note).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Collab>> GetCollaboratorByRedisCache()
        {
            try
            {
                List<Collab> result = await fundoo.Collabs.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}