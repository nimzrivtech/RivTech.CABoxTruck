using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class ReasonMoveService : IReasonMoveService
    {
        private readonly IReasonMoveRepository _repository;

        private readonly IMapper _mapper;

        public ReasonMoveService(IReasonMoveRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ReasonMoveDTO>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            return _mapper.Map<List<ReasonMoveDTO>>(result);
        }

        public async Task<bool> IsExistAsync(Int16 id)
        {
            return await _repository.AnyAsync(x => x.Id == id);
        }

        public async Task<ReasonMoveDTO> GetByIdAsync(Int16 id)
        {
            var result = await _repository.FindAsync(x => x.Id == id);
            return _mapper.Map<ReasonMoveDTO>(result);
        }

        public async Task<ReasonMoveDTO> InsertAsync(ReasonMoveDTO model)
        {
            var obj = _mapper.Map<LvReasonMove>(model);

            var result = await _repository.AddAsync(obj);
            await _repository.SaveChangesAsync();

            return _mapper.Map<ReasonMoveDTO>(result);
        }

        public async Task<ReasonMoveDTO> UpdateAsync(ReasonMoveDTO model)
        {
            var driver = _repository.Update(_mapper.Map<LvReasonMove>(model));
            await _repository.SaveChangesAsync();
            return _mapper.Map<ReasonMoveDTO>(driver);
        }

        public async Task<string> RemoveAsync(Int16 id)
        {
            var entity = await _repository.FindAsync(x => x.Id == id);
            entity.IsActive = false;
            await _repository.SaveChangesAsync();
            return id.ToString();
        }
    }
}
