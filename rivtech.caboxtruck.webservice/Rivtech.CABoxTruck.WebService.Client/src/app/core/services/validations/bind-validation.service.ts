import { Injectable } from '@angular/core';
import { BindingData } from '../../../modules/submission/data/binding/binding.data';
import FormUtils from '../../../shared/utilities/form.utils';
import { SubmissionData } from '@app/modules/submission/data/submission.data';

@Injectable()
export class BindValidationService {
  bindSection: boolean;

  get BindForm() {
    return this.bindData.formBinding;
  }
  constructor(public bindData: BindingData, public submissionData: SubmissionData) { }

  checkBindPage() {
    this.bindSection = this.checkBindSection() && this.checkBindAndQuoteOptionSection();
  }

  checkBindSection() {
    this.BindForm.markAllAsTouched();
    return FormUtils.validateForm(this.BindForm);
  }

  checkBindAndQuoteOptionSection() {
    if ((this.submissionData.riskDetail?.underwritingQuestions?.areAnswersConfirmed ?? false) == false) {
      return true
    }
    if (this.submissionData.riskDetail?.bindingRequirements?.length === 0) {
      return false;
    }
    if (this.submissionData.riskDetail?.quoteConditions?.length === 0) {
      return false;
    }
    return true;
  }
}
