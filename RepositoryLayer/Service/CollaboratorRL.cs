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
    public class CollaboratorRL : ICollaboratorRL
    {
        private readonly FundooContext fundooContext;
        public CollaboratorRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        public CollaboratorEntity AddCollaborator(CollaboratorModel model)
        {
            var entity = fundooContext.Users.FirstOrDefault(x=>x.email==model.EmailId);
            if(entity != null) 
            {
                CollaboratorEntity collaborator = new CollaboratorEntity();
                collaborator.Email = model.EmailId;
                collaborator.NotesId = model.NoteId;
                try
                {
                    fundooContext.Collaborators?.Add(collaborator);
                    fundooContext.SaveChanges();
                    return collaborator;
                }
                catch(Exception ex)
                {
                    throw new CustomizeException(ex.Message);
                }


            }
            else
            {
                throw new CustomizeException("Collaborator Doesn't Exists");
            }

        }

        public CollaboratorEntity DeleteCollaborator(int noteId, string email)
        {
            var entity = fundooContext.Collaborators.FirstOrDefault(x => x.Email == email && x.NotesId==noteId) ;
            if (entity != null)
            {
                try
                {
                    fundooContext.Collaborators?.Remove(entity);
                    fundooContext.SaveChanges();
                    return entity;
                }
                catch (Exception ex)
                {
                    throw new CustomizeException(ex.Message);
                }

            }
            else
            {
                throw new CustomizeException("No Collaborator Found");
            }
        }

        public IEnumerable<CollaboratorEntity> GetCollaboratorById(int noteid)
        {
            var entity = fundooContext.Collaborators.Where(x => x.NotesId == noteid);
            if (entity != null)
            {
                try
                {
                    return entity;
                }
                catch (Exception ex)
                {
                    throw new CustomizeException(ex.Message);
                }

            }
            else
            {
                throw new CustomizeException("No Collaborator Found");
            }
        }
    }
}
