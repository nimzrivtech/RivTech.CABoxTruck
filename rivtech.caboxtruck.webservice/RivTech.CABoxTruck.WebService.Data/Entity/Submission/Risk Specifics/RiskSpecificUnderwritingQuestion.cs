using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics
{
    public class RiskSpecificUnderwritingQuestion : BaseEntity<Guid>
    {
        public RiskSpecificUnderwritingQuestion()
        {
            this.CreatedDate = DateTime.Now;
        }
        public RiskDetail Risk { get; set; }
        public bool AreWorkersCompensationProvided { get; set; }
        public string WorkersCompensationCarrier { get; set; }
        public string WorkersCompensationProvidedExplanation { get; set; }
        public bool AreAllEquipmentOperatedUnderApplicantsAuthority { get; set; }
        public string EquipmentsOperatedUnderAuthorityExplanation { get; set; }
        public bool HasInsuranceBeenObtainedThruAssignedRiskPlan { get; set; }
        public string ObtainedThruAssignedRiskPlanExplanation { get; set; }
        public bool HasAnyCompanyProvidedNoticeOfCancellation { get; set; }
        public string NoticeOfCancellationExplanation { get; set; }
        public bool HasFiledForBankruptcy { get; set; }
        public string FilingForBankruptcyExplanation { get; set; }
        public bool HasOperatingAuthoritySuspended { get; set; }
        public string SuspensionExplanation { get; set; }
        public bool HasVehicleCountBeenAffectedByCovid19 { get; set; }
        public int NumberOfVehiclesRunningDuringCovid19 { get; set; }
        public bool AreAnswersConfirmed { get; set; }

        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
