using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RivTech.CABoxTruck.WebService.Constants;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Data.Common;
using System.Net.Http;
using System.Collections.Concurrent;
using System.Timers;
using log4net.Core;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace RivTech.CABoxTruck.WebService.Middlewares
{
    public class RateLimiterMiddleware : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILog4NetService _log4NetService;
        private readonly int _defaultLimit;
        private readonly int _cooldownPeriod;
        private readonly int _cooldownRequestCount;
        private readonly string _unit;
        private readonly RequestDelegate _next;
        private readonly ConcurrentDictionary<string, RateLimitData> _rateLimits;
        private readonly ConcurrentDictionary<string, CustomRateLimit> _rateLimitCustom;
        public RateLimiterMiddleware(RequestDelegate next, ILog4NetService log4NetService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _rateLimits = new ConcurrentDictionary<string, RateLimitData>();
            _rateLimitCustom = new ConcurrentDictionary<string, CustomRateLimit>();
            _cooldownRequestCount = Convert.ToInt32(Environment.GetEnvironmentVariable("RateLimitPeriod") ?? configuration["RateLimitPeriod"] ?? "10");
            _log4NetService = log4NetService;
            _cooldownPeriod = Convert.ToInt32(Environment.GetEnvironmentVariable("RateLimitPeriodReset") ?? configuration["RateLimitPeriodReset"] ?? "60");
            _defaultLimit = Convert.ToInt32(Environment.GetEnvironmentVariable("RateLimit") ?? configuration["RateLimit"] ?? "25");
            string rateLimitCustom = Environment.GetEnvironmentVariable("RateLimitCustom") ?? configuration["RateLimitCustom"];
            
            if(rateLimitCustom != null && rateLimitCustom != "")
            {
                // Parse the JSON string into a JArray
                JArray jsonArray = JArray.Parse(rateLimitCustom);

                foreach (var endpoints in jsonArray)
                {
                    // Access the first element of the array
                    JObject jsonObject = (JObject)endpoints;

                    // Access properties from the JObject
                    string endpoint = (string)jsonObject["Endpoint"];
                    System.Diagnostics.Debug.WriteLine(endpoint);
                    int limit = (int)jsonObject["Limit"];
                    _rateLimitCustom.TryAdd(endpoint, new CustomRateLimit(endpoint, limit));
                }
            }
            _unit = Environment.GetEnvironmentVariable("RateLimitUnit") ?? configuration["RateLimitUnit"] ?? "s";
            _httpContextAccessor = httpContextAccessor;
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            // Apply CORS headers
            if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            }
            if (context.Request.Method == "OPTIONS")
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.CompleteAsync();
                return;
            }
            
            var request = _httpContextAccessor.HttpContext.Request;
            var segments = request.Path.Value.Split('/');
            string responseLimit = _defaultLimit + _unit;

            var endpoint = string.Join("/", segments.Take(4));
            // Get the client's IP address4
            var ipAddress = context.Connection.RemoteIpAddress.ToString();

            // Get or create rate limit data for this endpoint
            var rateLimitData = _rateLimits.GetOrAdd($"{endpoint}_{ipAddress}", key =>
            {
                var split = key.Split('_');
                var ep = split[0];
                var ip = split[1];
                CustomRateLimit rateLimit = _rateLimitCustom.Values.FirstOrDefault(ep => ep.EndPoint == endpoint);
                responseLimit = (rateLimit != null ? rateLimit.Limit : _defaultLimit) + _unit;
                return new RateLimitData(ep, ip, rateLimit != null ? rateLimit.Limit : _defaultLimit, _cooldownRequestCount, _cooldownPeriod);
            });
            rateLimitData.Limit -= 1;
            DateTime? validUntil = DateTime.Now.AddSeconds(_cooldownPeriod);
            double createdDateDifference = (DateTime.Now - rateLimitData.CreatedDate).TotalSeconds;
            switch (_unit)
            {
                case "m":
                    createdDateDifference = (DateTime.Now - rateLimitData.CreatedDate).TotalMinutes;
                    validUntil = DateTime.Now.AddMinutes(_cooldownPeriod);
                    break;
                case "h":
                    createdDateDifference = (DateTime.Now - rateLimitData.CreatedDate).TotalHours;
                    validUntil = DateTime.Now.AddHours(_cooldownPeriod);
                    break;
            }
            // Check if the client has exceeded the rate limit for this IP address
            if (rateLimitData.Limit > 0 && createdDateDifference > _cooldownRequestCount)
            {
                if (!rateLimitData.ExceededLimitDT.HasValue)
                {
                    rateLimitData.ExceededLimitDT = null;
                    CustomRateLimit rateLimit = _rateLimitCustom.Values.FirstOrDefault(ep => ep.EndPoint == endpoint);
                    rateLimitData.Limit = rateLimit != null ? rateLimit.Limit : _defaultLimit;
                    rateLimitData.CreatedDate = DateTime.Now;
                }
            }

            // if (!context.Response.Headers.ContainsKey("X-Rate-Limit"))
            // {
            //     context.Response.Headers.Add("X-Rate-Limit", responseLimit);
            //     context.Response.Headers.Add("X-Rate-Limit-Remaining", rateLimitData.Limit.ToString());
            //     context.Response.Headers.Add("X-Rate-Limit-Reset", validUntil?.Hour + ":" + validUntil?.Minute + ":" + validUntil?.Second);
            // }
            // else
            // {
            //     context.Response.Headers["X-Rate-Limit-Remaining"] = rateLimitData.Limit.ToString();
            //     context.Response.Headers["X-Rate-Limit-Reset"] = validUntil?.Hour + ":" + validUntil?.Minute + ":" + validUntil?.Second;
            //     context.Response.Headers["X-Rate-Limit"] = responseLimit;
            // }

            if (rateLimitData.Limit <= 0)
            {
                if (!rateLimitData.ExceededLimitDT.HasValue)
                {
                    rateLimitData.ExceededLimitDT = DateTime.Now;
                }
                double? diffLimit = (DateTime.Now - rateLimitData.ExceededLimitDT)?.TotalSeconds;
                validUntil = rateLimitData.ExceededLimitDT?.AddSeconds(_cooldownPeriod);
                switch (_unit)
                {
                    case "h":
                        diffLimit = (DateTime.Now - rateLimitData.ExceededLimitDT)?.TotalHours;
                        validUntil = rateLimitData.ExceededLimitDT?.AddHours(_cooldownPeriod);
                        break;
                    case "m":
                        diffLimit = (DateTime.Now - rateLimitData.ExceededLimitDT)?.TotalMinutes;
                        validUntil = rateLimitData.ExceededLimitDT?.AddMinutes(_cooldownPeriod);
                        break;
                }
                if (_cooldownPeriod - diffLimit <= 0)
                {
                    rateLimitData.ExceededLimitDT = null;
                    CustomRateLimit rateLimit = _rateLimitCustom.Values.FirstOrDefault(ep => ep.EndPoint == endpoint);
                    rateLimitData.Limit = rateLimit != null ? rateLimit.Limit : _defaultLimit;
                    rateLimitData.CreatedDate = DateTime.Now;
                } else
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(ExceptionMessages.RATE_LIMIT_EXCEED);
                    ErrorLogDTO errLog = new ErrorLogDTO
                    {
                        UserId = Data.Common.Services.GetCurrentUser(),
                        ErrorMessage = ExceptionMessages.RATE_LIMIT_EXCEED,
                        ErrorCode = "429",
                        JsonMessage = new RaterException(ExceptionMessages.RATE_LIMIT_EXCEED).ToString(),
                        Action = endpoint,
                        Method = context.Request.Method,
                        CreatedDate = DateTime.Now
                    };
                    await _log4NetService.ErrorAsync(errLog);
                    return;
                }
            }

            await _next(context);
        }
    }
    public class RateLimitData
    {
        public RateLimitData(string ipAddress, string endpoint, int limit, int cooldownRequestCount, int cooldownPeriod)
        {
            EndPoint = endpoint;
            IpAddress = ipAddress;
            Limit = limit;
            CooldownRequestCount = cooldownRequestCount;
            CooldownPeriod = cooldownPeriod;
            CreatedDate = DateTime.Now;
        }

        public string EndPoint { get; }
        public int Limit { get; set; }
        public int CooldownRequestCount { get; }
        public int CooldownPeriod { get; }
        public string IpAddress { get; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExceededLimitDT { get; set; }
    }
    public class CustomRateLimit
    {
        public CustomRateLimit(string endpoint, int limit)
        {
            EndPoint = endpoint;
            Limit = limit;
        }

        public string EndPoint { get; }
        public int Limit { get; set; }
    }
}
