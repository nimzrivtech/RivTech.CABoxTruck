using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public interface IEndorsementChangeDeserializer
    {
        IEndorsementChangeCollection Deserialize(EndorsementChanges endorsementChanges);
    }

    public class EndorsementChangeDeserializer : IEndorsementChangeDeserializer
    {
        public IEndorsementChangeCollection Deserialize(EndorsementChanges endorsementChanges)
        {
            if (endorsementChanges is null)
                return null;

            var changes = new List<IEndorsementChangeItem>();

            var items = JsonConvert.DeserializeObject<dynamic>(endorsementChanges.Changes ?? "");
            if (items is null)
                return null;

            foreach (var item in items)
            {
                IEndorsementChangeItem changeItem;
                var serializedItem = JsonConvert.SerializeObject(item);
                switch (item.Type.ToString())
                {
                    case nameof(AdditionalInterestChangeItem): changeItem = JsonConvert.DeserializeObject<AdditionalInterestChangeItem>(serializedItem); break;
                    case nameof(BindRequirementsChangeItem): changeItem = JsonConvert.DeserializeObject<BindRequirementsChangeItem>(serializedItem); break;
                    case nameof(BusinessDetailsChangeItem): changeItem = JsonConvert.DeserializeObject<BusinessDetailsChangeItem>(serializedItem); break;
                    case nameof(BusinessNameChangeItem): changeItem = JsonConvert.DeserializeObject<BusinessNameChangeItem>(serializedItem); break;
                    case nameof(CargoChangeItem): changeItem = JsonConvert.DeserializeObject<CargoChangeItem>(serializedItem); break;
                    case nameof(CarrierInfoChangeItem): changeItem = JsonConvert.DeserializeObject<CarrierInfoChangeItem>(serializedItem); break;
                    case nameof(CoverageLimitsChangeItem): changeItem = JsonConvert.DeserializeObject<CoverageLimitsChangeItem>(serializedItem); break;
                    case nameof(FormDriverRequirementsEndorsementChangeItem): changeItem = JsonConvert.DeserializeObject<FormDriverRequirementsEndorsementChangeItem>(serializedItem); break;
                    case nameof(FormExclusionRadiusEndorsementChangeItem): changeItem = JsonConvert.DeserializeObject<FormExclusionRadiusEndorsementChangeItem>(serializedItem); break;
                    case nameof(FormGeneralLiabilityEndorsementChangeItem): changeItem = JsonConvert.DeserializeObject<FormGeneralLiabilityEndorsementChangeItem>(serializedItem); break;
                    case nameof(FormHiredPhysicalDamageEndorsementChangeItem): changeItem = JsonConvert.DeserializeObject<FormHiredPhysicalDamageEndorsementChangeItem>(serializedItem); break;
                    case nameof(FormLossPayeeChangeItem): changeItem = JsonConvert.DeserializeObject<FormLossPayeeChangeItem>(serializedItem); break;
                    case nameof(FormMCS90ChangeItem): changeItem = JsonConvert.DeserializeObject<FormMCS90ChangeItem>(serializedItem); break;
                    case nameof(FormSelectionChangeItem): changeItem = JsonConvert.DeserializeObject<FormSelectionChangeItem>(serializedItem); break;
                    case nameof(FormTerritoryExclusionEndorsementChangeItem): changeItem = JsonConvert.DeserializeObject<FormTerritoryExclusionEndorsementChangeItem>(serializedItem); break;
                    case nameof(FormTrailerInterchangeCoverageChangeItem): changeItem = JsonConvert.DeserializeObject<FormTrailerInterchangeCoverageChangeItem>(serializedItem); break;
                    case nameof(GeneralLiabilityChangeItem): changeItem = JsonConvert.DeserializeObject<GeneralLiabilityChangeItem>(serializedItem); break;
                    case nameof(PolicyContactsChangeItem): changeItem = JsonConvert.DeserializeObject<PolicyContactsChangeItem>(serializedItem); break;
                    case nameof(VehicleAddChangeItem): changeItem = JsonConvert.DeserializeObject<VehicleAddChangeItem>(serializedItem); break;
                    case nameof(VehicleUpdateChangeItem): changeItem = JsonConvert.DeserializeObject<VehicleUpdateChangeItem>(serializedItem); break;
                    case nameof(VehicleRemoveChangeItem): changeItem = JsonConvert.DeserializeObject<VehicleRemoveChangeItem>(serializedItem); break;
                    case nameof(VehicleReinstateChangeItem): changeItem = JsonConvert.DeserializeObject<VehicleReinstateChangeItem>(serializedItem); break;
                    case nameof(DriverAddChangeItem): changeItem = JsonConvert.DeserializeObject<DriverAddChangeItem>(serializedItem); break;
                    case nameof(DriverUpdateChangeItem): changeItem = JsonConvert.DeserializeObject<DriverUpdateChangeItem>(serializedItem); break;
                    case nameof(DriverRemoveChangeItem): changeItem = JsonConvert.DeserializeObject<DriverRemoveChangeItem>(serializedItem); break;
                    case nameof(DriverReinstateChangeItem): changeItem = JsonConvert.DeserializeObject<DriverReinstateChangeItem>(serializedItem); break;
                    case nameof(AddressesChangeItem): changeItem = JsonConvert.DeserializeObject<AddressesChangeItem>(serializedItem); break;
                    case nameof(ManusciptChangeItem): changeItem = JsonConvert.DeserializeObject<ManusciptChangeItem>(serializedItem); break;
                    case nameof(CommoditiesHauledChangeItem): changeItem = JsonConvert.DeserializeObject<CommoditiesHauledChangeItem>(serializedItem); break;
                    case nameof(RatingFactorsChangeItem): changeItem = JsonConvert.DeserializeObject<RatingFactorsChangeItem>(serializedItem); break;
                    case nameof(FormPennsylvaniaAddCombFPBChangeItem): changeItem = JsonConvert.DeserializeObject<FormPennsylvaniaAddCombFPBChangeItem>(serializedItem); break;
                    case nameof(CoveredAutoDesignationSymbolChangeItem): changeItem = JsonConvert.DeserializeObject<CoveredAutoDesignationSymbolChangeItem>(serializedItem); break;
                    default: changeItem = null; break;
                }

                if (changeItem != null)
                    changes.Add(changeItem);

            }

            return new EndorsementChangeCollection(changes);
        }
    }
}
