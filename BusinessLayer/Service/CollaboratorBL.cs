using BusinessLayer.Interface;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class CollaboratorBL : ICollaboratorBL
    {
        private ICollaboratorRL _collaboratorRL;
        public CollaboratorBL(ICollaboratorRL collaboratorRL)
        {
            _collaboratorRL = collaboratorRL;
        }
        public CollaboratorEntity AddCollaborator(CollaboratorModel model)
        {
            try
            {
                return _collaboratorRL.AddCollaborator(model);
            }
            catch
            {
                throw;
            }
            throw new NotImplementedException();
        }

        public CollaboratorEntity DeleteCollaborator(int noteId, string emai)
        {
            try
            {
                return _collaboratorRL.DeleteCollaborator( noteId, emai);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<CollaboratorEntity> GetCollaboratorById(int noteId)
        {
            try
            {
                return _collaboratorRL.GetCollaboratorById(noteId);
            }
            catch
            {
                throw;
            }
        }
    }
}
