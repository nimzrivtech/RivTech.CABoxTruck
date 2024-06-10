using System;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics
{
    public class RiskSpecificUnderwritingQuestionDto
    {
        public Guid? Id { get; set; }
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
    }
}
