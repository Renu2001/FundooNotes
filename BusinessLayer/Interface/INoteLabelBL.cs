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
        public NoteEntity AddLabelsToNotes(int labelId, int noteId);
        public IEnumerable<NoteLabelEntity> GetAllLabelsFromNotes();
        public IEnumerable<NoteLabelEntity> GetAllNotesFromLabel(int labelId);
        public NoteLabelEntity RemoveLabelsFromNotes(int labelId, int noteId);
    }
}
