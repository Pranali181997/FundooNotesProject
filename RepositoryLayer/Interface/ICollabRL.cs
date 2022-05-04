using CommonDatabaseLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
   public interface ICollabRL
    {
        Task<Collab> AddCollab(int userId, int NoteId, CollabPostModel collabPostModel);
        Task<bool> RemoveCollaborator(int userId, int NoteId, int collaboratorId);
        Task<List<Collab>> GetCollaboratorByUserId(int userId);
        Task<List<Collab>> GetCollaboratorByNoteId(int userId, int NoteId);
         Task<List<Collab>> GetCollaboratorByRedisCache();
    }
}
