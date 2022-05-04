using CommonDatabaseLayer;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Interface
{
   public interface ILabelBL
    {
        Task AddLable(LablePostModel lablePostModel, int UserId, int NoteId);
      
        Task<RepositoryLayer.Entity.Label> UpdateLable(int userId, int lableId, LablePostModel lablePostModel);
        //Task<List<RepositoryLayer.Entity.Label>> GetAllLabelsByNoteId(int noteId, int userId);
        Task DeleteLabel(int LabelId, int userId);
        Task<List<RepositoryLayer.Entity.Label>> Getlabel(int userId);
        Task<List<RepositoryLayer.Entity.Label>> GetlabelByNoteId(int NoteId);
        Task<List<RepositoryLayer.Entity.Label>> GetlabelByRedisCache();
    }
}
