using RivTech.CABoxTruck.WebService.DTO.User;
using RivTech.CABoxTruck.WebService.Service.IServices.User;
using System;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO.Constants;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public UserInfoService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task<string> LoginAsync(LoginDto model)
        {
            dynamic clientDetails = Data.Common.Services.GetClientDetails();
            await _auditLogRepository.AddAsync(new AuditLog()
            {
                UserId = Data.Common.Services.GetCurrentUser(),
                AuditType = AuditType.Login,
                Method = "LoginAsync",
                Description = $"{model.Username} has been successfully logged in",
                KeyID = model.Username,
                OS = clientDetails.os,
                DeviceType = clientDetails.deviceType,
                Browser = clientDetails.browser,
                BrowserVersion = clientDetails.browserVersion,
                IPAddress = clientDetails.ipAddress,
                AppVersion = model.AppVersion
            });

            await _auditLogRepository.SaveChangesAsync();

            return "Ok";
        }

        public async Task<string> LogoutAsync(LoginDto model)
        {
            dynamic clientDetails = Data.Common.Services.GetClientDetails();
            await _auditLogRepository.AddAsync(new AuditLog()
            {
                UserId = Data.Common.Services.GetCurrentUser(),
                AuditType = AuditType.Logout,
                Method = "LogoutAsync",
                Description = $"{model.Username} has been successfully logged out",
                KeyID = model.Username,
                OS = clientDetails.os,
                DeviceType = clientDetails.deviceType,
                Browser = clientDetails.browser,
                BrowserVersion = clientDetails.browserVersion,
                IPAddress = clientDetails.ipAddress
            });

            await _auditLogRepository.SaveChangesAsync();
            return "Ok";
        }

        public async Task<string> FailedLoginAsync(LoginDto model)
        {
            dynamic clientDetails = Data.Common.Services.GetClientDetails();
            await _auditLogRepository.AddAsync(new AuditLog()
            {
                UserId = 0,
                AuditType = AuditType.Failed,
                Method = "FailedLoginAsync",
                Description = $"Username {model.Username} failed to login because the username/password is not valid",
                KeyID = model.Username,
                OS = model.OS,
                DeviceType = model.DeviceType,
                Browser = model.Browser,
                BrowserVersion = model.BrowserVersion,
                IPAddress = clientDetails.ipAddress
            });

            await _auditLogRepository.SaveChangesAsync();
            return "Ok";
        }

        public async Task<string> LockAccountAsync(LoginDto model)
        {
            dynamic clientDetails = Data.Common.Services.GetClientDetails();
            await _auditLogRepository.AddAsync(new AuditLog()
            {
                UserId = 0,
                AuditType = AuditType.Lock,
                Method = "LockAccountAsync",
                Description = $"Username {model.Username} is locked out after reaching the maximum failed login attempts",
                KeyID = model.Username,
                OS = model.OS,
                DeviceType = model.DeviceType,
                Browser = model.Browser,
                BrowserVersion = model.BrowserVersion,
                IPAddress = clientDetails.ipAddress
            });

            await _auditLogRepository.SaveChangesAsync();
            return "Ok";
        }

        public async Task<string> LockedAccountAsync(LoginDto model)
        {
            dynamic clientDetails = Data.Common.Services.GetClientDetails();
            await _auditLogRepository.AddAsync(new AuditLog()
            {
                UserId = 0,
                AuditType = AuditType.Locked,
                Method = "LockedAccountAsync",
                Description = $"Username {model.Username} failed to login because the account is locked",
                KeyID = model.Username,
                OS = model.OS,
                DeviceType = model.DeviceType,
                Browser = model.Browser,
                BrowserVersion = model.BrowserVersion,
                IPAddress = clientDetails.ipAddress
            });

            await _auditLogRepository.SaveChangesAsync();
            return "Ok";
        }
    }
}
