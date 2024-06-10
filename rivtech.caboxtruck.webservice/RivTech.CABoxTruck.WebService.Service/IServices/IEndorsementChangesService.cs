using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IEndorsementChangesService
    {
        Task InsertEndorsementChangesAsync(IEndorsementChangeCollection endorsementChangesCollection, Guid riskId, int endorsementNumber);
        /// <summary>
        /// Insert a new change item to existing EndorsementChanges record. Throws exception if not 
        /// </summary>
        Task InsertEndorsementChangeAsync(Guid riskId, IEndorsementChangeItem endorsementChangeItem);
        Task<EndorsementChanges> CreateNewAsync(Guid riskId, int endorsementNumber);
        Task UpdateEndorsementChangesAsync(IEndorsementChangeCollection endorsementChangesCollection, Guid riskId, int endorsementNumber);
        Task DeleteEndorsementChangesAsync(EndorsementChanges endorsementChanges);
        Task<IEnumerable<EndorsementChanges>> GetAllAsync();
        Task<EndorsementChanges> GetEndorsementChangeAsync(Guid id);
        Task<EndorsementChanges> GetCurrent(Guid riskId);
        Task<IEnumerable<EndorsementChanges>> GetEndorsementChangesAsync(Guid riskId);
        Task<EndorsementChanges> GetEndorsementChangesByRiskId(Guid riskId, int endorsementNumber);
        Task<EndorsementChangeCollection> GetChangesByRiskAndEndorsement(Guid riskId, int endorsementNumber);
        Task<bool> HasVehicleChangesInCurrentEndorsementAsync(VehicleDTO vehicle, Guid riskId);
    }
}