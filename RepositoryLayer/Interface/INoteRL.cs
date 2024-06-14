﻿using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface INoteRL
    {
        NoteEntity ArchieveById(int id);
        public NoteEntity CreateNote(NoteModel noteModel);
        public NoteEntity DeleteNoteById(int id);
        public IEnumerable<NoteEntity> GetAllNote();
        public NoteEntity GetNoteById(int id);
        NoteEntity TrashById(int id);
        public NoteEntity UpdateById(int id, NoteModel model);
    }
}