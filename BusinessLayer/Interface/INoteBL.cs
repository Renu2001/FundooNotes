using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface INoteBL
    {
        public NoteEntity CreateNote(NoteModel noteModel);
        public NoteEntity DeleteNoteById(int id);
        public List<NoteEntity> GetAllNote();
        public NoteEntity GetNoteById(int id);
        public NoteEntity UpdateById(int id, NoteModel model);
    }
}
