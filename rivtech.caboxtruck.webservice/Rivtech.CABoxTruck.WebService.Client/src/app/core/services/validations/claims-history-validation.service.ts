import { Injectable } from '@angular/core';
import { SubmissionData } from '../../../modules/submission/data/submission.data';
import { ClaimsHistoryoData } from '../../../modules/submission/data/claims/claims-history.data';
import FormUtils from '../../../shared/utilities/form.utils';

@Injectable()
export class ClaimsHistoryValidationService {
  claimsSection: boolean;

  get ClaimsForm() {
    return this.claimsData.formInfo;
  }
  constructor(public claimsData: ClaimsHistoryoData, private submissionData: SubmissionData) { }

  checkClaimsPage() {
    this.claimsSection = this.checkClaimsSection();
  }

  checkClaimsSection() {
    this.ClaimsForm.markAllAsTouched();
    this.claimsData.isTouched = true;
    this.claimsData.setRequiredValidator();
    return FormUtils.validateForm(this.ClaimsForm);
  }
}
