using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class PreBindPaymentService : IPreBindPaymentService
    {
        private readonly IPreBindPaymentRepository _preBindPaymentRepository;
        public PreBindPaymentService(IPreBindPaymentRepository preBindPaymentRepository)
        {
            _preBindPaymentRepository = preBindPaymentRepository;
        }

        public async  Task<List<PreBindPaymentDTO>> GeneratePreBindPayments(PreBindPaymentInputDTO data)
        {
            return await _preBindPaymentRepository.GeneratePreBindPaymentAsync(data); ;
        }
    }
}
