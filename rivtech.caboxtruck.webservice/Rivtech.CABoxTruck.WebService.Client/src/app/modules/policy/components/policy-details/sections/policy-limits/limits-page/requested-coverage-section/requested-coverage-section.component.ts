import { Component, OnInit } from '@angular/core';
import { LimitsService } from '../../../../../../../../modules/submission/services/coverage-limits.service';
import { LimitsData } from '../../../../../../../../modules/submission/data/limits/limits.data';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { ApplicantData } from '../../../../../../../../modules/submission/data/applicant/applicant.data';
import { PolicySummaryData } from '../../../../../../../../modules/policy/data/policy-summary.data';
import { LimitsDefaultValues } from '@app/shared/constants/limits-defaults.constants';

@Component({
  selector: 'app-requested-coverage-section',
  templateUrl: './requested-coverage-section.component.html',
  styleUrls: ['./requested-coverage-section.component.scss']
})
export class RequestedCoverageSectionComponent implements OnInit {
  hideMe: boolean = false;
  constructor(
    public applicantData: ApplicantData,
    public limitsData: LimitsData,
    public submissionData: SubmissionData,
    public coverageLimitService: LimitsService,
    public policySummaryData: PolicySummaryData
  ) { }

  get f() { return this.limitsData.limitsForm.limitsPageForm.controls; }

  ngOnInit() {
    this.limitsData.initiateFormFields();
    this.getCoverageLimitAll();
    this.limitsData.retrieveDropDownValues(true);
    this.limitsData.limitsForm.limitsPageForm.markAllAsTouched();
  }

  getCoverageLimitAll() {
    this.limitsData.populatePolicyLimitsFields();
    this.limitsData.oldLimitsData = Object.assign({}, this.submissionData.riskDetail.riskCoverage);
    // Set defaults
    if (this.submissionData.riskDetail.riskCoverage == null) {
      this.limitsData.setDefaults();
      this.limitsData.hasRiskCoverage = false;
      return;
    }
    else if (this.limitsData.raterVersionSupport() && this.submissionData.riskDetail.riskCoverage != null && !this.hasV7LimitsValues()) {
      this.limitsData.limitsForm.limitsPageForm.controls['trailerInterchangeLimitId'].patchValue(LimitsDefaultValues.triId);
      this.limitsData.limitsForm.limitsPageForm.controls['nonOwnedTrailerLimitId'].patchValue(LimitsDefaultValues.noId);
      // this.limitsData.limitsForm.limitsPageForm.controls['trailerTypeLimitId'].patchValue(null);
      this.limitsData.limitsForm.limitsPageForm.controls['pollutionCovLimitId'].patchValue(LimitsDefaultValues.pocId);
      this.limitsData.limitsForm.limitsPageForm.controls['trailerInterchangeLimitSymbolId'].patchValue(null);
      this.limitsData.limitsForm.limitsPageForm.controls['nonOwnedTrailerLimitSymbolId'].patchValue(null);
    }

  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  hasV7LimitsValues(): boolean {
    return this.limitsData.raterVersionSupport() &&
           (this.limitsData.limitsForm.limitsPageForm.value.trailerInterchangeLimitId ||
            this.limitsData.limitsForm.limitsPageForm.value.trailerInterchangeLimitSymbolId ||
            this.limitsData.limitsForm.limitsPageForm.value.nonOwnedTrailerLimitId ||
            this.limitsData.limitsForm.limitsPageForm.value.nonOwnedTrailerLimitSymbolId ||
            this.limitsData.limitsForm.limitsPageForm.value.trailerTypeLimitId ||
            this.limitsData.limitsForm.limitsPageForm.value.pollutionCovLimitId); 
  }
}
