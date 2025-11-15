using BFF.Application.Interfaces.Services;
using BFF.Domain.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace BFF.Infrastructure.Services;

[ExcludeFromCodeCoverage]
public class CacheService : ICacheService
{
    private readonly IDistributedCache? _distributedCache;
    private readonly IMemoryCache? _memoryCache;
    private readonly RedisSettings _redisSettings;
    private readonly ILogger<CacheService> _logger;
    private readonly bool _useDistributedCache;
    private readonly System.Collections.Concurrent.ConcurrentDictionary<string, byte> _memoryKeys = new();

    public CacheService(
        IServiceProvider serviceProvider,
        IOptions<RedisSettings> redisOptions,
        ILogger<CacheService> logger)
    {
        _redisSettings = redisOptions.Value;
        _logger = logger;

        _distributedCache = serviceProvider.GetService<IDistributedCache>();
        _memoryCache = serviceProvider.GetService<IMemoryCache>();

        _useDistributedCache = _distributedCache != null &&
                              !string.IsNullOrEmpty(_redisSettings.ConnectionString);

        _logger.LogInformation("Cache configurado: {CacheType}",
            _useDistributedCache ? "Redis (Distributed)" : "Memory");
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        var fullKey = GetFullKey(key);

        try
        {
            if (_useDistributedCache && _distributedCache != null)
            {
                var cachedValue = await _distributedCache.GetStringAsync(fullKey);
                if (!string.IsNullOrEmpty(cachedValue))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    return JsonSerializer.Deserialize<T>(cachedValue, options);
                }
            }
            else if (_memoryCache != null)
            {
                return _memoryCache.Get<T>(fullKey);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar item do cache: {Key}. Erro: {Error}", fullKey, ex.Message);
            await RemoveAsync(key);
        }

        return null;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
    {
        var fullKey = GetFullKey(key);
        var exp = expiration ?? _redisSettings.DefaultExpiration;

        try
        {
            if (_useDistributedCache && _distributedCache != null)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                var serializedValue = JsonSerializer.Serialize(value, options);
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = exp
                };

                await _distributedCache.SetStringAsync(fullKey, serializedValue, cacheOptions);
            }
            else if (_memoryCache != null)
            {
                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = exp
                };

                _memoryCache.Set(fullKey, value, options);
                _memoryKeys.TryAdd(fullKey, 0);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar item no cache: {Key}. Erro: {Error}", fullKey, ex.Message);
        }
    }

    public async Task RemoveAsync(string key)
    {
        var fullKey = GetFullKey(key);

        try
        {
            if (_useDistributedCache && _distributedCache != null)
            {
                await _distributedCache.RemoveAsync(fullKey);
            }
            else if (_memoryCache != null)
            {
                _memoryCache.Remove(fullKey);
                _memoryKeys.TryRemove(fullKey, out _);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover item do cache: {Key}", fullKey);
        }
    }

    public async Task RemovePatternAsync(string pattern)
    {
        try
        {
            if (_useDistributedCache && !string.IsNullOrEmpty(_redisSettings.ConnectionString))
            {
                var multiplexer = await StackExchange.Redis.ConnectionMultiplexer.ConnectAsync(_redisSettings.ConnectionString);
                try
                {
                    var endpoints = multiplexer.GetEndPoints();
                    var db = multiplexer.GetDatabase(_redisSettings.Database);
                    var serverPattern = $"{_redisSettings.KeyPrefix}{GetFullKey(pattern)}*";

                    foreach (var endpoint in endpoints)
                    {
                        var server = multiplexer.GetServer(endpoint);
                        if (!server.IsConnected) continue;

                        foreach (var key in server.Keys(_redisSettings.Database, serverPattern))
                        {
                            await db.KeyDeleteAsync(key);
                        }
                    }
                }
                finally
                {
                    await multiplexer.CloseAsync();
                }
            }
            else
            {
                if (_memoryCache == null)
                {
                    _logger.LogWarning("RemovePatternAsync: MemoryCache não disponível.");
                    return;
                }

                var fullPrefix = GetFullKey(pattern);
                var keysToRemove = _memoryKeys.Keys.Where(k => k.StartsWith(fullPrefix, StringComparison.OrdinalIgnoreCase)).ToList();
                foreach (var k in keysToRemove)
                {
                    _memoryCache.Remove(k);
                    _memoryKeys.TryRemove(k, out _);
                }
                _logger.LogInformation("RemovePatternAsync: {Count} chaves removidas do MemoryCache com prefixo {Prefix}", keysToRemove.Count, fullPrefix);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover itens do cache por padrão: {Pattern}", pattern);
        }
    }

    private string GetFullKey(string key)
    {
        return $"{_redisSettings.KeyPrefix}{key}";
    }
}
