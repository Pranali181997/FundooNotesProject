using CommonDatabaseLayer;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ILabelRL
    {
        Task AddLable(LablePostModel lablePostModel, int UserId, int NoteId);
        Task<Entity.Label> UpdateLable(int userId, int lableId, LablePostModel lablePostModel);
        //Task<List<Entity.Label>> GetAllLabelsByNoteId(int noteId, int userId);
        Task DeleteLabel(int LabelId, int userId);
        Task<List<Entity.Label>> Getlabel(int userId);
        Task<List<Entity.Label>> GetlabelByNoteId(int NoteId);
        Task<List<Entity.Label>> GetlabelByRedisCache();
    }
}
