using CommonDatabaseLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ILableRL
    {
        Task AddLable(LablePostModel lablePostModel, int UserId, int NoteId);

    }
}
