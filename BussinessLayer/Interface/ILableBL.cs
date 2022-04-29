using CommonDatabaseLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Interface
{
   public interface ILableBL
    {
        Task AddLable(LablePostModel lablePostModel, int UserId, int NoteId);
    }
}
