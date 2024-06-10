using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RivTech.CABoxTruck.WebService.Constants;
using RivTech.CABoxTruck.WebService.Data.Common;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Errors;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILog4NetService _log4NetService;
        private readonly IHostEnvironment _env;
        private readonly ILogger _logger;

        public IConfiguration Configuration { get; }

        public ErrorLoggingMiddleware(
            RequestDelegate next, 
            ILog4NetService log4NetService, 
            IConfiguration configuration, 
            IHostEnvironment env,
            ILogger<ErrorLoggingMiddleware> logger
        )
        {
            _log4NetService = log4NetService;
            _next = next;
            _env = env;
            _logger = logger;
            Configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = _env.IsDevelopment()
                ? new ExceptionResponse((int)HttpStatusCode.InternalServerError, exception.Message, exception.StackTrace.ToString())
                : ErrorMessageHandler(exception);

            await LogErrorToDatabase(response.ReferenceNumber, context, exception);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(response.ToString());
            _logger.LogError(exception, $"{exception.Message} | UserId: {{userId}}", Services.GetCurrentUser());
        }

        private async Task LogErrorToDatabase(int refNum, HttpContext context, Exception exception)
        {
            //Get Error Code
            int errorCode = 0;
            if (!(exception is Win32Exception w32ex)) w32ex = exception.InnerException as Win32Exception;
            if (w32ex != null) errorCode = w32ex.ErrorCode;

            ErrorLogDTO errLog = new ErrorLogDTO
            {
                UserId = Data.Common.Services.GetCurrentUser(),
                ErrorMessage = exception.Message,
                ErrorCode = errorCode != 0 ? errorCode.ToString() : null,
                JsonMessage = exception.ToString(),
                Action = context.Request.Path.Value,
                Method = context.Request.Method,
                CreatedDate = DateTime.Now
            };

            await _log4NetService.ErrorAsync(errLog);
        }

        private Exception GetInnerMostException(Exception ex)
        {
            Exception currentEx = ex;
            while (currentEx.InnerException != null)
            {
                currentEx = currentEx.InnerException;
            }

            return currentEx;
        }

        private ExceptionResponse ErrorMessageHandler(Exception exception)
        {
            ExceptionResponse response = new ExceptionResponse((int)HttpStatusCode.InternalServerError);

            var errorName = exception.GetType().Name.ToString();

            switch (errorName)
            {
                case nameof(NotImplementedException):
                    response.StatusCode = (int)HttpStatusCode.NotImplemented;
                    response.Message = ExceptionMessages.NOIMPLEMENTEDEXCEPTION;
                    break;
                case nameof(NotSupportedException):
                    response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                    response.Message = ExceptionMessages.NOIMPLEMENTEDEXCEPTION;
                    break;
                case nameof(ArgumentException):
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = ExceptionMessages.ARGUMENTEXCEPTION;
                    break;
                case nameof(NullReferenceException):
                    response.Message = ExceptionMessages.NULLREFERENCEEXCEPTION;
                    break;
                case nameof(AuthenticationException):
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = ExceptionMessages.AUTHENTICATIONEXCEPTION;
                    break;
                case nameof(UnauthorizedAccessException):
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    response.Message = ExceptionMessages.UNAUTHORIZEDEXCEPTION;
                    break;
                case nameof(DbUpdateConcurrencyException):
                case nameof(DbUpdateException):
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = ExceptionMessages.DBUPDATEEXCEPTION;
                    break;
                case nameof(DbException):
                    response.Message = ExceptionMessages.DBEXCEPTION;
                    break;
                case nameof(IndexOutOfRangeException):
                    response.Message = ExceptionMessages.INDEXOUTOFRANGEEXCEPTION;
                    break;
                case nameof(IOException):
                    response.Message = ExceptionMessages.IOEXCEPTION;
                    break;
                case nameof(WebException):
                    response.Message = ExceptionMessages.WEBEXCEPTION;
                    break;
                case nameof(SqlException):
                    response.Message = ExceptionMessages.SQLEXCEPTION;
                    break;
                case nameof(StackOverflowException):
                    response.StatusCode = (int)HttpStatusCode.RequestEntityTooLarge;
                    response.Message = ExceptionMessages.STACKOVERFLOWEXCEPTION;
                    break;
                case nameof(OutOfMemoryException):
                    response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                    response.Message = ExceptionMessages.OUTOFMEMORYEXCEPTION;
                    break;
                case nameof(InvalidCastException):
                    response.Message = ExceptionMessages.INVALIDCASTEXCEPTION;
                    break;
                case nameof(InvalidOperationException):
                    response.Message = ExceptionMessages.INVALIDOPERATIONEXCEPTION;
                    break;
                case nameof(ObjectDisposedException):
                    response.Message = ExceptionMessages.OBJECTDISPOSEDEXCEPTION;
                    break;
                case nameof(ModelNotFoundException):
                    response.Message = exception.Message;
                    break;
                case nameof(ObjectAlreadyExistsException):
                    response.Message = ExceptionMessages.OBJECT_APREADY_EXISTS;
                    break;
                default:
                    response.Message = ExceptionMessages.EXCEPTION;
                    break;
            }

            return response;
        }
    }
}
