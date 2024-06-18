using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface INoteLabelBL
    {
        public IEnumerable<object> GetAllLabelsAndNotesFromAllNotes();
        public NoteEntity AddLabelsToNotes(int labelId, int noteId);
        public IEnumerable<LabelEntity> GetAllLabelsFromNotes(int noteid);
        public IEnumerable<NoteEntity> GetAllNotesFromLabel(int labelId);
        public NoteLabelEntity RemoveLabelsFromNotes(int labelId, int noteId);
    }
}
