using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers
{
    public class GeneralLiabilityComparer : IChangeComparer
    {
        private readonly IRiskSpecificGeneralLiabilityCargoService _generalLiabilityCargoService;
        public GeneralLiabilityComparer(IRiskSpecificGeneralLiabilityCargoService generalLiabilityCargoService)
        {
            _generalLiabilityCargoService = generalLiabilityCargoService;
        }

        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();
            var generalLiabilityChangeItem = new GeneralLiabilityChangeItem();

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var hasChanges = false;

            var newGeneralLiability = newRiskDTO.GeneralLiabilityCargo;
            var oldGeneralLiability = oldRiskDTO.GeneralLiabilityCargo;

            if (newGeneralLiability.GeneralLiability.ContractuallyRequiredToCarryGeneralLiabilityInsurance != oldGeneralLiability.GeneralLiability.ContractuallyRequiredToCarryGeneralLiabilityInsurance)
            {
                generalLiabilityChangeItem.ContractuallyRequiredToCarryGeneralLiabilityInsurance = newGeneralLiability.GeneralLiability.ContractuallyRequiredToCarryGeneralLiabilityInsurance;
                hasChanges = true;
            }

            if (newGeneralLiability.GeneralLiability.HaveOperationsOtherThanTrucking != oldGeneralLiability.GeneralLiability.HaveOperationsOtherThanTrucking)
            {
                generalLiabilityChangeItem.HaveOperationsOtherThanTrucking = newGeneralLiability.GeneralLiability.HaveOperationsOtherThanTrucking;
                hasChanges = true;
            }

            if (newGeneralLiability.GeneralLiability.HaveOperationsOtherThanTruckingExplain != oldGeneralLiability.GeneralLiability.HaveOperationsOtherThanTruckingExplain)
            {
                generalLiabilityChangeItem.HaveOperationsOtherThanTruckingExplain = newGeneralLiability.GeneralLiability.HaveOperationsOtherThanTruckingExplain;
                hasChanges = true;
            }

            if (newGeneralLiability.GeneralLiability.OperationsAtStorageLotOrImpoundYard != oldGeneralLiability.GeneralLiability.OperationsAtStorageLotOrImpoundYard)
            {
                generalLiabilityChangeItem.OperationsAtStorageLotOrImpoundYard = newGeneralLiability.GeneralLiability.OperationsAtStorageLotOrImpoundYard;
                hasChanges = true;
            }

            if (newGeneralLiability.GeneralLiability.AnyStorageOfGoods != oldGeneralLiability.GeneralLiability.AnyStorageOfGoods)
            {
                generalLiabilityChangeItem.AnyStorageOfGoods = newGeneralLiability.GeneralLiability.AnyStorageOfGoods;
                hasChanges = true;
            }

            if (newGeneralLiability.GeneralLiability.AnyWarehousing != oldGeneralLiability.GeneralLiability.AnyWarehousing)
            {
                generalLiabilityChangeItem.AnyWarehousing = newGeneralLiability.GeneralLiability.AnyWarehousing;
                hasChanges = true;
            }

            if (newGeneralLiability.GeneralLiability.AnyStorageOfVehiclesForOthers != oldGeneralLiability.GeneralLiability.AnyStorageOfVehiclesForOthers)
            {
                generalLiabilityChangeItem.AnyStorageOfVehiclesForOthers = newGeneralLiability.GeneralLiability.AnyStorageOfVehiclesForOthers;
                hasChanges = true;
            }

            if (newGeneralLiability.GeneralLiability.AnyLeasingSpaceToOthers != oldGeneralLiability.GeneralLiability.AnyLeasingSpaceToOthers)
            {
                generalLiabilityChangeItem.AnyLeasingSpaceToOthers = newGeneralLiability.GeneralLiability.AnyLeasingSpaceToOthers;
                hasChanges = true;
            }

            if (newGeneralLiability.GeneralLiability.AnyFreightForwarding != oldGeneralLiability.GeneralLiability.AnyFreightForwarding)
            {
                generalLiabilityChangeItem.AnyFreightForwarding = newGeneralLiability.GeneralLiability.AnyFreightForwarding;
                hasChanges = true;
            }

            if (newGeneralLiability.GeneralLiability.AnyStorageOfFuelsAndOrChemicals != oldGeneralLiability.GeneralLiability.AnyStorageOfFuelsAndOrChemicals)
            {
                generalLiabilityChangeItem.AnyStorageOfFuelsAndOrChemicals = newGeneralLiability.GeneralLiability.AnyStorageOfFuelsAndOrChemicals;
                hasChanges = true;
            }

            if (newGeneralLiability.GeneralLiability.HasApplicantHadGeneralLiabilityLoss != oldGeneralLiability.GeneralLiability.HasApplicantHadGeneralLiabilityLoss)
            {
                generalLiabilityChangeItem.HasApplicantHadGeneralLiabilityLoss = newGeneralLiability.GeneralLiability.HasApplicantHadGeneralLiabilityLoss;
                hasChanges = true;
            }

            if (newGeneralLiability.GeneralLiability.HasApplicantHadGeneralLiabilityLossExplain != oldGeneralLiability.GeneralLiability.HasApplicantHadGeneralLiabilityLossExplain)
            {
                generalLiabilityChangeItem.HasApplicantHadGeneralLiabilityLossExplain = newGeneralLiability.GeneralLiability.HasApplicantHadGeneralLiabilityLossExplain;
                hasChanges = true;
            }

            if (newGeneralLiability.GeneralLiability.GlAnnualPayroll != oldGeneralLiability.GeneralLiability.GlAnnualPayroll)
            {
                generalLiabilityChangeItem.GlAnnualPayroll = newGeneralLiability.GeneralLiability.GlAnnualPayroll;
                hasChanges = true;
            }

            if (hasChanges)
            {
                items.Add(generalLiabilityChangeItem);
            }
            else
            {
                items = null;
            }

            return items;
            }
        }
    }

