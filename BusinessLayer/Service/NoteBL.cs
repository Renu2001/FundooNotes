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
        public NoteEntity CreateNote(NoteModel noteModel)
        {
            try
            {
                return _noteRL.CreateNote(noteModel);
            }
            catch(CustomizeException ex)
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
            catch (CustomizeException ex)
            {
                throw;
            }
        }

        public List<NoteEntity> GetAllNote()
        {
            try
            {
                return _noteRL.GetAllNote();
            }
            catch (CustomizeException ex)
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
            catch (CustomizeException ex)
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
            catch (CustomizeException ex)
            {
                throw;
            }
        }
    }
}
