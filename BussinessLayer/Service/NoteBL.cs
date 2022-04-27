using BussinessLayer.Interface;
using CommonDatabaseLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
        public class NoteBL : INoteBL
        {
            INoteRL noteRL;
            public NoteBL(INoteRL userRL)
            {
                this.noteRL = userRL;
            }
            public async Task AddNote(NotePostModel notePostModel, int userId)
            {
                try
                {
                    await this.noteRL.AddNote(notePostModel, userId);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        public async Task<Note> GetNote(int noteId, int userId)
        {
            try
            {
                return await this.noteRL.GetNote(noteId, userId); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
