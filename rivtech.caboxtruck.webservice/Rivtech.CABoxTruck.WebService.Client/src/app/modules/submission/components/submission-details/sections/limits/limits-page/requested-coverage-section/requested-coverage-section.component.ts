import { Component, ElementRef, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { LimitsService } from '../../../../../../../../modules/submission/services/coverage-limits.service';
import { LimitsData } from '../../../../../../../../modules/submission/data/limits/limits.data';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { LimitsDefaultValues, LimitsDefaults } from '../../../../../../../../shared/constants/limits-defaults.constants';
import { ApplicantData } from '../../../../../../../../modules/submission/data/applicant/applicant.data';
import { Validators } from '@angular/forms';

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
    public coverageLimitService: LimitsService
  ) { }

  get f() { return this.limitsData.limitsForm.limitsPageForm.controls; }

  ngOnInit() {
    this.getCoverageLimitAll();
    this.limitsData.retrieveDropDownValues(true);
  }

  getCoverageLimitAll() {
    this.limitsData.populateLimitsFields();
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
            this.limitsData.limitsForm.limitsPageForm.value.pollutionCovLimitId); 
  }
}
