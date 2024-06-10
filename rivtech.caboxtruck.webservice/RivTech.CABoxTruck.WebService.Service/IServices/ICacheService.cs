using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface ICacheService
    {
        T GetOrAdd<T>(string cacheKey, Func<T> factory, DateTime absoluteExpiration);
        Task<List<object>> GetKeyValueList(string type);
    }
}
