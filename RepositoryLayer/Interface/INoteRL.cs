using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface INoteRL
    {
        public NoteEntity CreateNote(NoteModel noteModel);
        NoteEntity DeleteNoteById(int id);
        List<NoteEntity> GetAllNote();
        public NoteEntity GetNoteById(int id);
        NoteEntity UpdateById(int id, NoteModel model);
    }
}
