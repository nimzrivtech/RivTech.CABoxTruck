using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IPaymentPostingRepository
    {
        Task<List<PaymentPostingDTO>> GeneratePaymentPostingAsync(PaymentPostingInputDTO data);
    }
}
