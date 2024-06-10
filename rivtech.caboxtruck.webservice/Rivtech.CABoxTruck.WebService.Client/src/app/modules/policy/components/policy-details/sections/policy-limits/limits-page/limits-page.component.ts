import { Component, HostListener, Injectable, OnInit } from '@angular/core';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { NavigationService } from '../../../../../../../core/services/navigation/navigation.service';
import { PathConstants } from '../../../../../../../shared/constants/path.constants';
import { LimitsData } from '@app/modules/submission/data/limits/limits.data';
import { Router, NavigationEnd, NavigationStart, CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { LayoutService } from '@app/core/services/layout/layout.service';
import { PolicyNavValidateService } from '@app/core/services/navigation/policy-nav-validate.service';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { VehicleData } from '@app/modules/submission/data/vehicle/vehicle.data';
import { catchError, concatMap, finalize, map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { createPolicyDetailsMenuItems } from '../../../policy-details-navitems';
import { from, Observable, of, Subject, throwError } from 'rxjs';
import * as _ from 'lodash';
import FormUtils from '@app/shared/utilities/form.utils';
import { LimitsDefaultValues } from '@app/shared/constants/limits-defaults.constants';
import { FormsData } from '@app/modules/submission/data/forms/forms.data';
import { CanComponentDeactivate } from '@app/core/guards/candeactivate.guard';
import Utils from '@app/shared/utilities/utils';

@Component({
  selector: 'app-limits-page',
  templateUrl: './limits-page.component.html',
  styleUrls: ['./limits-page.component.scss']
})
export class LimitsPageComponent implements OnInit, CanComponentDeactivate {

  private reset$ = new Subject();
  private newCategoryRoute: string;
  private currentCategoryRoute: string;
  private saveCategoryEndSource = new Subject<void>();
  private saveCategoryEnded = this.saveCategoryEndSource.asObservable();

  constructor(
    private navigationService: NavigationService,
    private router: Router,
    private limitsData: LimitsData,
    public vehicleData: VehicleData,
    private layoutService: LayoutService,
    private submissionData: SubmissionData,
    private riskSpecificsData: RiskSpecificsData,
    private policyNavValidationService: PolicyNavValidateService,
    private formsData: FormsData,
    private policySummaryData: PolicySummaryData) { }

  ngOnInit() {
    this.limitsData.initiateFormFields();
    // this.riskSpecificsData.initiateFormFields(new RiskDTO());
    this.currentCategoryRoute = this.getCurrentCategoryRoute();
  }

  canDeactivate(): Observable<boolean> | boolean {
    Utils.blockUI();
    return this.savingUponNavigate()
      .pipe(
        map(val => {
          return true;
        }),
        catchError(error => {
          console.log('Error saving upon navigate.');
          return of(false); //return from(['A','B','C'])
        }),
        finalize(() => Utils.unblockUI())
      );
  }

  routerNavigation(): Observable<any> {
    return this.router.events.filter(event => event instanceof NavigationStart)
      .pipe(takeUntil(this.reset$));
  }

  savingUponNavigate(): Observable<any> {
    console.log('Saving upon navigate....');
    //if (this.isPolicyUrl) { return of(); }
    //this.newCategoryRoute = this.getCurrentCategoryRoute();

    //if (['dashboard', 'login'].includes(this.newCategoryRoute, 0)) {
    //  return of();
    //}

    // console.log(this.newCategoryRoute);
    //if (this.currentCategoryRoute === PathConstants.Submission.Limits.LimitsPage) {
      this.submissionData.validationList.limits = this.checkLimitsSection;
      this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificsData.hiddenNavItems));

      if (this.policySummaryData.canEditPolicy) {
        return this.limitsData.saveRiskCoverageLimit().pipe(
          concatMap(() => this.vehicleData.updateOtherCoverages(this.limitsData.oldLimitsData, true)),
          switchMap(() => this.policySummaryData.updatePendingEndorsementObservable()),
          switchMap(() => this.riskSpecificsData.resetGeneralLiabilityCargo$()),
          switchMap(() => this.formsData.populateFields$()), // to trigger adding/removing GLMTC forms
          catchError((err) => throwError(err)),
          finalize(() => {
            this.vehicleData.isVehicleUpdated = true;
            this.submissionData.validationList.generalLiabilityCargo = this.generalLiabilityCargoValidStatus;
            this.submissionData.validationList.riskSpecifics = this.generalLiabilityCargoValidStatus;
            this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificsData.hiddenNavItems));
            this.saveCategoryEndSource.next();
            // Unsubscribe the router navigation to avoid multiple api calls in this block. 
            console.log('Saving upon navigate finished.');
          })
        );
      }
      return of({});
    //this.currentCategoryRoute = this.newCategoryRoute;
    //return of();
  }


  private getCurrentCategoryRoute(): string {
    const splitUrl = _.split(this.router.url, '/');
    const currentCategoryIndex = splitUrl.length - 1;
    return splitUrl[currentCategoryIndex];
  }

  get isPolicyUrl() {
    const splitUrl = _.split(this.router.url, '/');
    return splitUrl[0] === 'policies';
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Submission.Broker.Index);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Submission.Vehicle.Index);
        break;
    }
  }

  get checkLimitsSection() {
    this.limitsSection.markAllAsTouched();
    return FormUtils.validateForm(this.limitsSection);
  }

  get limitsSection() {
    return this.limitsData.limitsForm.limitsPageForm;
  }

  get generalLiabilityCargoValidStatus() {
    if (!this.riskSpecificsData?.hiddenNavItems?.includes('General Liability & Cargo')) {
      return this.checkGeneralLiabSection && this.checkCargoSection;
    } else {
      return true;
    }
  }

  get commoditiesHauled() {
    return this.riskSpecificsData?.commoditiesHauled;
  }

  get checkGeneralLiabSection() {
    if (!this.hasGLCoverage) { return true; }
    return this.hasGLCoverage && (this.genLiabData.generalLiability?.glAnnualPayroll ?? 0) > 0;
  }

  get checkCargoSection() {
    if (!this.hasCargo && !this.hasRefBrkdown) { return true; }
    return (this.genLiabData.commoditiesHauled.length > 0);
  }

  get genLiabData() {
    return this.submissionData.riskDetail?.generalLiabilityCargo;
  }

  get hasCargo() {
    return Number(this.limitsSection.value.cargoLimitId ?? 0) > LimitsDefaultValues.cargoId;
  }

  get hasRefBrkdown() {
    return Number(this.limitsSection.value.refCargoLimitId ?? 0) > LimitsDefaultValues.refId;
  }

  get hasGLCoverage() {
    return Number(this.limitsSection.value.glbiLimitId ?? 0) > LimitsDefaultValues.glId;
  }
}
