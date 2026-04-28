using ApplicationCore.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ApplicationCore.Services
{
    public class CachingService(IDistributedCache cache, ILogger<ICachingService> logger) : ICachingService
    {
        private readonly IDistributedCache _cache = cache;
        private readonly ILogger<ICachingService> _logger;

        public void SetData<T>(string key, T data)
        {
            _logger.LogInformation($"Setting Cache");
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _cache.SetString(key, JsonSerializer.Serialize(data), options);
            _logger.LogInformation($"Cache data set successfully");
        }

        public T? GetData<T>(string key)
        {
            _logger.LogInformation($"Getting data from cache");
            var data = _cache.GetString(key);

            if (data is null)
            {
                _logger.LogWarning($"Not cache found with key {key}");
                return default(T);
            }

            _logger.LogInformation($"Data fetched successfully");
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
            _logger.LogInformation($"Removing data from cache");
            _cache.Remove(key);
        }
    }
}
