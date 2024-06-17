using Microsoft.EntityFrameworkCore;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NoteLabelRL : INoteLabelRL
    {
        private readonly FundooContext fundooContext;
        public NoteLabelRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        public NoteEntity AddLabelsToNotes(int labelId,int noteId)
        {
            var note = fundooContext.Notes.FirstOrDefault(n => n.NoteId == noteId);
            if (note == null)
            {
                throw new ArgumentException("Note not found");
            }

            var label = fundooContext.Labels.FirstOrDefault(l => l.LabelId == labelId);
            if (label == null)
            {
                throw new ArgumentException("Label not found");
            }

            var noteLabel = new NoteLabelEntity
            {
                NoteId = noteId,
                LabelId = labelId,
                Notes = note,
                Labels = label
            };
            fundooContext.NoteLabel.Add(noteLabel);
            fundooContext.SaveChanges();

            return note;
        }

        public IEnumerable<NoteLabelEntity> GetAllLabelsFromNotes()
        {
            return fundooContext.NoteLabel
                .Include(nl => nl.Notes)
                .Include(nl => nl.Labels)
                .ToList();
        }

        public IEnumerable<NoteLabelEntity> GetAllNotesFromLabel(int labelId)
        {
            var label = fundooContext.Labels.FirstOrDefault(x=>x.LabelId==labelId);
            if (label == null)
            {
                throw new ArgumentException("Label not found");
            }

            return fundooContext.NoteLabel
                .Include(nl => nl.Notes)
                .Include(nl => nl.Labels)
                .Where(nl => nl.LabelId == label.LabelId)
                .ToList();
        }

        public NoteLabelEntity RemoveLabelsFromNotes(int labelId, int noteId)
        {
            var noteLabel = fundooContext.NoteLabel.FirstOrDefault(nl => nl.NoteId == noteId && nl.LabelId == labelId);
            if (noteLabel == null)
            {
                throw new ArgumentException("Label not found for the given note");
            }

            fundooContext.NoteLabel.Remove(noteLabel);
            fundooContext.SaveChanges();

            return noteLabel;
        }
    }
}
