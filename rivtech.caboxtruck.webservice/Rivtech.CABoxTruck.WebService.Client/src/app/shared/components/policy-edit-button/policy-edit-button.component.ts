import { Component, OnInit } from '@angular/core';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';

@Component({
  selector: 'app-policy-edit-button',
  templateUrl: './policy-edit-button.component.html',
  styleUrls: ['./policy-edit-button.component.scss']
})
export class PolicyEditButtonComponent implements OnInit {

  constructor(
    public policySummaryData: PolicySummaryData,
    public submissionData: SubmissionData
  ) { }

  ngOnInit() {
  }

  get endorsementNotAllowedTooltip(): string {
    return this.submissionData.isCancelledPolicy ? 'Endorsement is not allowed for canceled policies.' : '';
  }

  get isEditDisabled() {
    return this.policySummaryData.isPolicyLocked
            || this.policySummaryData.canEditPolicy 
            || this.submissionData.isCancelledPolicy;
  }

}
