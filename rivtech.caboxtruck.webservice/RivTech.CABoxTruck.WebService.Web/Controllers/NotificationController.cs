using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Common;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Notification;
using RivTech.CABoxTruck.WebService.DTO.Report;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.Service.IServices.Report;
using RivTech.CABoxTruck.WebService.Service.Services;
using RivTech.CABoxTruck.WebService.Service.Services.AzureFunctions;
using RivTech.CABoxTruck.WebService.Service.Services.Billing.Reports;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly long _userId = -1;

        public NotificationController(
            INotificationService notificationService
        )
        {
            _notificationService = notificationService;
            _userId = Services.GetCurrentUser();
        }

        [HttpPost("GetNotificationsByUserId")]
        [Produces(typeof(List<NotificationDTO>))]
        public async Task<IActionResult> GetNotificationsByUserId([FromBody] NotificationFilterDTO filter)
        {
            try
            {
                filter.UserId = _userId;

                return Ok(await _notificationService.GetNotificationsByUserId(filter));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("CreateNotifications")]
        [Produces(typeof(List<NotificationDTO>))]
        public async Task<IActionResult> CreateNotifications([FromBody] NotificationFilterDTO filter)
        {
            try
            {
                filter.UserId = _userId;

                await _notificationService.CreateNotifications(filter);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("UpdateNotificationsIsReadFlag")]
        [Produces(typeof(List<NotificationDTO>))]
        public async Task<IActionResult> UpdateNotificationsIsReadFlag([FromBody] NotificationFilterDTO filter)
        {
            try
            {
                filter.UserId = _userId;

                await _notificationService.UpdateNotificationsIsReadFlag(filter);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("GetNotificationByFilter")]
        [Produces(typeof(NotificationDTO))]
        public async Task<IActionResult> GetNotificationByFilter([FromBody] NotificationFilterDTO filter)
        {
            try
            {
                filter.UserId = _userId;

                return Ok(await _notificationService.GetNotificationByFilter(filter));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("UpdateNotificationIsRendered")]
        [Produces(typeof(NotificationDTO))]
        public async Task<IActionResult> UpdateNotificationIsRendered([FromBody] NotificationFilterDTO filter)
        {
            try
            {
                filter.UserId = _userId;

                await _notificationService.UpdateNotificationIsRendered(filter);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}