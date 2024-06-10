using System;
using System.ComponentModel;
using AutoMapper;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Data.Common;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class Log4NetService : ILog4NetService
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IErrorLogRepository _errorLogRepository;

        public Log4NetService(
            IConfiguration configuration,
            IMapper mapper,
            IAuditLogRepository auditLogRepository,
            IErrorLogRepository errorLogRepository)
        {
            // Load configuration
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            _auditLogRepository = auditLogRepository;
            _errorLogRepository = errorLogRepository;

            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task InfoAsync(AuditLogDTO auditLog)
        {
            log.Info(JsonConvert.SerializeObject(auditLog));
            var data = _mapper.Map<AuditLog>(auditLog);
            await _auditLogRepository.AddAsync(data);
            await _auditLogRepository.SaveChangesAsync();
        }

        public async Task ErrorAsync(ErrorLogDTO errorLog)
        {
            log.Error(JsonConvert.SerializeObject(errorLog));
            var data = _mapper.Map<ErrorLog>(errorLog);
            await _errorLogRepository.AddAsync(data);
            await _errorLogRepository.SaveChangesAsync();
        }

        public void Error(string errorMessage, string jsonMessage, [CallerMemberName] string callerMemberName = "")
        {
            ErrorLogDTO errLog = new ErrorLogDTO
            {
                UserId = Data.Common.Services.GetCurrentUser(),
                ErrorMessage = errorMessage,
                ErrorCode = null,
                JsonMessage = jsonMessage,
                Action = callerMemberName,
                Method = null,
                CreatedDate = DateTime.Now
            };

            log.Error(JsonConvert.SerializeObject(errLog));
            var data = _mapper.Map<ErrorLog>(errLog);

            var contextFactory = new DbContextFactory();
            var connectionString = _configuration.GetSection("ConnectionStrings").GetValue<string>("ConCABoxTruck");

            using var context = contextFactory.Create(connectionString);
            context.ErrorLog.Add(data);
            context.SaveChanges();
        }

        public void Error(Exception exception, [CallerMemberName] string callerMemberName = "")
        {
            int errorCode = GetErrorCode(exception);

            ErrorLogDTO errLog = new ErrorLogDTO
            {
                UserId = Data.Common.Services.GetCurrentUser(),
                ErrorMessage = exception.Message,
                ErrorCode = errorCode != 0 ? errorCode.ToString() : null,
                JsonMessage = exception.ToString(),
                Action = callerMemberName,
                Method = null,
                CreatedDate = DateTime.Now
            };

            log.Error(JsonConvert.SerializeObject(errLog));
            var data = _mapper.Map<ErrorLog>(errLog);

            var contextFactory = new DbContextFactory();
            var connectionString = _configuration.GetSection("ConnectionStrings").GetValue<string>("ConCABoxTruck");

            using var context = contextFactory.Create(connectionString);
            context.ErrorLog.Add(data);
            context.SaveChanges();
        }

        private int GetErrorCode(Exception ex)
        {
            int errorCode = 0;

            if (!(ex is Win32Exception w32ex)) w32ex = ex.InnerException as Win32Exception;
            if (w32ex != null) errorCode = w32ex.ErrorCode;

            return errorCode;
        }
    }
}
