import { Component, OnDestroy, OnInit } from '@angular/core';
import { LimitsData } from '../../../../../../../../../modules/submission/data/limits/limits.data';
import { CoverageData } from '../../../../../../../../../modules/submission/data/coverages/coverages.data';
import { BaseComponent } from '../../../../../../../../../shared/base-component';
import { FormArray } from '@angular/forms';
import { ErrorMessageConstant } from '../../../../../../../../../shared/constants/error-message.constants';
import { SubmissionData } from '../../../../../../../../../modules/submission/data/submission.data';

@Component({
  selector: 'app-historical-coverage-chart',
  templateUrl: './historical-coverage-chart.component.html',
  styleUrls: ['./historical-coverage-chart.component.scss']
})
export class HistoricalCoverageChartComponent extends BaseComponent implements OnInit, OnDestroy {
  public ErrorMessageConstant = ErrorMessageConstant;

  constructor(public coverageData: CoverageData,
    public limitsData: LimitsData,
    public submissionData: SubmissionData) { super(); }

  get f() { return this.coverageData.coverageForms.historicalCoverageForm.controls; }

  ngOnInit() {
    this.coverageData.initDateOptions();
    this.limitsData.setCoveragesDisplay();
  }

  setEndDate(startDate, field) {
    if (startDate?.jsDate) {
      const endDates = this.coverageData.coverageForms.historicalCoverageForm.value.policyTermEndDate;
      const endDate = startDate?.jsDate ? { isRange: false, singleDate:
        {
          jsDate: new Date(startDate?.jsDate.getFullYear() + 1, startDate?.jsDate.getMonth(), startDate?.jsDate.getDate())
        } } : null;
      endDates.splice(field, 1, endDate);
      this.coverageData.coverageForms.historicalCoverageForm.controls['policyTermEndDate'].patchValue(endDates);
    }
  }

  changeEndDate(field) {

  }

  ngOnDestroy() {
    this.revertDateIfInvalidInput();
  }

  private revertDateIfInvalidInput() {
    const currentHistories = this.coverageData.submissionData?.riskDetail?.riskHistory?.riskHistories;
    document.querySelectorAll('.term-start-date input').forEach((dp, i) => {
      this.revertLastSavedDate(i, dp, 'policyTermStartDate');
    });
    document.querySelectorAll('.term-end-date input').forEach((dp, i) => {
      this.revertLastSavedDate(i, dp, 'policyTermEndDate');
    });
    document.querySelectorAll('.loss-run-date input').forEach((dp, i) => {
      this.revertLastSavedDate(i, dp, 'lossRunDateForEachTerm');
    });
  }

  private revertLastSavedDate(i: number, datePicker: any, propName: string) {
    const currentHistories = this.coverageData.submissionData?.riskDetail?.riskHistory?.riskHistories;
    if (datePicker.className.includes('ng-invalid')) {
      const currentValue = currentHistories[i][propName];
      if (currentValue) {
        const dates = <FormArray>this.coverageData.coverageForms.historicalCoverageForm.controls[propName];
        dates.controls[i].patchValue({ isRange: false, singleDate: { jsDate: new Date(currentValue) } });
      }
    }
  }
}
