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

        public NoteEntity ArchieveById(int id)
        {
            var result = fundooContext.Notes?.Find(id);
            if (result != null && result.IsTrashed != true)
            {
                try
                {
                        result.IsArchived = !result.IsArchived;
                        fundooContext.Notes?.Update(result);
                        fundooContext.SaveChanges();
                        return result;
                    
                }
                catch (Exception ex)
                {
                    throw new CustomizeException(ex.Message);
                }
            }
            else
            {
                throw new CustomizeException("Cannot be Archive!!!");
            }

        }

        public NoteEntity CreateNote(NoteModel noteModel)
        {
            NoteEntity noteEntity = new NoteEntity() { };
            noteEntity.Title = noteModel.Title;
            noteEntity.Description = noteModel.Description;
            try
            {
                fundooContext.Notes?.Add(noteEntity);
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
            var result = fundooContext.Notes?.Find(id);
            if (result != null)
            {
                try
                {
                    fundooContext.Notes?.Remove(result);
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

        public IEnumerable<NoteEntity> GetAllNote()
        {
            var result = fundooContext.Notes?.ToList();
            if(result != null)
            {
                result.RemoveAll(note => note.IsTrashed || note.IsArchived);
                return result;
            }
            else
            {
                throw new CustomizeException("No Note Found");
            }
           
        }

        public NoteEntity GetNoteById(int id)
        {
            var entity = fundooContext.Notes?.Find(id);
            if (entity != null)
            {
                return entity;
            }
            else
            {
                throw new CustomizeException("No Note Found");
            }
            
        }

        public NoteEntity TrashById(int id)
        {
            var result = fundooContext.Notes?.Find(id);
            if (result != null)
            {
                result.IsTrashed = !result.IsTrashed;
                result.IsArchived = !result.IsArchived;
                fundooContext.Notes?.Update(result);
                fundooContext.SaveChanges();
            }
            return result;
        }

        public NoteEntity UpdateById(int id, NoteModel model)
        {
            var entity = fundooContext.Notes?.Find(id);
            if (entity != null)
            {
                entity.Title = model.Title;
                entity.Description = model.Description;
                try
                {
                    fundooContext.Notes?.Update(entity);
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
