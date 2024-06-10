import { Injectable } from '@angular/core';
import { UnderwritingQuestionsData } from '../../../modules/submission/data/riskspecifics/underwriting-questions.data';
import FormUtils from '../../../shared/utilities/form.utils';

@Injectable()
export class UnderwritingQuestionsValidationService {
  underwritingQuestionsSection: boolean;

  get UnderwritingQuestionsForm() {
    return this.underwritingQuestionsData.underwritingQuestionsForm;
  }
  constructor(public underwritingQuestionsData: UnderwritingQuestionsData) { }

  checkUnderwritingQuestionsPage() {
    this.underwritingQuestionsSection = this.checkUnderwritingQuestionsSection();
  }

  get areAnswersConfirmed(): boolean {
    return !!this.UnderwritingQuestionsForm.get('areAnswersConfirmed').value;
  }

  checkUnderwritingQuestionsSection() {
    this.UnderwritingQuestionsForm.markAllAsTouched();
    return FormUtils.validateForm(this.UnderwritingQuestionsForm);
  }
}
