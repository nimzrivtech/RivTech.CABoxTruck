using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class PaymentPlanService : IPaymentPlanService
    {
        private readonly IPaymentPlanRepository _paymentPlanRepository;
        private readonly IMapper _mapper;
        public PaymentPlanService(IPaymentPlanRepository paymentPlanRepository, IMapper mapper)
        {
            _paymentPlanRepository = paymentPlanRepository;
            _mapper = mapper;
        }

        public async Task<List<PaymentPlanDTO>> GetAllAsync()
        {
            var result = await _paymentPlanRepository.GetAsync(x => x.IsActive);
            return _mapper.Map<List<PaymentPlanDTO>>(result);
        }

        public async Task<PaymentPlanDTO> GetPaymentPlanAsync(byte id)
        {
            var result = await _paymentPlanRepository.FindAsync(x => x.Id == id && x.IsActive);
            return _mapper.Map<PaymentPlanDTO>(result);
        }

        public async Task<string> InsertPaymentPlanAsync(PaymentPlanDTO model)
        {
            var result = await _paymentPlanRepository.AddAsync(_mapper.Map<Data.Entity.LvPaymentPlan>(model));
            await _paymentPlanRepository.SaveChangesAsync();
            return result.Id.ToString();
        }

        public async Task<string> RemovePaymentPlanAsync(byte id)
        {
            var entity = await _paymentPlanRepository.FindAsync(x => x.Id == id && x.IsActive);
            entity.IsActive = false;
            await _paymentPlanRepository.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public async Task<string> UpdatePaymentPlanAsync(PaymentPlanDTO model)
        {
            var result = _paymentPlanRepository.Update(_mapper.Map<Data.Entity.LvPaymentPlan>(model));
            await _paymentPlanRepository.SaveChangesAsync();
            return result.Id.ToString();
        }
    }
}
