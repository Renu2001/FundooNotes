using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NoteRL : INoteRL
    {
        private readonly FundooContext fundooContext;
        public NoteRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public NoteEntity CreateNote(NoteModel noteModel)
        {
            NoteEntity noteEntity = new NoteEntity();
            noteEntity.Title = noteModel.Title;
            noteEntity.Description = noteModel.Description;
            noteEntity.IsArchived = noteModel.IsArchived;
            noteEntity.IsDeleted = noteModel.IsDeleted;
            try
            {
                fundooContext.Notes.Add(noteEntity);
                fundooContext.SaveChanges();
                return noteEntity;
            }
            catch (Exception ex)
            {
                throw new CustomizeException(ex.Message);
            }
        }

        public NoteEntity DeleteNoteById(int id)
        {
            var result = fundooContext.Notes.Find(id);
            if (result != null)
            {
                try
                {
                    fundooContext.Notes.Remove(result);
                    fundooContext.SaveChanges();
                    return result;
                }
                catch(Exception ex)
                {
                    throw new CustomizeException(ex.Message);
                }
               
            }
            else
            {
                throw new CustomizeException("No Note Found");
            }
            
        }

        public List<NoteEntity> GetAllNote()
        {
            var result = fundooContext.Notes.ToList();
            if(result != null)
            {
                return result;
            }
            else
            {
                throw new CustomizeException("No Note Found");
            }
           
        }

        public NoteEntity GetNoteById(int id)
        {
            var entity = fundooContext.Notes.Find(id);
            if (entity != null)
            {
                return entity;
            }
            else
            {
                throw new CustomizeException("No Note Found");
            }
            
        }

        public NoteEntity UpdateById(int id, NoteModel model)
        {
            var entity = fundooContext.Notes.Find(id);
            if (entity != null)
            {
                entity.Title = model.Title;
                entity.Description = model.Description;
                entity.IsArchived = model.IsArchived;
                entity.IsDeleted = model.IsDeleted;
                try
                {
                    fundooContext.Notes.Update(entity);
                    fundooContext.SaveChanges();
                    return entity;
                }
                catch(Exception ex)
                {
                    throw new CustomizeException(ex.Message);
                }
            }
            else
            {
                throw new CustomizeException("Note Not Found");
            }

        }
    }
}
