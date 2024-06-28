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
        IEnumerable<NoteWithLabelDTO> GetAllLabelsAndNotesFromAllNotes();
        NoteEntity AddLabelsToNotes(int labelId, int noteId);
        IEnumerable<LabelEntity> GetAllLabelsFromNotes(int noteid);
        IEnumerable<NoteEntity> GetAllNotesFromLabel(int labelId);
        NoteLabelEntity RemoveLabelsFromNotes(int labelId, int noteId);
    }
}
