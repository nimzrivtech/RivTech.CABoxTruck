using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Dynamic;
using System.Net.Http.Headers;

namespace RivTech.CABoxTruck.WebService.Data.Common
{
    public class Services
    {
        public static IHttpContextAccessor HttpContextAccessor;

        public static long GetCurrentUser()
        {
            var authenticatedUser = 0L;

            if (HttpContextAccessor == null)
                return authenticatedUser;

            var authenticatedUserId = HttpContextAccessor.HttpContext?.User
                .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (authenticatedUserId != null)
                authenticatedUser = long.Parse(authenticatedUserId);

            return authenticatedUser;
        }

        public static string GetAuthToken()
        {
            if (HttpContextAccessor.HttpContext != null)
            {
                if (AuthenticationHeaderValue.TryParse(HttpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization], out var headerValue))
                    return headerValue.Parameter;
            }
            return null;
        }

        public static DateTime? GetUserDate()
        {
            if (HttpContextAccessor.HttpContext != null)
            {
                if (DateTime.TryParse(HttpContextAccessor.HttpContext.Request.Headers["userdate"], out DateTime uiSelectedDate))
                {
                    return uiSelectedDate;
                }
            }

            return DateTime.Now;
        }

        public static object GetClientDetails()
        {
            dynamic obj = new ExpandoObject();
            obj.os = HttpContextAccessor.HttpContext?.User.FindFirst("os")?.Value;
            obj.deviceType = HttpContextAccessor.HttpContext?.User.FindFirst("device_type")?.Value;
            obj.browser = HttpContextAccessor.HttpContext?.User.FindFirst("browser")?.Value;
            obj.browserVersion = HttpContextAccessor.HttpContext?.User.FindFirst("browser_version")?.Value;
            obj.ipAddress = HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            return obj;
        }
    }
}
