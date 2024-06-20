using BusinessLayer.Interface;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class NoteBL : INoteBL
    {
        private INoteRL _noteRL;
        public NoteBL(INoteRL noteRL)
        {
            _noteRL = noteRL;
        }

        public NoteEntity ArchieveById(int id)
        {
            try
            {
                return _noteRL.ArchieveById(id);
            }
            catch
            {
                throw;
            }
        }

        public NoteEntity CreateNote(NoteModel noteModel)
        {
            try
            {
                return _noteRL.CreateNote(noteModel);
            }
            catch
            {
                throw;
            }
        }

        public NoteEntity DeleteNoteById(int id)
        {
            try
            {
                return _noteRL.DeleteNoteById(id);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<NoteEntity> GetAllArchievedNote()
        {
            try
            {
                return _noteRL.GetAllArchievedNote();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<NoteEntity> GetAllNote()
        {
            try
            {
                return _noteRL.GetAllNote();
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<NoteEntity> GetAllTrashNote()
        {
            try
            {
                return _noteRL.GetAllTrashNote();
            }
            catch
            {
                throw;
            }
        }

        public NoteEntity GetNoteById(int id)
        {
            try
            {
                return _noteRL.GetNoteById(id);
            }
            catch
            {
                throw;
            }
        }

        public NoteEntity TrashById(int id)
        {
            try
            {
                return _noteRL.TrashById(id);
            }
            catch
            {
                throw;
            }

        }

        public NoteEntity UpdateById(int id, NoteModel model)
        {
            try
            {
                return _noteRL.UpdateById(id, model);
            }
            catch
            {
                throw;
            }
        }
    }
}
