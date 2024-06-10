using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO.Notes;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Data.Common;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class RiskNotesService : IRiskNotesService
    {
        private readonly IMapper _mapper;
        private readonly IRiskNotesRepository _riskNotesRepository;

        public RiskNotesService(IRiskNotesRepository riskNotesRepository, IMapper mapper)
        {
            _riskNotesRepository = riskNotesRepository;
            _mapper = mapper;
        }

        public async Task<List<RiskNotesDTO>> GetByRiskIdAsync(Guid riskDetailId)
        {
            var result = await _riskNotesRepository.GetAsync(rf => rf.RiskDetailId == riskDetailId && rf.IsActive, i => i.NoteCategoryOption);
            return _mapper.Map<List<RiskNotesDTO>>(result);
        }

        public async Task<RiskNotesDTO> InsertorUpdateNotesAsync(RiskNotesDTO model)
        {
            RiskNotes result = null;
            if (!model.Id.HasValue)
            {
                result = await _riskNotesRepository.AddAsync(new RiskNotes()
                {
                    RiskDetailId = model.RiskDetailId,
                    CategoryId = model.CategoryId,
                    Description = model.Description,
                    CreatedBy = Data.Common.Services.GetCurrentUser(), 
                    IsActive = true,
                    SourcePage = model.SourcePage
                });
            }
            else
            {
                var note = await _riskNotesRepository.FindAsync(x => x.Id == model.Id);
                note.CategoryId = model.CategoryId;
                note.Description = model.Description;
                result = _riskNotesRepository.Update(note);
            }

            await _riskNotesRepository.SaveChangesAsync();

            result = await _riskNotesRepository.FindAsync(x => x.Id == result.Id, i => i.NoteCategoryOption);

            return _mapper.Map<RiskNotesDTO>(result);
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var riskNote = await _riskNotesRepository.FindAsync(rf => rf.Id == id);
            riskNote.IsActive = false;
            riskNote.DeletedDate = DateTime.Now;
            var result = _riskNotesRepository.Update(riskNote);
            await _riskNotesRepository.SaveChangesAsync();
            return true;
        }
    }
}