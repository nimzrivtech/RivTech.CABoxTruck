import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { NextBackButtonComponent } from '../../../submission-details-shared/components/next-back-button/next-back-button.component';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';
import { CoverageData } from '../../../../../../../modules/submission/data/coverages/coverages.data';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { HistoricalCoverageValidationService } from '../../../../../../../core/services/validations/historical-coverage-validation.service';
import { SubmissionNavValidateService } from '../../../../../../../core/services/navigation/submission-nav-validate.service';

@Component({
  selector: 'app-historical-coverage',
  templateUrl: './historical-coverage.component.html',
  styleUrls: ['./historical-coverage.component.scss']
})
export class HistoricalCoverageComponent implements OnInit {

  constructor(private router: Router,
    public location: Location,
    public coverageData: CoverageData,
    public submissionData: SubmissionData,
    public formValidation: SubmissionNavValidateService,
    public historicalValidation: HistoricalCoverageValidationService,
    private menuOverrideService: MenuOverrideService) { }

  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;
  ngOnInit() {
  }

  public onClick(clickType?) {
    this.nextBack.location = this.location;
    const prevUrl = this.nextBack.getPrev();
    const nextUrl = this.nextBack.getNext();
    this.coverageData.coverageForms.historicalCoverageForm.markAllAsTouched();
    this.coverageData.isTouched = true;
    switch (clickType) {
      case ClickTypes.Back:
        this.router.navigate([prevUrl]);
        break;
      case ClickTypes.Next:
        this.menuOverrideService.overrideSideBarBehavior();
        this.router.navigate([nextUrl]);
        break;
    }
  }
}
