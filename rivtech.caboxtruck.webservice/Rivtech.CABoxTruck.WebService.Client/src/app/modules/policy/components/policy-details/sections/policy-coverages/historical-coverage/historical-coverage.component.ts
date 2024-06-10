import { Component, OnInit } from '@angular/core';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { PathConstants } from '../../../../../../../shared/constants/path.constants';
import { NavigationService } from '../../../../../../../core/services/navigation/navigation.service';
import { CoverageData } from '../../../../../../../modules/submission/data/coverages/coverages.data';
import { ApplicantData } from '../../../../../../../modules/submission/data/applicant/applicant.data';
import { BrokerGenInfoData } from '../../../../../../../modules/submission/data/broker/general-information.data';
import { BrokerInfoDto } from '../../../../../../../shared/models/submission/brokerinfo/brokerInfoDto';
import { NavigationEnd, Router } from '@angular/router';
import { LayoutService } from '@app/core/services/layout/layout.service';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { createPolicyDetailsMenuItems } from '../../../policy-details-navitems';
import FormUtils from '@app/shared/utilities/form.utils';
import * as _ from 'lodash';

@Component({
  selector: 'app-historical-coverage',
  templateUrl: './historical-coverage.component.html',
  styleUrls: ['./historical-coverage.component.scss']
})
export class HistoricalCoverageComponent implements OnInit {

  private reset$ = new Subject();
  private newCategoryRoute: string;
  private currentCategoryRoute: string;
  private saveCategoryEndSource = new Subject<void>();
  private saveCategoryEnded = this.saveCategoryEndSource.asObservable();

  constructor(private navigationService: NavigationService,
              private coverageData: CoverageData,
              private applicantData: ApplicantData,
              private brokerInfoData: BrokerGenInfoData,
              private router: Router,
              private layoutService: LayoutService,
              private submissionData: SubmissionData,
              private riskSpecificsData: RiskSpecificsData,
              private policySummaryData: PolicySummaryData) { }

  ngOnInit() {
    this.applicantData.initiateFormFields();
    this.coverageData.initiateFormFields();
    this.brokerInfoData.initiateFormGroup(new BrokerInfoDto());
    this.brokerInfoData.initiateBrokerFields();

    this.applicantData.populateBusinessDetailsFields();
    this.brokerInfoData.populateBrokerFields();
    this.currentCategoryRoute = this.getCurrentCategoryRoute();
    this.savingUponNavigate();
    this.coverageData.populateHistoryFields();
    this.coverageData.isTouched = true;
    this.coverageData.coverageForms.historicalCoverageForm.markAllAsTouched();
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Submission.Driver.Index);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Submission.Claims.Index);
        break;
    }
  }

  savingUponNavigate() {
    this.router.events
      .filter(event => event instanceof NavigationEnd)
      .pipe(takeUntil(this.reset$)).subscribe((event: NavigationEnd) => {
        this.newCategoryRoute = this.getCurrentCategoryRoute();
        if (['dashboard', 'login'].includes(this.newCategoryRoute, 0)) {
          return;
        }
        // console.log(this.newCategoryRoute);
        if (this.currentCategoryRoute === PathConstants.Submission.Coverages.HistoricalCoverage) {
          if (this.policySummaryData.canEditPolicy) {
            this.coverageData.saveRiskHistory(() => this.saveCategoryEndSource.next());
          }
          this.submissionData.validationList.historicalCoverages = this.checkCoveragesSection;
          this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificsData.hiddenNavItems));
        }
        this.currentCategoryRoute = this.newCategoryRoute;
      });
  }

  private getCurrentCategoryRoute(): string {
    const splitUrl = _.split(this.router.url, '/');
    const currentCategoryIndex = splitUrl.length - 1;
    return splitUrl[currentCategoryIndex];
  }

  get checkCoveragesSection() {
    this.coverageData.coverageForms.historicalCoverageForm.markAllAsTouched();
    return FormUtils.validateForm(this.coverageData.coverageForms.historicalCoverageForm);
  }
}
