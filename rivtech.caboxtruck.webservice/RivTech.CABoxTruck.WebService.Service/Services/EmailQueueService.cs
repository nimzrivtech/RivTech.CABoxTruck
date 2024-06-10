using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.ExternalData.Entity;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class EmailQueueService : IEmailQueueService
    {
        private readonly IEmailQueueRepository _emailQueueRepository;
        private readonly IMapper _mapper;

        public EmailQueueService(IEmailQueueRepository emailQueueRepository, IMapper mapper)
        {
            _emailQueueRepository = emailQueueRepository;
            _mapper = mapper;
        }

        public async Task<EmailQueueDTO> InsertEmailQueueAsync(EmailQueueDTO emailQueueDTO)
        {
            emailQueueDTO.Id = Guid.NewGuid();
            var emailQueue = _mapper.Map<EmailQueue>(emailQueueDTO);

            await _emailQueueRepository.AddAsync(emailQueue);
            await _emailQueueRepository.SaveChangesAsync();

            return emailQueueDTO;
        }

        public async Task UpdateEmailQueueAsync(EmailQueueDTO emailQueueDTO)
        {
            var emailQueue = _mapper.Map<EmailQueue>(emailQueueDTO);
            _emailQueueRepository.Update(emailQueue);
            await _emailQueueRepository.SaveChangesAsync();
        }

        public async Task DeleteEmailQueueAsync(EmailQueueDTO emailQueueDTO)
        {
            var emailQueue = _mapper.Map<EmailQueue>(emailQueueDTO);
            _emailQueueRepository.Remove(emailQueue);
            await _emailQueueRepository.SaveChangesAsync();
        }
        public async Task<EmailQueueDTO> GetEmailQueueAsync(Guid id)
        {
            var result = await _emailQueueRepository.FindAsync(x => x.Id == id && !x.IsSent);
            return _mapper.Map<EmailQueueDTO>(result);
        }
        public async Task<List<EmailQueueDTO>> GetAllAsync()
        {
            var result = await _emailQueueRepository.GetAsync(x => !x.IsSent);
            return _mapper.Map<List<EmailQueueDTO>>(result);
        }
    }
}
