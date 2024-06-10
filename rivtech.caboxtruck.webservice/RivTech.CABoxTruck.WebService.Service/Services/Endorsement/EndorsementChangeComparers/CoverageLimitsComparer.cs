using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers
{
    public class CoverageLimitsComparer : IChangeComparer
    {
        private readonly ICoverageLimitService _coverageLimitService;
        public CoverageLimitsComparer(ICoverageLimitService coverageLimitService)
        {
            _coverageLimitService = coverageLimitService;
        }

        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();
            var coverageLimitsChangeItem = new CoverageLimitsChangeItem();

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var hasChanges = false;

            var newCoverageLimit = newRiskDTO.RiskCoverages.FirstOrDefault();
            var oldCoverageLimit = oldRiskDTO.RiskCoverages.FirstOrDefault();

            if (newCoverageLimit != null && oldCoverageLimit != null)
            {
                if (newCoverageLimit.AutoBILimitId != oldCoverageLimit.AutoBILimitId)
                {
                    coverageLimitsChangeItem.AutoBILimitId = newCoverageLimit.AutoBILimitId;
                    hasChanges = true;
                }

                if (newCoverageLimit.AutoPDLimitId != oldCoverageLimit.AutoPDLimitId)
                {
                    coverageLimitsChangeItem.AutoPDLimitId = newCoverageLimit.AutoPDLimitId;
                    hasChanges = true;
                }

                if (newCoverageLimit.LiabilityDeductibleId != oldCoverageLimit.LiabilityDeductibleId)
                {
                    coverageLimitsChangeItem.LiabilityDeductibleId = newCoverageLimit.LiabilityDeductibleId;
                    hasChanges = true;
                }

                if (newCoverageLimit.MedPayLimitId != oldCoverageLimit.MedPayLimitId)
                {
                    coverageLimitsChangeItem.MedPayLimitId = newCoverageLimit.MedPayLimitId;
                    hasChanges = true;
                }

                if (newCoverageLimit.PIPLimitId != oldCoverageLimit.PIPLimitId)
                {
                    coverageLimitsChangeItem.PIPLimitId = newCoverageLimit.PIPLimitId;
                    hasChanges = true;
                }

                if (newCoverageLimit.UMBILimitId != oldCoverageLimit.UMBILimitId)
                {
                    coverageLimitsChangeItem.UMBILimitId = newCoverageLimit.UMBILimitId;
                    hasChanges = true;
                }

                if (newCoverageLimit.UMPDLimitId != oldCoverageLimit.UMPDLimitId)
                {
                    coverageLimitsChangeItem.UMPDLimitId = newCoverageLimit.UMPDLimitId;
                    hasChanges = true;
                }

                if (newCoverageLimit.UIMBILimitId != oldCoverageLimit.UIMBILimitId)
                {
                    coverageLimitsChangeItem.UIMBILimitId = newCoverageLimit.UIMBILimitId;
                    hasChanges = true;
                }

                if (newCoverageLimit.UIMPDLimitId != oldCoverageLimit.UIMPDLimitId)
                {
                    coverageLimitsChangeItem.UIMPDLimitId = newCoverageLimit.UIMPDLimitId;
                    hasChanges = true;
                }

                if (newCoverageLimit.CoveredSymbolsId != oldCoverageLimit.CoveredSymbolsId)
                {
                    coverageLimitsChangeItem.CoveredSymbolsId = newCoverageLimit.CoveredSymbolsId;
                    hasChanges = true;
                }

                if (newCoverageLimit.SpecifiedPerils != oldCoverageLimit.SpecifiedPerils)
                {
                    coverageLimitsChangeItem.SpecifiedPerils = newCoverageLimit.SpecifiedPerils;
                    hasChanges = true;
                }

                if (newCoverageLimit.CompDeductibleId != oldCoverageLimit.CompDeductibleId)
                {
                    coverageLimitsChangeItem.CompDeductibleId = newCoverageLimit.CompDeductibleId;
                    hasChanges = true;
                }

                if (newCoverageLimit.FireDeductibleId != oldCoverageLimit.FireDeductibleId)
                {
                    coverageLimitsChangeItem.FireDeductibleId = newCoverageLimit.FireDeductibleId;
                    hasChanges = true;
                }

                if (newCoverageLimit.CollDeductibleId != oldCoverageLimit.CollDeductibleId)
                {
                    coverageLimitsChangeItem.CollDeductibleId = newCoverageLimit.CollDeductibleId;
                    hasChanges = true;
                }

                if (newCoverageLimit.GLBILimitId != oldCoverageLimit.GLBILimitId)
                {
                    coverageLimitsChangeItem.GLBILimitId = newCoverageLimit.GLBILimitId;
                    hasChanges = true;
                }

                if (newCoverageLimit.CargoLimitId != oldCoverageLimit.CargoLimitId)
                {
                    coverageLimitsChangeItem.CargoLimitId = newCoverageLimit.CargoLimitId;
                    hasChanges = true;
                }

                if (newCoverageLimit.RefCargoLimitId != oldCoverageLimit.RefCargoLimitId)
                {
                    coverageLimitsChangeItem.RefCargoLimitId = newCoverageLimit.RefCargoLimitId;
                    hasChanges = true;
                }

                if (newCoverageLimit.TrailerInterchangeLimitId != oldCoverageLimit.TrailerInterchangeLimitId)
                {
                    coverageLimitsChangeItem.TrailerInterchangeLimitId = newCoverageLimit.TrailerInterchangeLimitId;
                    hasChanges = true;
                }

                if (newCoverageLimit.NonOwnedTrailerLimitId != oldCoverageLimit.NonOwnedTrailerLimitId)
                {
                    coverageLimitsChangeItem.NonOwnedTrailerLimitId = newCoverageLimit.NonOwnedTrailerLimitId;
                    hasChanges = true;
                }

                if (newCoverageLimit.TrailerTypeLimitId != oldCoverageLimit.TrailerTypeLimitId)
                {
                    coverageLimitsChangeItem.TrailerTypeLimitId = newCoverageLimit.TrailerTypeLimitId;
                    hasChanges = true;
                }

                if (newCoverageLimit.PollutionCovLimitId != oldCoverageLimit.PollutionCovLimitId)
                {
                    coverageLimitsChangeItem.PollutionCovLimitId = newCoverageLimit.PollutionCovLimitId;
                    hasChanges = true;
                }

                if (newCoverageLimit.NumAddtlInsured != oldCoverageLimit.NumAddtlInsured)
                {
                    coverageLimitsChangeItem.NumAddtlInsured = newCoverageLimit.NumAddtlInsured;
                    hasChanges = true;
                }

                if (newCoverageLimit.WaiverSubrogation != oldCoverageLimit.WaiverSubrogation)
                {
                    coverageLimitsChangeItem.WaiverSubrogation = newCoverageLimit.WaiverSubrogation;
                    hasChanges = true;
                }

                if (newCoverageLimit.PrimeNonContribEndorsement != oldCoverageLimit.PrimeNonContribEndorsement)
                {
                    coverageLimitsChangeItem.PrimeNonContribEndorsement = newCoverageLimit.PrimeNonContribEndorsement;
                    hasChanges = true;
                }

                if (newCoverageLimit.EndorsementNumber != oldCoverageLimit.EndorsementNumber)
                {
                    coverageLimitsChangeItem.EndorsementNumber = newCoverageLimit.EndorsementNumber;
                    hasChanges = true;
                }

                if (newCoverageLimit.OptionNumber != oldCoverageLimit.OptionNumber)
                {
                    coverageLimitsChangeItem.OptionNumber = newCoverageLimit.OptionNumber;
                    hasChanges = true;
                }

                if (newCoverageLimit.ALSymbolId != oldCoverageLimit.ALSymbolId)
                {
                    coverageLimitsChangeItem.ALSymbolId = newCoverageLimit.ALSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.PDSymbolId != oldCoverageLimit.PDSymbolId)
                {
                    coverageLimitsChangeItem.PDSymbolId = newCoverageLimit.PDSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.LiabilityDeductibleSymbolId != oldCoverageLimit.LiabilityDeductibleSymbolId)
                {
                    coverageLimitsChangeItem.LiabilityDeductibleSymbolId = newCoverageLimit.LiabilityDeductibleSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.MedPayLimitSymbolId != oldCoverageLimit.MedPayLimitSymbolId)
                {
                    coverageLimitsChangeItem.MedPayLimitSymbolId = newCoverageLimit.MedPayLimitSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.PIPLimitSymbolId != oldCoverageLimit.PIPLimitSymbolId)
                {
                    coverageLimitsChangeItem.PIPLimitSymbolId = newCoverageLimit.PIPLimitSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.UMBILimitSymbolId != oldCoverageLimit.UMBILimitSymbolId)
                {
                    coverageLimitsChangeItem.UMBILimitSymbolId = newCoverageLimit.UMBILimitSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.UMPDLimitSymbolId != oldCoverageLimit.UMPDLimitSymbolId)
                {
                    coverageLimitsChangeItem.UMPDLimitSymbolId = newCoverageLimit.UMPDLimitSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.UIMBILimitSymbolId != oldCoverageLimit.UIMBILimitSymbolId)
                {
                    coverageLimitsChangeItem.UIMBILimitSymbolId = newCoverageLimit.UIMBILimitSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.UIMPDLimitSymbolId != oldCoverageLimit.UIMPDLimitSymbolId)
                {
                    coverageLimitsChangeItem.UIMPDLimitSymbolId = newCoverageLimit.UIMPDLimitSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.CompDeductibleSymbolId != oldCoverageLimit.CompDeductibleSymbolId)
                {
                    coverageLimitsChangeItem.CompDeductibleSymbolId = newCoverageLimit.CompDeductibleSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.FireDeductibleSymbolId != oldCoverageLimit.FireDeductibleSymbolId)
                {
                    coverageLimitsChangeItem.FireDeductibleSymbolId = newCoverageLimit.FireDeductibleSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.CollDeductibleSymbolId != oldCoverageLimit.CollDeductibleSymbolId)
                {
                    coverageLimitsChangeItem.CollDeductibleSymbolId = newCoverageLimit.CollDeductibleSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.GLBILimitSymbolId != oldCoverageLimit.GLBILimitSymbolId)
                {
                    coverageLimitsChangeItem.GLBILimitSymbolId = newCoverageLimit.GLBILimitSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.CargoLimitSymbolId != oldCoverageLimit.CargoLimitSymbolId)
                {
                    coverageLimitsChangeItem.CargoLimitSymbolId = newCoverageLimit.CargoLimitSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.RefCargoLimitSymbolId != oldCoverageLimit.RefCargoLimitSymbolId)
                {
                    coverageLimitsChangeItem.RefCargoLimitSymbolId = newCoverageLimit.RefCargoLimitSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.TrailerInterchangeLimitSymbolId != oldCoverageLimit.TrailerInterchangeLimitSymbolId)
                {
                    coverageLimitsChangeItem.TrailerInterchangeLimitSymbolId = newCoverageLimit.TrailerInterchangeLimitSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.RefCargoLimitSymbolId != oldCoverageLimit.RefCargoLimitSymbolId)
                {
                    coverageLimitsChangeItem.RefCargoLimitSymbolId = newCoverageLimit.RefCargoLimitSymbolId;
                    hasChanges = true;
                }

                if (newCoverageLimit.BrokerCommisionAL != oldCoverageLimit.BrokerCommisionAL)
                {
                    coverageLimitsChangeItem.BrokerCommisionAL = newCoverageLimit.BrokerCommisionAL;
                    hasChanges = true;
                }

                if (newCoverageLimit.BrokerCommisionPD != oldCoverageLimit.BrokerCommisionPD)
                {
                    coverageLimitsChangeItem.BrokerCommisionPD = newCoverageLimit.BrokerCommisionPD;
                    hasChanges = true;
                }


                if (newCoverageLimit.DepositPct != oldCoverageLimit.DepositPct)
                {
                    coverageLimitsChangeItem.DepositPct = newCoverageLimit.DepositPct;
                    hasChanges = true;
                }

                //if (newCoverageLimit.DepositPremium != oldCoverageLimit.DepositPremium)
                //{
                //    coverageLimitsChangeItem.DepositPremium = newCoverageLimit.DepositPremium;
                //    hasChanges = true;
                //}

                if (newCoverageLimit.NumberOfInstallments != oldCoverageLimit.NumberOfInstallments)
                {
                    coverageLimitsChangeItem.NumberOfInstallments = newCoverageLimit.NumberOfInstallments;
                    hasChanges = true;
                }

                if (newCoverageLimit.SubjectivitiesId != oldCoverageLimit.SubjectivitiesId)
                {
                    coverageLimitsChangeItem.SubjectivitiesId = newCoverageLimit.SubjectivitiesId;
                    hasChanges = true;
                }
            }

            if (hasChanges)
            {
                items.Add(coverageLimitsChangeItem);
            }
            else
            {
                items = null;
            }

            return items;
        }
    }
}
