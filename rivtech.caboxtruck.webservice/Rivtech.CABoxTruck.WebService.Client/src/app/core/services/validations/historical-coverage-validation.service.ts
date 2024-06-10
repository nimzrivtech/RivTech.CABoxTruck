import { Injectable } from '@angular/core';
import { CoverageData } from '../../../modules/submission/data/coverages/coverages.data';
import FormUtils from '../../../shared/utilities/form.utils';

@Injectable()
export class HistoricalCoverageValidationService {
  historicalCoverageSection: boolean;

  get HistoricalCoverageForm() {
    return this.coverageData.coverageForms.historicalCoverageForm;
  }
  constructor(public coverageData: CoverageData) { }

  checkHistoricalCoveragePage() {
    this.historicalCoverageSection = this.checkHistoricalCoverageSection();
  }

  checkHistoricalCoverageSection() {
    this.HistoricalCoverageForm.markAllAsTouched();
    this.coverageData.isTouched = true;
    this.coverageData.setRequiredValidator();

    return FormUtils.validateForm(this.HistoricalCoverageForm);
  }
}
