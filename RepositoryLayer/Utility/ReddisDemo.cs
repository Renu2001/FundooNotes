using Microsoft.Extensions.Caching.Distributed;
using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RepositoryLayer.Utility
{
    public class ReddisDemo
    {
        private readonly IDistributedCache _cache;
        public ReddisDemo(IDistributedCache cache)
        {
            _cache = cache;
        }
        public void SetCache<T>(T result2,string cacheKey)
        {
            var cachedDataString2 = JsonSerializer.Serialize(result2);
            var newDataToCache = Encoding.UTF8.GetBytes(cachedDataString2);
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(24))
                .SetSlidingExpiration(TimeSpan.FromMinutes(12));
            _cache.Set(cacheKey, newDataToCache, options);
        }

        public T GetCache<T>(string cacheKey) where T : class
        {
            var cachedData = _cache.Get(cacheKey);
            if (cachedData != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                return JsonSerializer.Deserialize<T>(cachedDataString);
            }
            return null;
        }
        public void RemoveCache(string cacheKey) 
        {
           _cache.Remove(cacheKey);   
        }
    }
}
