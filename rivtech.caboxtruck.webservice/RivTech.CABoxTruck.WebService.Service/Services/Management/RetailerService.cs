using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class RetailerService : IRetailerService
    {
        private readonly IRiskManuscriptsRepository _manuscriptsRepository;
        private readonly IRetailerAgencyRepository _retailerAgencyRepository;

        private readonly IRetailerRepository _retailerRepository;
        private readonly IMapper _mapper;

        public RetailerService(IRiskManuscriptsRepository manuscriptsRepository,
            IMapper mapper, IRetailerRepository retailerRepository, IRetailerAgencyRepository retailerAgencyRepository)
        {
            _manuscriptsRepository = manuscriptsRepository;
            _mapper = mapper;
            _retailerRepository = retailerRepository;
            _retailerAgencyRepository = retailerAgencyRepository;
        }

        public async Task<List<RetailerDTO>> GetAll()
        {
            var result = await _retailerRepository.GetAsync(r => r.Entity != null, r => r.Entity);
            return _mapper.Map<List<RetailerDTO>>(result);
        }

        public async Task<List<RetailerDTO>> GetAllByAgency(Guid agencyId)
        {
            var result = await _retailerAgencyRepository.GetRetailerAsync(agencyId);
            return _mapper.Map<List<RetailerDTO>>(result.Select(x => x.Retailer).ToList());
        }

        public async Task<RetailerDTO> GetById(Guid retailerId)
        {
            var result = await _retailerRepository.GetAsync(r => r.Id == retailerId, r => r.Entity, r => r.LinkedAgencies);
            return _mapper.Map<RetailerDTO>(result.FirstOrDefault());
        }

        public async Task<RetailerDTO> InsertAsync(RetailerDTO resource)
        {
            var model = _mapper.Map<Retailer>(resource);
           
            var result = await _retailerRepository.AddAsync(model);
            foreach (var agency in result.LinkedAgencies)
            {
                agency.RetailerId = result.Id;
            }

           
            await _retailerRepository.SaveChangesAsync();
            return _mapper.Map<RetailerDTO>(result);
        }

        public async Task<RetailerDTO> UpdateAsync(RetailerDTO resource)
        {
            var model = await _retailerRepository.FindAsync(r => r.Id == resource.Id);

            model.IsActive = resource.IsActive;
            model.LinkedAgencies = _mapper.Map<List<RetailerAgency>>(resource.LinkedAgencies);
            model.Entity = _mapper.Map<Entity>(resource.Entity);

            //Unlinked agencies, cascade delete doesn't work
            var linkedAgencyIds = resource.LinkedAgencies.Select(la => la.AgencyId);
            var unlinkedAgencies = await _retailerAgencyRepository
                .GetAsync(ra => ra.RetailerId == resource.Id && !linkedAgencyIds.Contains(ra.AgencyId));

            _retailerAgencyRepository.RemoveRange(unlinkedAgencies);

            var result = _retailerRepository.Update(model);

            await _retailerRepository.SaveChangesAsync();
            return _mapper.Map<RetailerDTO>(result);
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var entity = await _retailerRepository.FindAsync(x => x.Id == id);
            entity.IsActive = false;
            _retailerRepository.Remove(entity);
            await _retailerRepository.SaveChangesAsync();
            return true;
        }

    }
}