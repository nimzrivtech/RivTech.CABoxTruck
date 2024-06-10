import { Injectable } from '@angular/core';
import { FormArray, FormGroup } from '@angular/forms';
import { QuoteLimitsData } from '../../../modules/submission/data/quote/quote-limits.data';
import FormUtils from '../../../shared/utilities/form.utils';

@Injectable()
export class QuoteValidationService {
  quoteSection: boolean;

  get QuotePageForm() {
    return this.quoteLimitsData.quoteOptionsForm;
  }
  constructor(public quoteLimitsData: QuoteLimitsData) { }

  checkQuotePage() {
    this.quoteSection = this.checkQuoteSection();
  }

  checkQuoteSection() {
    this.QuotePageForm.markAllAsTouched();
    var formsValid = FormUtils.validateForm(this.QuotePageForm) 

    /*
    var optionItems = (this.QuotePageForm.controls["optionItems"] as FormArray);
    var option = optionItems.controls[0] as FormGroup;
    var rfSched = option.controls["liabilityScheduleRatingFactor"];
    var rfXP = option.controls["liabilityExperienceRatingFactor"];
    */

    return formsValid; //&& rfSched != null && rfXP != null;
  }
}
