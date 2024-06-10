using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Notification;
using RivTech.CABoxTruck.WebService.DTO.Report;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface INotificationService
    {
        Task<List<NotificationDTO>> GetNotificationsByUserId(NotificationFilterDTO filter);
        Task<Guid> CreateNotifications(NotificationFilterDTO filter);
        Task UpdateNotificationsIsReadFlag(NotificationFilterDTO filter);
        Task UpdateNotificationStatus(NotificationFilterDTO filter);
        Task<NotificationDTO> GetNotificationByFilter(NotificationFilterDTO filter);
        Task UpdateNotificationIsRendered(NotificationFilterDTO filter);
    }
}
