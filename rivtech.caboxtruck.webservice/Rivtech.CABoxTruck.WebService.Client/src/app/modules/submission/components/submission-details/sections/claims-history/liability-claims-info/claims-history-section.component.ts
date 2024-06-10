import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { ClaimsHistoryoData } from '../../../../../data/claims/claims-history.data';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { FormArray } from '@angular/forms';
import { SelectItem } from '../../../../../../../shared/models/dynamic/select-item';
import { ErrorMessageConstant } from '../../../../../../../shared/constants/error-message.constants';
@Component({
  selector: 'app-claims-history-section',
  templateUrl: './claims-history-section.component.html',
  styleUrls: ['./claims-history-section.component.scss']
})
export class ClaimsHistorySectionComponent implements OnInit, OnDestroy {
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;

  lossRunErrorMessage = ErrorMessageConstant.lossRunErrorMessage;
  constructor(public Data: ClaimsHistoryoData, public submissionData: SubmissionData) { }

  get groundUpNetOptions(): SelectItem[] {
    return [
      { value: null, label: 'Please Select' },
      { value: 'G', label: 'G' },
      { value: 'N', label: 'N' },
    ]
  }

  ngOnInit() {
    this.Data.setRequiredValidator();
  }

  ngOnDestroy() {
    this.revertDateIfInvalidInput();
  }

  get f() { return this.Data.formInfo.controls; }

  private revertDateIfInvalidInput() {
    const currentHistories = this.submissionData.riskDetail?.claimsHistory;
    document.querySelectorAll('.loss-run-date input').forEach((dp, i) => {
      if (dp.className.includes('ng-invalid')) {
        const currentValue = currentHistories[i].lossRunDate;
        if (currentValue) {
          const runLossDates = <FormArray>this.Data.formInfo.controls['lossRunDate'];
          runLossDates.controls[i].patchValue({ isRange: false, singleDate: { jsDate: new Date(currentValue) } });
        }
      }
    });
  }
}
