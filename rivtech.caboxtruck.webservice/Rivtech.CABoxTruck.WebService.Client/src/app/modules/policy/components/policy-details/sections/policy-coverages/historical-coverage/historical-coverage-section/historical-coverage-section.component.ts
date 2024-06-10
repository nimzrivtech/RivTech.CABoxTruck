import { Component, OnInit } from '@angular/core';
import { CoverageHistoryService } from '../../../../../../../../modules/submission/services/coverage-history.service';
import { CoverageData } from '../../../../../../../../modules/submission/data/coverages/coverages.data';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';

@Component({
  selector: 'app-historical-coverage-section',
  templateUrl: './historical-coverage-section.component.html',
  styleUrls: ['./historical-coverage-section.component.scss']
})
export class HistoricalCoverageSectionComponent implements OnInit {
  hideMe: boolean = false;
  constructor(
    public coverageData: CoverageData,
    public submissionData: SubmissionData,
    public policySummaryData: PolicySummaryData,
    public coverageHistoryService: CoverageHistoryService) { }

  ngOnInit() {
    this.coverageData.resetFormValidators();
    this.coverageData.setRequiredValidator();
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }
}
