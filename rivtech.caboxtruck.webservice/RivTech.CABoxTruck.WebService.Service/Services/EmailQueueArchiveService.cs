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
    public class EmailQueueArchiveService : IEmailQueueArchiveService
    {
        private readonly IEmailQueueArchiveRepository _emailQueueArchiveRepository;
        private readonly IMapper _mapper;

        public EmailQueueArchiveService(IEmailQueueArchiveRepository emailQueueArchiveRepository, IMapper mapper)
        {
            _emailQueueArchiveRepository = emailQueueArchiveRepository;
            _mapper = mapper;
        }

        public async Task InsertEmailQueueArchiveAsync(EmailQueueArchiveDTO emailQueueArchiveDTO)
        {
            var emailQueueArchive = _mapper.Map<EmailQueueArchive>(emailQueueArchiveDTO);
            emailQueueArchive.IsSent = true;

            await _emailQueueArchiveRepository.AddAsync(emailQueueArchive);
            await _emailQueueArchiveRepository.SaveChangesAsync();
        }

        public async Task UpdateEmailQueueArchiveAsync(EmailQueueArchiveDTO emailQueueArchiveDTO)
        {
            var emailQueueArchive = _mapper.Map<EmailQueueArchive>(emailQueueArchiveDTO);
            _emailQueueArchiveRepository.Update(emailQueueArchive);
            await _emailQueueArchiveRepository.SaveChangesAsync();
        }

        public async Task DeleteEmailQueueArchiveAsync(EmailQueueArchiveDTO emailQueueArchiveDTO)
        {
            var emailQueueArchive = _mapper.Map<EmailQueueArchive>(emailQueueArchiveDTO);
            _emailQueueArchiveRepository.Remove(emailQueueArchive);
            await _emailQueueArchiveRepository.SaveChangesAsync();
        }
    }
}
