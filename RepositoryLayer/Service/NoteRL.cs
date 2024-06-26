using Microsoft.Extensions.Caching.Distributed;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Utility;
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
        public const string cacheKey = "RedisCachingDemoGET_ALL_PRODUCTS";
        List<NoteEntity> result;
        public NoteRL(FundooContext fundooContext, IDistributedCache cache, RabbitDemo rabitMQProducer)
        {
            this.fundooContext = fundooContext;
            _cache = cache;
            _rabitMQProducer = rabitMQProducer;
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
                    var cachedData = _cache.Get(cacheKey);
                    if (cachedData != null)
                    {
                        var cachedDataString = Encoding.UTF8.GetString(cachedData);
                        var result2 = JsonSerializer.Deserialize<List<NoteEntity>>(cachedDataString) ?? new List<NoteEntity>();
                        foreach (var noteEntity in result2)
                        {
                            if (noteEntity.NoteId == id)
                            {
                                noteEntity.IsArchived = !noteEntity.IsArchived;
                            }
                        }
                        var cachedDataString2 = JsonSerializer.Serialize(result2);
                        var newDataToCache = Encoding.UTF8.GetBytes(cachedDataString2);
                        var options = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(24))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(12));
                        _cache.SetAsync(cacheKey, newDataToCache, options);
                    }
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
                var cachedData = _cache.Get(cacheKey);
                if (cachedData != null)
                {
                    var cachedDataString = Encoding.UTF8.GetString(cachedData);
                    var result2 = JsonSerializer.Deserialize<List<NoteEntity>>(cachedDataString) ?? new List<NoteEntity>();
                    result2.Add(noteEntity);
                    var cachedDataString2 = JsonSerializer.Serialize(result2);
                    var newDataToCache = Encoding.UTF8.GetBytes(cachedDataString2);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(24))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(12));
                    _cache.SetAsync(cacheKey, newDataToCache, options);
                }
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
                    _cache.Remove(cacheKey);
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

        public IEnumerable<NoteEntity> GetAllNote()
        {
           
            var cachedData = _cache.Get(cacheKey);
            if (cachedData != null)
            {

                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                result = JsonSerializer.Deserialize<List<NoteEntity>>(cachedDataString) ?? new List<NoteEntity>();
            }
            else
            {
                result = fundooContext.Notes?.ToList();
                //result.RemoveAll(note => note.IsTrashed || note.IsArchived);
                var cachedDataString = JsonSerializer.Serialize(result);
                var newDataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(50))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(120));
                _cache.Set(cacheKey, newDataToCache, options);
            }

            if (result != null)
            {
                return result;
            }
            else
            {
                throw new CustomizeException("No Note Found");
            }

        }

        public IEnumerable<NoteEntity> GetAllTrashNote()
        {
            List<NoteEntity> notes;
            var cachedData = _cache.Get(cacheKey);
            if (cachedData != null)
            {

                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                result = JsonSerializer.Deserialize<List<NoteEntity>>(cachedDataString) ?? new List<NoteEntity>();
                notes = result.Where(note => note.IsTrashed).ToList();
            }
            else
            {
                notes = fundooContext.Notes?.Where(note => note.IsTrashed).ToList();
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

        public IEnumerable<NoteEntity> GetAllArchievedNote()
        {
            List<NoteEntity> notes;
            var cachedData = _cache.Get(cacheKey);
            if (cachedData != null)
            {

                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                result = JsonSerializer.Deserialize<List<NoteEntity>>(cachedDataString) ?? new List<NoteEntity>();
                notes = result.Where(note => note.IsArchived && note.IsTrashed == false).ToList();
            }
            else
            {
                notes = fundooContext.Notes?.Where(note => note.IsArchived && note.IsTrashed == false).ToList();
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

        public NoteEntity GetNoteById(int id)
        {
            NoteEntity notes;
            var cachedData = _cache.Get(cacheKey);
            if (cachedData != null)
            {

                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                result = JsonSerializer.Deserialize<List<NoteEntity>>(cachedDataString) ?? new List<NoteEntity>();
                notes =result.Where(note => note.NoteId == id).FirstOrDefault(); ; 
            }
            else
            {
                notes = fundooContext.Notes?.Find(id); ;
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

        public NoteEntity TrashById(int id)
        {
            var result = fundooContext.Notes?.Find(id);
            if (result != null)
            {
                result.IsTrashed = !result.IsTrashed;
                fundooContext.Notes?.Update(result);
                fundooContext.SaveChanges();
                var cachedData = _cache.Get(cacheKey);
                if (cachedData != null)
                {
                    var cachedDataString = Encoding.UTF8.GetString(cachedData);
                    var result2 = JsonSerializer.Deserialize<List<NoteEntity>>(cachedDataString) ?? new List<NoteEntity>();
                    foreach (var noteEntity in result2)
                    {
                        if (noteEntity.NoteId == id)
                        {
                            noteEntity.IsTrashed = !noteEntity.IsTrashed;
                        }
                    }
                    var cachedDataString2 = JsonSerializer.Serialize(result2);
                    var newDataToCache = Encoding.UTF8.GetBytes(cachedDataString2);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(24))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(12));
                    _cache.SetAsync(cacheKey, newDataToCache, options);
                }
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
                    var cachedData = _cache.Get(cacheKey);
                    if (cachedData != null)
                    {
                        var cachedDataString = Encoding.UTF8.GetString(cachedData);
                        var result2 = JsonSerializer.Deserialize<List<NoteEntity>>(cachedDataString) ?? new List<NoteEntity>();
                        foreach (var noteEntity in result2)
                        {
                            if (noteEntity.NoteId == id)
                            {
                                noteEntity.Title= model.Title;
                                noteEntity.Description=model.Description;
                            }
                        }
                        var cachedDataString2 = JsonSerializer.Serialize(result2);
                        var newDataToCache = Encoding.UTF8.GetBytes(cachedDataString2);
                        var options = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(24))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(12));
                        _cache.SetAsync(cacheKey, newDataToCache, options);
                    }
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
