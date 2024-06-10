import { Injectable } from '@angular/core';
import FormUtils from '../../../shared/utilities/form.utils';
import { ApplicantData } from '../../../modules/submission/data/applicant/applicant.data';
import { SubmissionData } from '../../../modules/submission/data/submission.data';

@Injectable()
export class ApplicantAdditionalInterestValidationService {
  additionalInterestSectionBinding: boolean;

  constructor(public applicantData: ApplicantData, public submissionData: SubmissionData) { }

  checkAdditionalInterestPageBinding() {
    this.additionalInterestSectionBinding = this.checkAdditionalInterestSection();
  }

  checkAdditionalInterestSection() {
    if (this.submissionData.isQuoteIssued || this.submissionData.isIssued) {
      if (!this.submissionData.riskDetail?.additionalInterest?.find(x => !x.isValidated)) {
        return true;
      }
      return false;
    }
    return true;
  }
}
