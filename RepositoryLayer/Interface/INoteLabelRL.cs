using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface INoteLabelRL
    {
        NoteEntity AddLabelsToNotes(int labelId, int noteId);
        IEnumerable<NoteLabelEntity> GetAllLabelsFromNotes();
        IEnumerable<NoteLabelEntity> GetAllNotesFromLabel(int labelId);
        NoteLabelEntity RemoveLabelsFromNotes(int labelId, int noteId);
    }
}
