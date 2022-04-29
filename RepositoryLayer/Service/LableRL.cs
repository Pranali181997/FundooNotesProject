using CommonDatabaseLayer;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.FundooNoteContex;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection.Emit;

namespace RepositoryLayer.Service
{
    public class LableRL : ILableRL
    {
        FundooContext fundoo;
        public IConfiguration Configuration { get; }
        public LableRL(FundooContext fundoo, IConfiguration configuration)
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
        //public async Task<List<Label>> GetAllLabelsByNoteId(int NoteId, int UserId)
        //{
        //    try
        //    {
        //        return await fundoo.Note.Where(u => u.UserId == userId).Include(u => u.User).ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }            
        //}
    }
}
