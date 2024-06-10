import { Injectable } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UnderwritingQuestionsService } from '../../../../core/services/submission/risk-specifics/underwriting-questions.service';
import Utils from '../../../../shared/utilities/utils';
import { ToastrService } from 'ngx-toastr';
import { IUnderwritingQuestionsDto, UnderwritingQuestionsDto } from '../../../../shared/models/submission/riskspecifics/underwriting-questions.dto';
import { SubmissionData } from '../submission.data';
import { ErrorMessageConstant } from '../../../../shared/constants/error-message.constants';
import { LayoutService } from '@app/core/services/layout/layout.service';
import { createSubmissionDetailsMenuItems } from '../../components/submission-details/submission-details-navitems';
import { RiskSpecificsData } from '../risk-specifics.data';
import { BindingData } from '../binding/binding.data';

@Injectable()
export class UnderwritingQuestionsData {
  underwritingQuestionsForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private submissionData: SubmissionData,
    private riskSpecificsData: RiskSpecificsData,
    private underwritingQuestionsService: UnderwritingQuestionsService,
    private layoutService: LayoutService,
    private bindingData: BindingData
    ) { }

  initiateFormGroup(obj: IUnderwritingQuestionsDto): FormGroup {
    this.underwritingQuestionsForm = this.formBuilder.group({
      id: [this.setInitialValue(obj.id, null)],
      areWorkersCompensationProvided: [this.setInitialValue(obj.areWorkersCompensationProvided, false)],
      workersCompensationProvidedExplanation: [this.setInitialValue(obj.workersCompensationProvidedExplanation, '')],
      workersCompensationCarrier: [this.setInitialValue(obj.workersCompensationCarrier, '')],
      areAllEquipmentOperatedUnderApplicantsAuthority: [this.setInitialValue(obj.areAllEquipmentOperatedUnderApplicantsAuthority, false)],
      equipmentsOperatedUnderAuthorityExplanation: [this.setInitialValue(obj.equipmentsOperatedUnderAuthorityExplanation, '')],
      hasInsuranceBeenObtainedThruAssignedRiskPlan: [this.setInitialValue(obj.hasInsuranceBeenObtainedThruAssignedRiskPlan, false)],
      obtainedThruAssignedRiskPlanExplanation: [this.setInitialValue(obj.obtainedThruAssignedRiskPlanExplanation, '')],
      hasAnyCompanyProvidedNoticeOfCancellation: [this.setInitialValue(obj.hasAnyCompanyProvidedNoticeOfCancellation, false)],
      noticeOfCancellationExplanation: [this.setInitialValue(obj.noticeOfCancellationExplanation, '')],
      hasFiledForBankruptcy: [this.setInitialValue(obj.hasFiledForBankruptcy, false)],
      filingForBankruptcyExplanation: [this.setInitialValue(obj.filingForBankruptcyExplanation, '')],
      hasOperatingAuthoritySuspended: [this.setInitialValue(obj.hasOperatingAuthoritySuspended, false)],
      suspensionExplanation: [this.setInitialValue(obj.suspensionExplanation, '')],
      hasVehicleCountBeenAffectedByCovid19: [this.setInitialValue(obj.hasVehicleCountBeenAffectedByCovid19, false)],
      numberOfVehiclesRunningDuringCovid19: [this.setInitialValue(obj.numberOfVehiclesRunningDuringCovid19, 0)],
      areAnswersConfirmed: [this.setInitialValue(obj.areAnswersConfirmed, false)] // Validators.requiredTrue
    },
    {
      validators: [this.validateConfirmationCheckbox()]
    });

    if ((this.submissionData.validationList.uwAnswersConfirmed?.enable ?? false) === false && this.submissionData.validationList.underwritingQuestions !== undefined) {
      this.underwritingQuestionsForm.markAllAsTouched();
    }

    return this.underwritingQuestionsForm;
  }

  populateUnderwritingQuestionsField() {
    if (this.submissionData.riskDetail?.underwritingQuestions) {
      this.initiateFormGroup(this.submissionData.riskDetail?.underwritingQuestions);
    }
  }

  public setInitialValue(obj: any, val: any) {
    return (obj == null || obj.length === 0) ? val : obj;
  }

  initiateFormValues(fb: FormGroup) {
  }

  saveUnderwritingQuestions() {
    Utils.blockUI();
    const riskDetailId = this.submissionData.riskDetail.id;
    let updatedUnderwritingQuestions: IUnderwritingQuestionsDto = new UnderwritingQuestionsDto(this.underwritingQuestionsForm.value);
    updatedUnderwritingQuestions.riskDetailId = riskDetailId;

    if (updatedUnderwritingQuestions.areWorkersCompensationProvided) {
      updatedUnderwritingQuestions.workersCompensationProvidedExplanation = null;
    }
    if (updatedUnderwritingQuestions.areAllEquipmentOperatedUnderApplicantsAuthority) {
      updatedUnderwritingQuestions.equipmentsOperatedUnderAuthorityExplanation = null;
    }
    if (!updatedUnderwritingQuestions.hasInsuranceBeenObtainedThruAssignedRiskPlan) {
      updatedUnderwritingQuestions.obtainedThruAssignedRiskPlanExplanation = '';
    }
    if (!updatedUnderwritingQuestions.hasAnyCompanyProvidedNoticeOfCancellation) {
      updatedUnderwritingQuestions.noticeOfCancellationExplanation = '';
    }
    if (!updatedUnderwritingQuestions.hasFiledForBankruptcy) {
      updatedUnderwritingQuestions.filingForBankruptcyExplanation = '';
    }
    if (!updatedUnderwritingQuestions.hasOperatingAuthoritySuspended) {
      updatedUnderwritingQuestions.suspensionExplanation = '';
    }
    if (!this.shouldShowCovid19Question) {
      updatedUnderwritingQuestions.hasVehicleCountBeenAffectedByCovid19 = false;
    }
    if (!updatedUnderwritingQuestions.hasVehicleCountBeenAffectedByCovid19 ||
      String(updatedUnderwritingQuestions.numberOfVehiclesRunningDuringCovid19) === '') {
      updatedUnderwritingQuestions.numberOfVehiclesRunningDuringCovid19 = 0;
    }

    updatedUnderwritingQuestions = this.ensureNullForEmpty(updatedUnderwritingQuestions);

    this.underwritingQuestionsService.updateUnderwritingQuestions(updatedUnderwritingQuestions).subscribe(() => {
      Utils.unblockUI();
      this.submissionData.riskDetail.underwritingQuestions = updatedUnderwritingQuestions;
      console.log('UW Questions Saved!');
      if (this.underwritingQuestionsForm.get('areAnswersConfirmed').value === true) {
        this.bindingData.saveBinding();
      }
    }, (error) => {
      Utils.unblockUI();
      this.toastr.error(ErrorMessageConstant.savingErrorMsg);
    });
    return;
  }

  get shouldShowCovid19Question() { return this.submissionData.riskDetail?.vehicles?.length > 10; }

  private ensureNullForEmpty(object: any, properties?: string[]) {
    const iterator = properties ? properties : Object.keys(object);
    iterator.forEach(prop => {
      if (object[prop] === '' || (Array.isArray(object[prop]) && object[prop].length === 0)) {
        object[prop] = null;
      }
    });
    return object;
  }

  toggleMenu(uwAnswersConfirmed: boolean = false) {
    this.submissionData.validationList.uwAnswersConfirmed.enable = uwAnswersConfirmed;
    this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList,
      this.riskSpecificsData.hiddenNavItems));
  }

  validateConfirmationCheckbox() {
    return (form: FormGroup) => {
      const isChecked = form.controls['areAnswersConfirmed'].value ?? false;

      return (isChecked) ? null : { 'uncheckedConfirmation': true };
    };
  }
}
