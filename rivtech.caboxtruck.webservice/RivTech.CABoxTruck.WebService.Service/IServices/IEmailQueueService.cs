using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IEmailQueueService
    {
        Task<EmailQueueDTO> InsertEmailQueueAsync(EmailQueueDTO emailQueueDTO);
        Task UpdateEmailQueueAsync(EmailQueueDTO emailQueueDTO);
        Task DeleteEmailQueueAsync(EmailQueueDTO emailQueueDTO);
        Task<List<EmailQueueDTO>> GetAllAsync();
        Task<EmailQueueDTO> GetEmailQueueAsync(Guid id);
    }
}
