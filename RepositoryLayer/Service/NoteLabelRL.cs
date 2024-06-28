using Microsoft.EntityFrameworkCore;
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
    public class NoteLabelRL : INoteLabelRL
    {
        private readonly FundooContext fundooContext;
        public NoteLabelRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public IEnumerable<NoteWithLabelDTO> GetAllLabelsAndNotesFromAllNotes()
        {
            var notesWithLabels = fundooContext.Notes
                .Include(n => n.NoteLabel)
                    .ThenInclude(nl => nl.Labels)
                .Select(n => new NoteWithLabelDTO
                {
                    NoteId = n.NoteId,
                    Title = n.Title,
                    Description = n.Description,
                    IsTrashed = n.IsTrashed,
                    IsArchived = n.IsArchived,
                    Labels = n.NoteLabel.Select(nl => new LabelDTO
                    {
                        LabelId = nl.Labels.LabelId,
                        LabelName = nl.Labels.LabelName
                    }).ToList()
                })
                .ToList();

            if (!notesWithLabels.Any())
            {
                throw new CustomizeException("No labels and notes found");
            }
            return notesWithLabels;

        }

        public NoteEntity AddLabelsToNotes(int labelId,int noteId)
        {
            var note = fundooContext.Notes.FirstOrDefault(n => n.NoteId == noteId);
            if (note == null)
            {
                throw new CustomizeException("Note not found");
            }

            var label = fundooContext.Labels.FirstOrDefault(l => l.LabelId == labelId);
            if (label == null)
            {
                throw new CustomizeException("Label not found");
            }

            var noteLabel = new NoteLabelEntity
            {
                NoteId = noteId,
                LabelId = labelId,
                Notes = note,
                Labels = label
            };
            var result = fundooContext.NoteLabel.FirstOrDefault(n => n.NoteId == noteId && n.LabelId == labelId);
            if (result == null)
            {
                fundooContext.NoteLabel.Add(noteLabel);
                fundooContext.SaveChanges();
                return note;
            }
            else
            {
                throw new CustomizeException("Already exists");

            }
        }


        public IEnumerable<LabelEntity> GetAllLabelsFromNotes(int noteid)
        {
            var result = fundooContext.Notes.Include(n=>n.NoteLabel).ThenInclude(n=>n.Labels).FirstOrDefault(n=>n.NoteId==noteid);
            if (result == null)
            {
                throw new CustomizeException("Note not found");
            }
            return result.NoteLabel.Select(n=>n.Labels).ToList();
        }

        public IEnumerable<NoteEntity> GetAllNotesFromLabel(int labelId)
        {
            var label = fundooContext.Labels.FirstOrDefault(x=>x.LabelId==labelId);
            if (label == null)
            {
                throw new CustomizeException("Label not found");
            }
            var matchlabel =  fundooContext.Labels.Include(n => n.NoteLabel).ThenInclude(n => n.Notes).FirstOrDefault(l => l.LabelId == labelId);
            return matchlabel.NoteLabel.Select(n => n.Notes).ToList();
        }

        public NoteLabelEntity RemoveLabelsFromNotes(int labelId, int noteId)
        {
            var noteLabel = fundooContext.NoteLabel.FirstOrDefault(nl => nl.NoteId == noteId && nl.LabelId == labelId);
            if (noteLabel == null)
            {
                throw new CustomizeException("Label not found for the given note");
            }

            fundooContext.NoteLabel.Remove(noteLabel);
            fundooContext.SaveChanges();

            return noteLabel;
        }
    }
}
