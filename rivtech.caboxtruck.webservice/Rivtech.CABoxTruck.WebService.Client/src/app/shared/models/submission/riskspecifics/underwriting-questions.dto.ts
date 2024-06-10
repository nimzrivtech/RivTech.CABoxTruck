export class UnderwritingQuestionsDto implements IUnderwritingQuestionsDto {
  id?: string = null;
  riskDetailId?: string = null;
  areWorkersCompensationProvided?: boolean = false;
  workersCompensationCarrier?: string = null;
  workersCompensationProvidedExplanation?: string = null;
  areAllEquipmentOperatedUnderApplicantsAuthority?: boolean = false;
  equipmentsOperatedUnderAuthorityExplanation?: string = null;
  hasInsuranceBeenObtainedThruAssignedRiskPlan?: boolean = false;
  obtainedThruAssignedRiskPlanExplanation?: string = null;
  hasAnyCompanyProvidedNoticeOfCancellation?: boolean = false;
  noticeOfCancellationExplanation?: string = null;
  hasFiledForBankruptcy?: boolean = false;
  filingForBankruptcyExplanation?: string = null;
  hasOperatingAuthoritySuspended?: boolean = false;
  suspensionExplanation?: string = null;
  hasVehicleCountBeenAffectedByCovid19?: boolean = false;
  numberOfVehiclesRunningDuringCovid19: number = 0;
  areAnswersConfirmed?: boolean = false;

  constructor(init?: Partial<IUnderwritingQuestionsDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IUnderwritingQuestionsDto {
  id?: string;
  riskDetailId?: string;
  areWorkersCompensationProvided?: boolean;
  workersCompensationCarrier?: string;
  workersCompensationProvidedExplanation?: string
  areAllEquipmentOperatedUnderApplicantsAuthority?: boolean;
  equipmentsOperatedUnderAuthorityExplanation?: string
  hasInsuranceBeenObtainedThruAssignedRiskPlan?: boolean;
  obtainedThruAssignedRiskPlanExplanation?: string;
  hasAnyCompanyProvidedNoticeOfCancellation?: boolean;
  noticeOfCancellationExplanation?: string;
  hasFiledForBankruptcy?: boolean;
  filingForBankruptcyExplanation?: string;
  hasOperatingAuthoritySuspended?: boolean;
  suspensionExplanation?: string;
  hasVehicleCountBeenAffectedByCovid19?: boolean;
  numberOfVehiclesRunningDuringCovid19: number;
  areAnswersConfirmed?: boolean;
}
