using CommonDatabaseLayer;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.FundooNoteContex;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient.DataClassification;

namespace RepositoryLayer.Service
{
    public class LabelRL : ILabelRL
    {
        FundooContext fundoo;
        
        public IConfiguration Configuration { get; }
        public LabelRL(FundooContext fundoo, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.fundoo = fundoo;
        }
        public async Task AddLable(LablePostModel lablePostModel, int UserId, int NoteId)
        {
            try
            {
                var user = fundoo.Users.FirstOrDefault(u => u.UserId == UserId);
                var note = fundoo.Note.FirstOrDefault(u => u.NoteId == NoteId);
                Entity.Label lable = new Entity.Label
                {
                    User = user,
                    Note = note
                };
                lable.LableName = lablePostModel.LableName;
                fundoo.lable.Add(lable);
                await fundoo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Entity.Label> UpdateLable(int userId, int lableId, LablePostModel lablePostModel)
        {
            try
            {
                var res1 = fundoo.lable.FirstOrDefault(u => u.LableId == lableId && u.UserId == userId);
                if (res1 != null)
                {
                    res1.LableName = lablePostModel.LableName;
                    await fundoo.SaveChangesAsync();
                    return await fundoo.lable.Where(a => a.LableId == lableId).FirstOrDefaultAsync();                   
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //public async Task<List<Entity.Label>>GetAllLabelsByNoteId(int noteId, int userId)
        //{
        //    try
        //    {
        //        return await fundoo.lable.Where(u => u.NoteId==noteId && u.UserId == userId).Include(u => u.User).ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //
        public async Task DeleteLabel(int LabelId, int userId)
        {
            try
            {
                var result = fundoo.lable.FirstOrDefault(u => u.LableId == LabelId && u.UserId == userId);
                fundoo.lable.Remove(result);
                await fundoo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Entity.Label>> Getlabel(int userId)
        {
            try
            {
                List<Entity.Label> reuslt = await fundoo.lable.Where(u => u.UserId == userId).Include(u => u.User).Include(u => u.Note).ToListAsync();
                return reuslt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Entity.Label>> GetlabelByNoteId(int NoteId)
        {
            try
            {
                List<Entity.Label> reuslt = await fundoo.lable.Where(u => u.NoteId == NoteId).Include(u => u.User).Include(u => u.Note).ToListAsync();
                return reuslt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Entity.Label>> GetlabelByRedisCache()
        {
            try
            {
                List<Entity.Label> reuslt = await fundoo.lable.ToListAsync();
                return reuslt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
   

    
