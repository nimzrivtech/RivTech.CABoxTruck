using Microsoft.Extensions.Caching.Memory;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class CacheService : ICacheService
    {
        public enum Duration
        {
            TenSeconds = 10, // 10 seconds  - used for testing
            OneMinute = 60,
            FiveMinutes = 60 * 5, // 60sec * 5 = 5 minutes
            TenMinutes = 60 * 10,
            ThirtyMinutes = 60 * 30,
            OneHour = 60 * 60,
            ThreeHours = 60 * 60 * 3,
            OneDay = 60 * 60 * 24,
            FiveDays = 60 * 60 * 24 * 5,
            FifteenDays = 60 * 60 * 24 * 15,
            ThirtyDays = 60 * 60 * 24 * 30,
            Zero = int.MaxValue,
            ThisDay = 60000
        }

        private readonly IKeyValueRepository _keyValueRepository;
        private readonly IMemoryCache _memoryCache;

        public CacheService(IKeyValueRepository keyValueRepository, IMemoryCache memoryCache)
        {
            _keyValueRepository = keyValueRepository;
            _memoryCache = memoryCache;
        }

        public T GetOrAdd<T>(string cacheKey, Func<T> factory, DateTime absoluteExpiration)
        {
            throw new NotImplementedException();
        }

        public async Task<List<object>> GetKeyValueList(string type)
        {
            var sCacheName = $"GetKeyValueList_{type.ToUpper()}";
            if (GetCache(sCacheName) is List<object> list) return list;
            var assembly = typeof(GenericEnumerationType).Assembly;

            var genericType = assembly.GetType($"{assembly.GetName().Name}.Entity.DataSets.{type}");

            if (genericType == null) throw new Exception();

            var method = typeof(IKeyValueRepository).GetMethod(nameof(IKeyValueRepository.Retrieve));
            var generic = method.MakeGenericMethod(genericType);

            var keyValueList = generic.Invoke(_keyValueRepository, null) as IEnumerable<object>;
            
            list = (keyValueList ?? throw new InvalidOperationException()).ToList();

            SetCache(sCacheName, list, Duration.ThisDay, () => { GetKeyValueList(type); });
            return list;
        }

        public DateTime GetDateEndOfDay()
        {
            var dateExpiration = DateTime.Now;
            return new DateTime(dateExpiration.Year, dateExpiration.Month, dateExpiration.Day).AddDays(1);
        }

        public object GetCache(string key)
        {
            if (_memoryCache.TryGetValue<object>(key, out var result))
                return result;

            return null;
        }

        public object SetCache(string sCacheName, object obj, Duration duration = Duration.Zero,
            Action callbackWhenExpired = null)
        {
            if (obj != null)
            {
                var dateExpiration = DateTime.Now;
                switch (duration)
                {
                    case Duration.Zero:
                        dateExpiration = DateTime.MaxValue;
                        break;
                    case Duration.ThisDay:
                        dateExpiration = GetDateEndOfDay();
                        break;
                    default:
                        dateExpiration = dateExpiration.AddSeconds((int)duration);
                        break;
                }

                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = dateExpiration,
                    //SlidingExpiration = TimeSpan.Zero,
                    Priority = CacheItemPriority.Normal
                }.RegisterPostEvictionCallback(
                    (key, value, reason, substate) =>
                    {
                        if (reason.Equals(EvictionReason.Expired))
                            callbackWhenExpired?.Invoke();
                    }
                );

                _memoryCache.Set(sCacheName, obj, options);
            }
            else
            {
                RemoveCache(sCacheName);
            }

            return obj;
        }

        public T GetCache<T>(string key)
        {
            if (_memoryCache.TryGetValue<T>(key, out var result))
                return result;

            return default(T);
        }

        public bool RemoveCache(string key)
        {
            var isCacheRemoved = false;
            try
            {
                _memoryCache.Remove(key);
                isCacheRemoved = true;
            }
            catch (Exception)
            {
                // ignored
            }

            return isCacheRemoved;
        }
    }
}
