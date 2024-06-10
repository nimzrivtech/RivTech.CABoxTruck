using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Emails;
using RivTech.CABoxTruck.WebService.ExternalData.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Emails
{
    public interface IEndorsementEmailService
    {
        Task<EndorsementEmailContentData> CreateContentData(Guid riskId);
        Task<EmailQueue> CreateEmailQueueAsync(EndorsementEmailContentData emailContentData);
    }
}
