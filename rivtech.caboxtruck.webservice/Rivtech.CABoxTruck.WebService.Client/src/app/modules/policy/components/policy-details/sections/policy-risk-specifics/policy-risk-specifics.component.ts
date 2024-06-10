import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { LayoutService } from '@app/core/services/layout/layout.service';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { PathConstants } from '@app/shared/constants/path.constants';
import { RiskDTO } from '@app/shared/models/risk/riskDto';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { createPolicyDetailsMenuItems } from '../../policy-details-navitems';
import * as _ from 'lodash';
import FormUtils from '@app/shared/utilities/form.utils';

@Component({
  selector: 'app-policy-risk-specifics',
  templateUrl: './policy-risk-specifics.component.html',
  styleUrls: ['./policy-risk-specifics.component.scss']
})
export class PolicyRiskSpecificsComponent implements OnInit {

  private reset$ = new Subject();
  private newCategoryRoute: string;
  private currentCategoryRoute: string;

  constructor(private riskSpecificData: RiskSpecificsData,
              private submissionData: SubmissionData,
              private router: Router,
              private layoutService: LayoutService,
              private policySummaryData: PolicySummaryData) { }

  ngOnInit() {
    this.currentCategoryRoute = this.getCurrentCategoryRoute();
    this.riskSpecificData.retrieveDropDownValues();
    this.riskSpecificData.setBusinessRules();
    if (this.submissionData?.riskDetail) {
      this.riskSpecificData.initiateFormFields(this.submissionData.riskDetail);
    } else {
      this.riskSpecificData.initiateFormFields(new RiskDTO());
    }

    this.savingUponNavigate();
  }

  savingUponNavigate() {
    this.router.events
      .filter(event => event instanceof NavigationEnd)
      .pipe(takeUntil(this.reset$)).subscribe((event: NavigationEnd) => {
        this.newCategoryRoute = this.getCurrentCategoryRoute();
        if (['dashboard', 'login'].includes(this.newCategoryRoute, 0)) {
          return;
        }
        if (this.currentCategoryRoute === PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo) {
          if (this.policySummaryData.canEditPolicy) {
            this.riskSpecificData.saveGeneralLiabilityCargo();
          }
          this.submissionData.validationList.generalLiabilityCargo = this.generalLiabilityCargoValidStatus;
          this.submissionData.validationList.riskSpecifics = this.generalLiabilityCargoValidStatus;
          this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificData.hiddenNavItems));
        }
        this.currentCategoryRoute = this.newCategoryRoute;
      });
  }

  private getCurrentCategoryRoute(): string {
    const splitUrl = _.split(this.router.url, '/');
    const currentCategoryIndex = splitUrl.length - 1;
    return splitUrl[currentCategoryIndex];
  }

  get generalLiabilityCargoValidStatus() {
    if (!this.riskSpecificData?.hiddenNavItems?.includes('General Liability & Cargo')) {
      return this.checkGeneralLiabSection && this.checkCargoSection;
    } else {
      return true;
    }
  }

  get checkGeneralLiabSection() {
    if (!this.riskSpecificData.hasGLCoverage) { return true; }
    this.generalLiabAndCargoForm.markAllAsTouched();
    return this.riskSpecificData.hasGLCoverage && FormUtils.validateForm(this.generalLiabAndCargoForm);
  }

  get checkCargoSection() {
    this.cargoSection.markAllAsTouched();
    if (!this.riskSpecificData.hasGLCargoCoverage && !this.riskSpecificData.hasGLRefrigerationCoverage) { return true; }
    let sum = this.riskSpecificData.commoditiesHauled.reduce((acc, cur) => acc + Number(cur.percentageOfCarry), 0);
    sum = Math.round(sum * 100) / 100;
    return (this.commoditiesHauled.length > 0 && sum == 100) && FormUtils.validateForm(this.cargoSection);
  }

  get generalLiabAndCargoForm () {
    return this.riskSpecificData.riskSpecificsForms.generalLiabilityForm;
  }

  get cargoSection() {
    return this.riskSpecificData.riskSpecificsForms.cargoForm;
  }

  get commoditiesHauled () {
    return this.riskSpecificData?.commoditiesHauled;
  }

}
