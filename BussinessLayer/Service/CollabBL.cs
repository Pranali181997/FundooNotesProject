using BussinessLayer.Interface;
using CommonDatabaseLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public class CollabBL:ICollabBL
    {
            ICollabRL IcollabRL;
            public CollabBL(ICollabRL IcollabRL)
            {
                this.IcollabRL = IcollabRL;
            }
        public async Task<Collab> AddCollab(int userId, int NoteId, CollabPostModel collabPostModel)
        {
            try
            {
                return await this.IcollabRL.AddCollab(userId, NoteId, collabPostModel);
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
                return await this.IcollabRL.RemoveCollaborator(userId, NoteId, collaboratorId);
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
                return await this.IcollabRL.GetCollaboratorByUserId(userId);
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
                return await this.IcollabRL.GetCollaboratorByNoteId(userId, NoteId);
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
                return await this.IcollabRL.GetCollaboratorByRedisCache();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

