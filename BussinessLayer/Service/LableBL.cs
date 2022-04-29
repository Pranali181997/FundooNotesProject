using BussinessLayer.Interface;
using CommonDatabaseLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
   public class LableBL:ILableBL
   {
        ILableRL IlableRL;
        public LableBL(ILableRL lableRL)
        {
            this.IlableRL = lableRL;
        }
        public async Task AddLable(LablePostModel lablePostModel, int UserId, int NoteId)
        {
            try
            {
                await this.IlableRL.AddLable(lablePostModel, UserId, NoteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
