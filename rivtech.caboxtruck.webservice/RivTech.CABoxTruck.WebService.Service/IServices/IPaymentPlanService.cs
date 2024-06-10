using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IPaymentPlanService
    {
        Task<string> InsertPaymentPlanAsync(PaymentPlanDTO model);
        Task<List<PaymentPlanDTO>> GetAllAsync();
        Task<PaymentPlanDTO> GetPaymentPlanAsync(byte id);
        Task<string> UpdatePaymentPlanAsync(PaymentPlanDTO model);
        Task<string> RemovePaymentPlanAsync(byte id);
    }
}
