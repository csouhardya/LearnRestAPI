using ApplicationCore.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ApplicationCore.Services
{
    public class CachingService(IDistributedCache cache) : ICachingService
    {
        private readonly IDistributedCache _cache = cache;

        public void SetData<T>(string key, T data)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _cache.SetString(key, JsonSerializer.Serialize(data), options);
        }

        public T? GetData<T>(string key)
        {
            var data = _cache.GetString(key);

            if (data is null)
                return default(T);

            var jsonData = JsonSerializer.Deserialize<T>(data);
            return jsonData;
        }

        public void ReInsertData<T>(string key, T data)
        {
            this.RemoveData<T>(key);
            this.SetData<T>(key, data);
        }

        public void RemoveData<T>(string key)
        {
            _cache.Remove(key);
        }
    }
}
