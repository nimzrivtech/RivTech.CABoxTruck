import { Component, OnInit, ViewChild } from '@angular/core';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { ActivatedRoute, Router } from '@angular/router';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { UnderwritingQuestionsData } from '../../../../../data/riskspecifics/underwriting-questions.data';
import { SubmissionData } from '../../../../../data/submission.data';
import { UnderwritingQuestionsDto } from '../../../../../../../shared/models/submission/riskspecifics/underwriting-questions.dto';
import { NextBackButtonComponent } from '../../../submission-details-shared/components/next-back-button/next-back-button.component';
import { Location } from '@angular/common';
import { UnderwritingQuestionConstants } from '../../../../../../../shared/constants/risk-specifics/underwriting-questions.label.constants';
import { GenericLabelConstants } from '../../../../../../../shared/constants/generic.labels.constants';
import { UnderwritingQuestionsValidationService } from '../../../../../../../core/services/validations/underwriting-questions-validation.service';
import { SubmissionNavValidateService } from '../../../../../../../core/services/navigation/submission-nav-validate.service';
import FormUtils from '../../../../../../../shared/utilities/form.utils';

@Component({
  selector: 'app-underwriting-questions',
  templateUrl: './underwriting-questions.component.html',
  styleUrls: ['./underwriting-questions.component.scss']
})
export class UnderwritingQuestionsComponent implements OnInit {

  constructor(private router: Router,
    private route: ActivatedRoute,
    private menuOverrideService: MenuOverrideService,
    private location: Location,
    public underwritingQuestionsData: UnderwritingQuestionsData,
    public underwritingQuestionsValidation: UnderwritingQuestionsValidationService,
    public formValidation: SubmissionNavValidateService,
    public submissionData: SubmissionData) {
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.underwritingQuestionsData.underwritingQuestionsForm; }
  get fn() { return 'underwritingQuestionsForm'; }
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;
  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;

  public UnderwritingQuestionConstants = UnderwritingQuestionConstants;
  public GenericLabelConstants = GenericLabelConstants;

  ngOnInit() {
    if (this.submissionData?.riskDetail?.underwritingQuestions) {
      this.underwritingQuestionsData.initiateFormGroup(this.submissionData.riskDetail.underwritingQuestions);
      this.underwritingQuestionsData.underwritingQuestionsForm.updateValueAndValidity({ onlySelf: false, emitEvent: true });
      this.fg.markAllAsTouched();
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

  public onClick(clickType?: any) {
    this.nextBack.location = this.location;
    switch (clickType) {
      case ClickTypes.Back:
        this.router.navigate([this.nextBack.getPrev()]);
        break;
      case ClickTypes.Next:
        this.menuOverrideService.overrideSideBarBehavior();
        this.underwritingQuestionsData.underwritingQuestionsForm.markAllAsTouched();
        this.router.navigate([this.nextBack.getNext()]);
        break;
    }
  }

  get isConfirmed() {
    return this.fc['areAnswersConfirmed'].value === true;
  }
}
