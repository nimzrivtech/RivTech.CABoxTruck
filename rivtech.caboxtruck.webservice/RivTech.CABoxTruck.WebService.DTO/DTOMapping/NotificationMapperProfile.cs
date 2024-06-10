using AutoMapper;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Common;
using RivTech.CABoxTruck.WebService.Common.ReportTemplates;
using RivTech.CABoxTruck.WebService.Data.DALDapperModels;
using RivTech.CABoxTruck.WebService.Data.DALDapperModels.Notification;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO.Notification;
using RivTech.CABoxTruck.WebService.DTO.Report;
using System.Globalization;

namespace RivTech.CABoxTruck.WebService.DTO.DTOMapping
{
    public class NotificationMapperProfile : Profile
    {
        public NotificationMapperProfile()
        {

            CreateMap<RivTech.CABoxTruck.WebService.Data.Entity.Notification, NotificationFilterDTO>().ReverseMap();
            CreateMap<UserNotification, NotificationFilterDTO>().ReverseMap();
            CreateMap<NotificationDTO, NotificationModel>();
            CreateMap<NotificationModel, NotificationDTO>()
                .ForMember(dest => dest.ErrorMessagesList, opts => opts.MapFrom(src => !string.IsNullOrWhiteSpace(src.ErrorMessages) ? JsonConvert.DeserializeObject<string[]>(src.ErrorMessages) : null));
        }
    }
}
