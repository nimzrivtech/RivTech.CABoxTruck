using AutoMapper;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class EndorsementChangesService : IEndorsementChangesService
    {
        private readonly IEndorsementChangesRepository _endorsementChangesRepository;
        private readonly IEndorsementChangeDeserializer _endorsementChangeDeserializer;
        private readonly IRiskRepository _riskRepository;
        private readonly IMapper _mapper;

        public EndorsementChangesService(
            IEndorsementChangesRepository endorsementChangesRepository,
            IEndorsementChangeDeserializer endorsementChangeDeserializer,
            IRiskRepository riskRepository,
            IVehicleRepository vehicleRepository,
            IMapper mapper
        )
        {
            _endorsementChangesRepository = endorsementChangesRepository;
            _endorsementChangeDeserializer = endorsementChangeDeserializer;
            _riskRepository = riskRepository;
            _mapper = mapper;
        }

        public async Task InsertEndorsementChangesAsync(IEndorsementChangeCollection endorsementChangesCollection, Guid riskId, int endorsementNumber)
        {
            EndorsementChanges endorsementChanges = new EndorsementChanges()
            {
                Id = Guid.NewGuid(),
                RiskId = riskId,
                EndorsementNumber = endorsementNumber,
                Changes = JsonConvert.SerializeObject(endorsementChangesCollection, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            };

            await _endorsementChangesRepository.AddAsync(endorsementChanges);
            await _endorsementChangesRepository.SaveChangesAsync();
        }

        public async Task UpdateEndorsementChangesAsync(IEndorsementChangeCollection endorsementChangesCollection, Guid riskId, int endorsementNumber)
        {
            var endorsementChange = await _endorsementChangesRepository.FindAsync(x => x.RiskId == riskId && x.EndorsementNumber == endorsementNumber); ;
            if (endorsementChange is null)
            {
                endorsementChange = await _endorsementChangesRepository.AddAsync(new EndorsementChanges
                {
                    EndorsementNumber = endorsementNumber,
                    RiskId = riskId,
                    Changes = null
                });
                await _endorsementChangesRepository.SaveChangesAsync();
            }

            var savedChange = await _endorsementChangesRepository.FindAsync(endorsementChange.Id);
            savedChange.EndorsementNumber = endorsementChange.EndorsementNumber;
            savedChange.RiskId = endorsementChange.RiskId;
            savedChange.Changes = JsonConvert.SerializeObject(endorsementChangesCollection, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            await _endorsementChangesRepository.SaveChangesAsync();
        }

        public async Task DeleteEndorsementChangesAsync(EndorsementChanges endorsementChanges)
        {
            _endorsementChangesRepository.Remove(endorsementChanges);
            await _endorsementChangesRepository.SaveChangesAsync();
        }

        public async Task<EndorsementChanges> GetEndorsementChangeAsync(Guid id)
        {
            var result = await _endorsementChangesRepository.FindAsync(x => x.Id == id);
            return result;
        }
        public async Task<EndorsementChanges> GetCurrent(Guid riskId)
        {
            var items = await GetEndorsementChangesAsync(riskId);
            return items.OrderByDescending(x => x.EndorsementNumber).FirstOrDefault();
        }

        public async Task<IEnumerable<EndorsementChanges>> GetAllAsync()
        {
            var result = await _endorsementChangesRepository.GetAllAsync();
            return result;
        }

        public async Task<IEnumerable<EndorsementChanges>> GetEndorsementChangesAsync(Guid riskId)
        {
            var result = await _endorsementChangesRepository.GetAsync(x => x.RiskId == riskId);
            return result;
        }

        public async Task<EndorsementChanges> GetEndorsementChangesByRiskId(Guid riskId, int endorsementNumber)
        {
            var result = await _endorsementChangesRepository.GetAsync(x => x.RiskId == riskId && x.EndorsementNumber == endorsementNumber);
            return result.FirstOrDefault();
        }

        public async Task InsertEndorsementChangeAsync(Guid riskId, IEndorsementChangeItem endorsementChangeItem)
        {
            var endorsementChange = await GetCurrent(riskId);
            if (endorsementChange is null)
                return;

            var changeCollection = _endorsementChangeDeserializer.Deserialize(endorsementChange);
            changeCollection.Add(endorsementChangeItem);
            _endorsementChangesRepository.Update(endorsementChange);
            await _endorsementChangesRepository.SaveChangesAsync();
        }

        public async Task<EndorsementChangeCollection> GetChangesByRiskAndEndorsement(Guid riskId, int endorsementNumber)
        {
            var endorsementChange = await _endorsementChangesRepository.FindAsync(x => x.RiskId == riskId && x.EndorsementNumber == endorsementNumber);
            var changeCollection = _endorsementChangeDeserializer.Deserialize(endorsementChange);
            return new EndorsementChangeCollection(changeCollection);
        }

        public async Task<EndorsementChanges> CreateNewAsync(Guid riskId, int endorsementNumber)
        {
            EndorsementChanges endorsementChanges = new EndorsementChanges()
            {
                Id = Guid.NewGuid(),
                RiskId = riskId,
                EndorsementNumber = endorsementNumber
            };

            await _endorsementChangesRepository.AddAsync(endorsementChanges);
            await _endorsementChangesRepository.SaveChangesAsync();
            return endorsementChanges;
        }

        public async Task<bool> HasVehicleChangesInCurrentEndorsementAsync(VehicleDTO vehicle, Guid riskId)
        {
            if (vehicle is null || vehicle.Id == Guid.Empty)
                throw new ArgumentException("Invalid vehicle");

            var risk = await _riskRepository.FindAsync(riskId);
            var changes = (await GetChangesByRiskAndEndorsement(riskId, risk.NextEndorsementNumber())).AsEnumerable();

            changes = changes?.Where(x => x is VehicleUpdateChangeItem || x is VehicleAddChangeItem || x is VehicleRemoveChangeItem);
            bool isUpdated = changes?.Select(x => x as VehicleUpdateChangeItem)?.FirstOrDefault(x => x?.MainId == vehicle.MainId) != null;
            bool isAdded = changes?.Select(x => x as VehicleAddChangeItem)?.FirstOrDefault(x => x?.Vehicles?.Any(v => v.VIN == vehicle.VIN) ?? false) != null;
            bool isRemoved = changes?.Select(x => x as VehicleRemoveChangeItem)?.FirstOrDefault(x => x?.MainId == vehicle.MainId) != null;

            var updateChangeItem = changes?.FirstOrDefault(x => x is VehicleUpdateChangeItem) as VehicleUpdateChangeItem;
            if(risk.NextEndorsementNumber() == 1 && isUpdated && updateChangeItem != null && (updateChangeItem.IsEndorsement1CargoChange ?? false) && (updateChangeItem.IsEndorsement1ReeferChange ?? false))
            {
                isUpdated = false;
            }

            return (isUpdated || isAdded || isRemoved);
        }
    }
}
