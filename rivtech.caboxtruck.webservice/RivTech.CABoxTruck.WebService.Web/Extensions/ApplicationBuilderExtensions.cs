using Microsoft.AspNetCore.Builder;
using RivTech.CABoxTruck.WebService.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Extensions
{
	public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseErrorLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorLoggingMiddleware>();
        }
        public static IApplicationBuilder UseRateLimit(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimiterMiddleware>();
        }
    }
}
