using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class BlacklistedDriverService : IBlacklistedDriverService
    {
        private readonly IMapper _mapper;
        private readonly IBlacklistedDriverRepository _blacklistedDriverRepository;

        public BlacklistedDriverService(IMapper mapper, IBlacklistedDriverRepository blacklistedDriverRepository)
        {
            _mapper = mapper;
            _blacklistedDriverRepository = blacklistedDriverRepository;
        }

        public async Task<BlacklistedDriverDTO> AddOrUpdateAsync(BlacklistedDriverDTO model)
        {
            var obj = _mapper.Map<BlacklistedDriver>(model);
            if (model?.Id == null)
            {
                var result = await _blacklistedDriverRepository.AddAsync(obj);
                await _blacklistedDriverRepository.SaveChangesAsync();

                model = _mapper.Map<BlacklistedDriverDTO>(result);
                await _blacklistedDriverRepository.SaveChangesAsync();
            }
            else
            {
                var result = (await _blacklistedDriverRepository.GetAsync(o => o.Id == obj.Id)).FirstOrDefault();
                result.FirstName = obj.FirstName;
                result.LastName = obj.LastName;
                result.DriverLicenseNumber = obj.DriverLicenseNumber;
                result.IsActive = obj.IsActive;
                result.Comments = obj.Comments;
                _blacklistedDriverRepository.Update(result);
                await _blacklistedDriverRepository.SaveChangesAsync();
            }

            return model;
        }

        public async Task<IEnumerable<BlacklistedDriverDTO>> SearchDriver(string driverName, string driverLicenseNumber)
        {
            var result = _blacklistedDriverRepository.GetAll();

            if (!string.IsNullOrEmpty(driverName))
            {
                result = result.Where(driver => driver.FirstName.Contains(driverName) || driver.LastName.Contains(driverName));
            }

            if (!string.IsNullOrEmpty(driverLicenseNumber))
            {
                result = result.Where(driver => driver.DriverLicenseNumber.Contains(driverLicenseNumber));
            }

            var list = await result.ToListAsync();
            return list.OrderBy(x => x.FirstName).Select(s => _mapper.Map<BlacklistedDriverDTO>(s)).ToList();
        }

        public async Task<bool> ValidateDriver(string driverName, string driverLicenseNumber)
        {
            return await _blacklistedDriverRepository.AnyAsync(driver => driver.LastName.Trim().ToLower() == driverName.Trim().ToLower() && 
                driver.DriverLicenseNumber.Trim().ToLower() == driverLicenseNumber.Trim().ToLower() && driver.IsActive);
        }
    }
}
