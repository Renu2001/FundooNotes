﻿using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ICollaboratorRL
    {
        CollaboratorEntity AddCollaborator(CollaboratorModel model);
        CollaboratorEntity DeleteCollaborator(int noteId, string email);
        IEnumerable<CollaboratorEntity> GetCollaboratorById(int noteId);
    }
}