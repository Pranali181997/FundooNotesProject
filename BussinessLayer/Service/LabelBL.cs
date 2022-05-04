using BussinessLayer.Interface;
using CommonDatabaseLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
   public class LabelBL:ILabelBL
   {
        ILabelRL IlabelRL;
        public LabelBL(ILabelRL IlabelRL)
        {
            this.IlabelRL = IlabelRL;
        }
        public async Task AddLable(LablePostModel lablePostModel, int UserId, int NoteId)
        {
            try
            {
                await this.IlabelRL.AddLable(lablePostModel, UserId, NoteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
   
        public async Task<RepositoryLayer.Entity.Label> UpdateLable(int userId, int lableId, LablePostModel lablePostModel)
        {
            try
            {
                return await this.IlabelRL.UpdateLable(userId, lableId, lablePostModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //public async Task<List<RepositoryLayer.Entity.Label>> GetAllLabelsByNoteId(int NoteId, int UserId)
        //{
        //    try
        //    {
        //        return await this.IlabelRL.GetAllLabelsByNoteId(NoteId, UserId);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        public async Task DeleteLabel(int LabelId, int userId)
        {
            try
            {
                await this.IlabelRL.DeleteLabel(LabelId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<RepositoryLayer.Entity.Label>> Getlabel(int userId)
        {
            try
            {
                return await this.IlabelRL.Getlabel(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RepositoryLayer.Entity.Label>> GetlabelByNoteId(int NoteId)
        {
            try
            {
                return await this.IlabelRL.Getlabel(NoteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<RepositoryLayer.Entity.Label>> GetlabelByRedisCache()
        {
            try
            {
                return await this.IlabelRL.GetlabelByRedisCache();
            }
            catch (Exception)
            {

                throw;
            }        
        }
    }
}
