using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Utility;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NoteRL : INoteRL
    {
        private readonly FundooContext fundooContext;
        private readonly IDistributedCache _cache;
        private readonly RabbitDemo _rabitMQProducer;
        private readonly ReddisDemo _reddis;
        public const string cacheKey = "RedisCachingDemoGET_ALL_PRODUCTS";
        public NoteRL(FundooContext fundooContext, IDistributedCache cache, RabbitDemo rabitMQProducer, ReddisDemo reddis)
        {
            this.fundooContext = fundooContext;
            _cache = cache;
            _rabitMQProducer = rabitMQProducer;
            _reddis = reddis;
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
                    _reddis.RemoveCache(cacheKey);
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
                _reddis.RemoveCache(cacheKey);
                _rabitMQProducer.SendProductMessage(noteEntity);
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
                    _reddis.RemoveCache(cacheKey);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new CustomizeException(ex.Message);
                }

            }
            else
            {
                throw new CustomizeException("No Note Found");
            }

        }

        public IEnumerable<NoteWithLabelDTO> GetAllLabelsAndNotesFromAllNotes()
        {
            var cachedData = _reddis.GetCache<List<NoteWithLabelDTO>>(cacheKey);
            if (cachedData != null)
            {
                cachedData.RemoveAll(note => note.IsTrashed || note.IsArchived);
                return cachedData;
            }
            else
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
                _reddis.SetCache(notesWithLabels, cacheKey);
                notesWithLabels.RemoveAll(note => note.IsTrashed || note.IsArchived);
                return notesWithLabels;
            }

        }
 

        public IEnumerable<NoteWithLabelDTO> GetAllTrashNote()
        {
            List<NoteWithLabelDTO> notes;
            var cachedData = _reddis.GetCache<List<NoteWithLabelDTO>>(cacheKey);
            if (cachedData != null)
            {
                notes = cachedData.Where(note => note.IsTrashed).ToList();
                return notes;
            }
           
            else
            {
                notes = fundooContext.Notes
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
                notes.Where(n => n.IsTrashed).ToList();

            }
            if (notes != null)
            {
                return notes;
            }
            else
            {
                throw new CustomizeException("No Note Found");
            }

        }

        public IEnumerable<NoteWithLabelDTO> GetAllArchievedNote()
        {
            List<NoteWithLabelDTO> notes;
            var cachedData = _reddis.GetCache<List<NoteWithLabelDTO>>(cacheKey);
            if (cachedData != null)
            {
                notes = cachedData.Where(note => note.IsArchived && note.IsTrashed == false).ToList();
                return notes;
            }

            else
            {
                notes = fundooContext.Notes
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
                notes.Where(note => note.IsArchived && note.IsTrashed == false).ToList();

            }
           
            if (notes != null)
            {
                return notes;
            }
            else
            {
                throw new CustomizeException("No Note Found");
            }
        }

        public NoteWithLabelDTO GetNoteById(int id)
        {
            NoteWithLabelDTO note;
            var cachedData = _reddis.GetCache<List<NoteWithLabelDTO>>(cacheKey);
            if (cachedData != null)
            {
                note = cachedData.FirstOrDefault(n => n.NoteId == id);
                if (note != null)
                {
                    return note;
                }
            }

            var notes = fundooContext.Notes
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
                .Where(n => n.IsArchived && !n.IsTrashed)
                .ToList();

            note = notes.FirstOrDefault(n => n.NoteId == id);
            return note;


        }
        public NoteEntity TrashById(int id)
        {
            var result = fundooContext.Notes?.Find(id);
            if (result != null)
            {
                result.IsTrashed = !result.IsTrashed;
                fundooContext.Notes?.Update(result);
                fundooContext.SaveChanges();
                _reddis.RemoveCache(cacheKey);
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
                    _reddis.RemoveCache(cacheKey);
                    return entity;
                }
                catch (Exception ex)
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
