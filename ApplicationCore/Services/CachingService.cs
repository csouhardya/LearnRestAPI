using ApplicationCore.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;
using System.Text.Json;

namespace ApplicationCore.Services
{
    public class CachingService(IDistributedCache cache, ILogger logger) : ICachingService
    {
        private readonly IDistributedCache _cache = cache;
        private readonly ILogger _logger = logger;

        public void SetData<T>(string key, T data)
        {
            _logger.Information($"Setting Cache");
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _cache.SetString(key, JsonSerializer.Serialize(data), options);
            _logger.Information($"Cache data set successfully");
        }

        public T? GetData<T>(string key)
        {
            _logger.Information($"Getting data from cache");
            var data = _cache.GetString(key);

            if (data is null)
            {
                _logger.Warning($"Not cache found with key {key}");
                return default(T);
            }

            _logger.Information($"Data fetched successfully");
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
            _logger.Information($"Removing data from cache");
            _cache.Remove(key);
        }
    }
}
