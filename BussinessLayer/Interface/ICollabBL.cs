using BussinessLayer.Service;
using CommonDatabaseLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Interface
{
   public interface ICollabBL
    {
        Task<Collab> AddCollab(int userId, int NoteId, CollabPostModel collabPostModel);
        Task<bool> RemoveCollaborator(int userId, int NoteId, int collaboratorId);
        Task<List<RepositoryLayer.Entity.Collab>> GetCollaboratorByUserId(int userId);
        Task<List<RepositoryLayer.Entity.Collab>> GetCollaboratorByNoteId(int userId, int NoteId);
    }
}
