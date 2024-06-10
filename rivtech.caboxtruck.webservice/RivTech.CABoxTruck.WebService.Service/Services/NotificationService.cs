using AutoMapper;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Common;
using RivTech.CABoxTruck.WebService.DALDapper.IDataAccess;
using RivTech.CABoxTruck.WebService.Data.DALDapperModels.Notification;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Notification;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.Service.IServices.Report;
using RivTech.CABoxTruck.WebService.Service.Services.Rater;
using RivTech.CABoxTruck.WebService.Service.Services.Report.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly INotificationDataAccess _notificationDataAccess;

        public NotificationService(IMapper mapper,
            INotificationDataAccess notificationDataAccess)
        {
            _mapper = mapper;
            _notificationDataAccess = notificationDataAccess;
        }
        public async Task<List<NotificationDTO>> GetNotificationsByUserId(NotificationFilterDTO filter)
        {
            var riskDetailModel = await _notificationDataAccess.GetNotificationsByUserId(filter);
            return _mapper.Map<List<NotificationDTO>>(riskDetailModel);
        }

        public async Task<Guid> CreateNotifications(NotificationFilterDTO filter)
        {
            return await _notificationDataAccess.CreateNotifications(filter);
        }

        public async Task UpdateNotificationsIsReadFlag(NotificationFilterDTO filter)
        {
            await _notificationDataAccess.UpdateNotificationsIsReadFlag(filter);
        }

        public async Task UpdateNotificationStatus(NotificationFilterDTO filter)
        {
            await _notificationDataAccess.UpdateNotificationStatus(filter);
        }

        public async Task<NotificationDTO> GetNotificationByFilter(NotificationFilterDTO filter)
        {
            var notificationModel = await _notificationDataAccess.GetNotificationByFilter(filter);

            return _mapper.Map<NotificationDTO>(notificationModel);
        }

        public async Task UpdateNotificationIsRendered(NotificationFilterDTO filter)
        {
            await _notificationDataAccess.UpdateNotificationIsRendered(filter);
        }
    }
}
