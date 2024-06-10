import { Component, OnInit, ViewChild } from '@angular/core';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { UnderwritingQuestionsDto } from '../../../../../../../shared/models/submission/riskspecifics/underwriting-questions.dto';
import { UnderwritingQuestionConstants } from '../../../../../../../shared/constants/risk-specifics/underwriting-questions.label.constants';
import { GenericLabelConstants } from '../../../../../../../shared/constants/generic.labels.constants';
import { UnderwritingQuestionsData } from '../../../../../../../modules/submission/data/riskspecifics/underwriting-questions.data';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { NavigationService } from '@app/core/services/navigation/navigation.service';
import { PathConstants } from '@app/shared/constants/path.constants';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';

@Component({
  selector: 'app-underwriting-questions',
  templateUrl: './underwriting-questions.component.html',
  styleUrls: ['./underwriting-questions.component.scss']
})
export class UnderwritingQuestionsComponent implements OnInit {

  constructor(
    private navigationService: NavigationService,
    private riskSpecificsData: RiskSpecificsData,
    public underwritingQuestionsData: UnderwritingQuestionsData,
    public submissionData: SubmissionData) {
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.underwritingQuestionsData.underwritingQuestionsForm; }
  get fn() { return 'underwritingQuestionsForm'; }
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;

  public UnderwritingQuestionConstants = UnderwritingQuestionConstants;
  public GenericLabelConstants = GenericLabelConstants;

  ngOnInit() {
    if (this.submissionData?.riskDetail?.underwritingQuestions) {
      this.underwritingQuestionsData.initiateFormGroup(this.submissionData.riskDetail.underwritingQuestions);
      this.underwritingQuestionsData.underwritingQuestionsForm.updateValueAndValidity({ onlySelf: false, emitEvent: true });
    } else {
      this.underwritingQuestionsData.initiateFormGroup(new UnderwritingQuestionsDto());
    }

    this.switchValidator(!this.fc['areWorkersCompensationProvided'].value, 'workersCompensationProvidedExplanation');
    this.switchValidator(!this.fc['areAllEquipmentOperatedUnderApplicantsAuthority'].value, 'equipmentsOperatedUnderAuthorityExplanation');
    this.switchValidator(this.fc['hasInsuranceBeenObtainedThruAssignedRiskPlan'].value, 'obtainedThruAssignedRiskPlanExplanation');
    this.switchValidator(this.fc['hasAnyCompanyProvidedNoticeOfCancellation'].value, 'noticeOfCancellationExplanation');
    this.switchValidator(this.fc['hasFiledForBankruptcy'].value, 'filingForBankruptcyExplanation');
    this.switchValidator(this.fc['hasOperatingAuthoritySuspended'].value, 'suspensionExplanation');
    this.switchValidator(this.fc['hasVehicleCountBeenAffectedByCovid19'].value, 'numberOfVehiclesRunningDuringCovid19', 0);
  }

  switchValidator(isChecked: boolean, targetControlName: string, defaultValue?: any) {
    // if (isChecked) {
    //   FormUtils.addRequiredValidator(this.fg, targetControlName);
    // } else {
    //   FormUtils.clearValidator(this.fg, targetControlName);
    // }

    if (!isChecked) {
      this.fc[targetControlName].setValue(defaultValue);
    }
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        if (this.riskSpecificsData.hiddenNavItems.includes('General Liability & Cargo')) {
          this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.RiskSpecifics.Index}/${PathConstants.Submission.RiskSpecifics.MaintenanceSafety}`);
        } else {
          this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.RiskSpecifics.Index}/${PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo}`);
        }
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Submission.Bind.Index);
        break;
    }
  }

  get isConfirmed() {
    return this.fc['areAnswersConfirmed'].value === true;
  }
}
