using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.GeneralLiabilityCargo
{
    public class RiskSpecificGeneralLiabilityDTO
    {
        public bool? ContractuallyRequiredToCarryGeneralLiabilityInsurance { get; set; }
        public bool? HaveOperationsOtherThanTrucking { get; set; }
        public string HaveOperationsOtherThanTruckingExplain { get; set; }
        public bool? OperationsAtStorageLotOrImpoundYard { get; set; }
        public bool? AnyStorageOfGoods { get; set; }
        public bool? AnyWarehousing { get; set; }
        public bool? AnyStorageOfVehiclesForOthers { get; set; }
        public bool? AnyLeasingSpaceToOthers { get; set; }
        public bool? AnyFreightForwarding { get; set; }
        public bool? AnyStorageOfFuelsAndOrChemicals { get; set; }
        public bool? HasApplicantHadGeneralLiabilityLoss { get; set; }
        public string HasApplicantHadGeneralLiabilityLossExplain { get; set; }
        public decimal? GlAnnualPayroll { get; set; }
    }
}
