import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { NavigationEnd, Router, NavigationStart } from '@angular/router';
import { LayoutService } from '@app/core/services/layout/layout.service';
import { PolicyNavValidateService } from '@app/core/services/navigation/policy-nav-validate.service';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { Observable,of, Subject } from 'rxjs';
import { catchError, finalize, map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { NavigationService } from '../../../../../../core/services/navigation/navigation.service';
import { ClickTypes } from '../../../../../../shared/constants/click-type.constants';
import { PathConstants } from '../../../../../../shared/constants/path.constants';
import { createPolicyDetailsMenuItems } from '../../policy-details-navitems';
import * as _ from 'lodash';
import { QuoteLimitsData } from '@app/modules/submission/data/quote/quote-limits.data';
import { RaterApiData } from '@app/modules/submission/data/rater-api.data';
import Utils from '@app/shared/utilities/utils';
import { NextBackBtnComponent } from '@app/shared/components/next-back-btn/next-back-btn.component';
import { CanComponentDeactivate } from '@app/core/guards/candeactivate.guard';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { VehicleData } from '@app/modules/submission/data/vehicle/vehicle.data';

@Component({
  selector: 'app-policy-vehicles',
  templateUrl: './policy-vehicles.component.html',
  styleUrls: ['./policy-vehicles.component.scss']
})
export class PolicyVehiclesComponent implements OnInit, CanComponentDeactivate {

  private reset$ = new Subject();
  private newCategoryRoute: string;
  private currentCategoryRoute: string;
  private saveCategoryEndSource = new Subject<void>();

  @Input() disableCalculate: boolean = false;

  constructor(private navigationService: NavigationService,
    private router: Router,
    private layoutService: LayoutService,
    private submissionData: SubmissionData,
    private riskSpecificsData: RiskSpecificsData,
    private policyNavValidateService: PolicyNavValidateService,
    private raterApiData: RaterApiData,
    private policySummaryData: PolicySummaryData,
    public vehicleData: VehicleData) { }

  ngOnInit() {
    this.currentCategoryRoute = this.getCurrentCategoryRoute();
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
        if (this.currentCategoryRoute === PathConstants.Submission.Vehicle.Index) {
          this.policyNavValidateService.validateCurrentCategory(PathConstants.Submission.Vehicle.Index);
          this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificsData.hiddenNavItems));
        }
        this.currentCategoryRoute = this.newCategoryRoute;
      });
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Submission.Limits.Index);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Submission.Driver.Index);
        break;
    }
  }

  canDeactivate(): Observable<boolean> | boolean {
    Utils.blockUI();
    return this.savingUponNavigate$()
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

  savingUponNavigate$(): Observable<any> {
    console.log('Saving upon navigate....');      
    if (this.policySummaryData.canEditPolicy) {
      this.vehicleData.onExitVehicleScreen(() => {
        this.saveCategoryEndSource.next();
        // Unsubscribe the router navigation to avoid multiple api calls in this block. 
        console.log('Saving upon navigate finished.');
      });
    }
    return of({});
  }

  private getCurrentCategoryRoute(): string {
    const splitUrl = _.split(this.router.url, '/');
    const currentCategoryIndex = splitUrl.length - 1;
    return splitUrl[currentCategoryIndex];
  }

  routerNavigation(): Observable<any> {
    return this.router.events.filter(event => event instanceof NavigationStart)
      .pipe(takeUntil(this.reset$));
  }

}
