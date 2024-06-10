import { Injectable } from '@angular/core';
import FormUtils from '../../../shared/utilities/form.utils';
import { ApplicantData } from '../../../modules/submission/data/applicant/applicant.data';

@Injectable()
export class ApplicantFilingsValidationService {
  filingInformationSection: boolean;

  get FilingsInformationForm() {
    return this.applicantData.applicantForms.filingsInformationForm;
  }
  constructor(public applicantData: ApplicantData) { }

  checkFilingsInformationPage() {
    this.filingInformationSection = this.checkFilingsInformationSection();
  }

  checkFilingsInformationSection() {
    this.FilingsInformationForm.markAllAsTouched();
    // return FormUtils.validateForm(this.FilingsInformationForm);
    let isInsuredHasMotorCarrierNo = this.FilingsInformationForm.controls['isInsuredHasMotorCarrierNo']?.value;
    if (FormUtils.validateFormControl(this.FilingsInformationForm.controls['mcNumber']) || isInsuredHasMotorCarrierNo != true) {
      return true;
    }

    return false;
  }
}
