import { Component, OnInit, ViewChild } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { NextBackButtonComponent } from '../../../submission-details-shared/components/next-back-button/next-back-button.component';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';
import { FormsData } from '../../../../../../../modules/submission/data/forms/forms.data';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { SubmissionNavValidateService } from '../../../../../../../core/services/navigation/submission-nav-validate.service';
import { RaterApiData } from '../../../../../../../modules/submission/data/rater-api.data';
import Utils from '../../../../../../../shared/utilities/utils';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';
import { ErrorMessageConstant } from '../../../../../../../shared/constants/error-message.constants';
import { BehaviorSubject } from 'rxjs';
import { QuoteLimitsData } from '../../../../../../../modules/submission/data/quote/quote-limits.data';
import { PathConstants } from '../../../../../../../shared/constants/path.constants';
import { LayoutService } from '../../../../../../../core/services/layout/layout.service';
import { createSubmissionDetailsMenuItems } from '../../../submission-details-navitems';
import { RiskSpecificsData } from '../../../../../../../modules/submission/data/risk-specifics.data';

@Component({
  selector: 'app-forms-page',
  templateUrl: './forms-page.component.html',
  styleUrls: ['./forms-page.component.scss']
})
export class FormsPageComponent implements OnInit {
  isReady = new BehaviorSubject<boolean>(false);
  constructor(
    public location: Location,
    private router: Router,
    private menuOverrideService: MenuOverrideService,
    public formsData: FormsData,
    private submissionData: SubmissionData,
    private submissionNavValidateService: SubmissionNavValidateService,
    private raterApiData: RaterApiData,
    private quoteLimitsData: QuoteLimitsData,
    private submissionNavValidationService: SubmissionNavValidateService,
    private layoutService: LayoutService,
    private riskSpecificsData: RiskSpecificsData
  ) { }

  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;
  ngOnInit() {
    if (this.raterApiData.disableCalculatePremium === false) {
      return;
    }

    this.validateCategories();
    this.raterApiData.preloadRater().subscribe();
  }

  public onClick(clickType?) {
    this.nextBack.location = this.location;
    const prevUrl = this.nextBack.getPrev();
    const nextUrl = this.nextBack.getNext();
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

  get hasInvalid() {
    return this.submissionNavValidateService.hasInvalid();
  }

  get hasQuoteIssued() {
    const preBindQuote = this.submissionData.riskDetail?.riskDocuments?.some(f => f.fileName === 'Pre-Bind Quote' && f.isConfidential);
    if (preBindQuote === true) {
      this.submissionData.isQuoteIssued = preBindQuote;
    }
    if (this.submissionData.isQuoteIssued === true) {
      return true;
    }
    return false;
  }

  validateCategories(): void {
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Applicant.NameAndAddress);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Applicant.BusinessDetails);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Applicant.FilingsInformation);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Applicant.AdditionalInterest);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Broker.Index);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Limits.LimitsPage);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Vehicle.Index);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Driver.Index);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Coverages.HistoricalCoverage);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.DestinationInfo);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.DriverInfo);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.DotInfo);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.MaintenanceSafety);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.UWQuestions);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Quote.Index);
    this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList,
      this.riskSpecificsData.hiddenNavItems));
  }
}
