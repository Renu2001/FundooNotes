using BusinessLayer.Interface;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class NoteLabelBL : INoteLabelBL
    {
        private INoteLabelRL _notelabelRL;
        public NoteLabelBL(INoteLabelRL notelabelRL)
        {
            _notelabelRL = notelabelRL;
        }
        public NoteEntity AddLabelsToNotes(int labelId, int noteId)
        {
            try
            {
                return _notelabelRL.AddLabelsToNotes(labelId,noteId);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<object> GetAllLabelsAndNotesFromAllNotes()
        {
            try
            {
                return _notelabelRL.GetAllLabelsAndNotesFromAllNotes();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<LabelEntity> GetAllLabelsFromNotes(int noteid)
        {
            try
            {
                return _notelabelRL.GetAllLabelsFromNotes(noteid);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<NoteEntity> GetAllNotesFromLabel(int labelId)
        {
            try
            {
                return _notelabelRL.GetAllNotesFromLabel(labelId);
            }
            catch
            {
                throw;
            }
        }

        public NoteLabelEntity RemoveLabelsFromNotes(int labelId, int noteId)
        {
            try
            {
                return _notelabelRL.RemoveLabelsFromNotes(labelId,noteId);
            }
            catch
            {
                throw;
            }
        }
    }
}
