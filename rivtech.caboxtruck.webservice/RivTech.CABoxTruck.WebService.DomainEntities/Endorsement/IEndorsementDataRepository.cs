using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement
{
    public interface IEndorsementDataRepository
    {
        Task<IEndorsementData> UpdateAsync(IEndorsementData endorsementData);
        Task<IEndorsementData> LoadAsync(EndorsementPoint endorsementPoint);
        Task<RiskEndorsementPoints> GetRiskEndorsementPoints(Guid riskId);
        Task<IEndorsementData> LoadForEndorsementFromRiskAsync(EndorsementPoint endorsementPoint);
        Task UpdateVehiclesEffectiveDates(IEndorsementData clonedEndorsementData, DateTime effectiveDate, bool isOutOfSequence);
        Task UpdateDriversEffectiveDates(IEndorsementData clonedEndorsementData, DateTime effectiveDate, bool isOutOfSequence);
    }
}
