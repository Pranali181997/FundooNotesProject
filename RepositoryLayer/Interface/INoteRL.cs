using CommonDatabaseLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface INoteRL
    {
        Task AddNote(NotePostModel notePostModel, int userId);
        Task<Note> GetNote(int noteId, int userId);
    }
}
