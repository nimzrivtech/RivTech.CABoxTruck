import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router, UrlTree } from '@angular/router';
import { LayoutService } from '@app/core/services/layout/layout.service';
import { NavigationService } from '@app/core/services/navigation/navigation.service';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { DriverDto, DriverHeaderDto } from '@app/shared/models/submission/Driver/DriverDto';
import { DriverIncidentDto } from '@app/shared/models/submission/Driver/DriverIncidentDto';
import { DriverData } from '../../../../../../modules/submission/data/driver/driver.data';
import { ClickTypes } from '../../../../../../shared/constants/click-type.constants';
import { PathConstants } from '../../../../../../shared/constants/path.constants';
import * as _ from 'lodash';
import { Observable, of, Subject, throwError } from 'rxjs';
import { catchError, map, take, takeUntil, tap } from 'rxjs/operators';
import { createPolicyDetailsMenuItems } from '../../policy-details-navitems';
import { PolicyNavValidateService } from '@app/core/services/navigation/policy-nav-validate.service';
import { CanComponentDeactivate } from '@app/core/guards/candeactivate.guard';
import NotifUtils from '@app/shared/utilities/notif-utils';
import Utils from '@app/shared/utilities/utils';

@Component({
  selector: 'app-policy-drivers',
  templateUrl: './policy-drivers.component.html',
  styleUrls: ['./policy-drivers.component.scss']
})
export class PolicyDriversComponent implements OnInit, CanComponentDeactivate {

  private reset$ = new Subject();
  private newCategoryRoute: string;
  private currentCategoryRoute: string;

  constructor(private navigationService: NavigationService,
              private riskSpecificsData: RiskSpecificsData,
              public driverData: DriverData,
              private router: Router,
              private layoutService: LayoutService,
              private submissionData: SubmissionData,
              private policyNavValidateService: PolicyNavValidateService,
              private policySummaryData: PolicySummaryData) { }

  ngOnInit() {
    this.driverData.initiateFormGroup(new DriverDto());
    this.driverData.initiateHeaderFormGroup(new DriverHeaderDto());
    this.driverData.initiateFormIncident(new DriverIncidentDto());
    this.currentCategoryRoute = this.getCurrentCategoryRoute();
    //this.savingUponNavigate();
  }


  canDeactivate() : Observable<boolean> {
    Utils.blockUI();

    return this.driverData.updateDriverExclusionEndorsementForm$(this.submissionData.riskDetail.id).pipe(
      take(1),
      tap(() => this.policySummaryData.updateSubtatusForPendingEndorsement()),
      tap(() => {
        this.newCategoryRoute = this.getCurrentCategoryRoute();
        if (['dashboard', 'login'].includes(this.newCategoryRoute, 0)) {
          return true;
        }
        if (this.currentCategoryRoute === PathConstants.Submission.Driver.Index) {
          this.policyNavValidateService.validateCurrentCategory(PathConstants.Submission.Driver.Index);
          this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificsData.hiddenNavItems));
        }
        Utils.unblockUI();
      }),
      map(() => true), // return true if success
      catchError((err) =>{ // show notif error if failed
        console.error(err);
        let errorMessage = `Error occured. `;
        if(err?.message !== undefined) {
          errorMessage += `\n${err?.message}`;
        } 
        NotifUtils.showError(errorMessage);
        return of(false);
      }),
    );
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Submission.Vehicle.Index);
        break;
      case ClickTypes.Next:
        if (this.riskSpecificsData.hiddenNavItems.includes('Historical Coverage') && this.riskSpecificsData.hiddenNavItems.includes('Claims History')) {
          this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.RiskSpecifics.Index}/${PathConstants.Submission.RiskSpecifics.DestinationInfo}`);
        } else {
          this.navigationService.navigatePolicyRoute(PathConstants.Submission.Coverages.Index);
        }
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
        if (this.currentCategoryRoute === PathConstants.Submission.Driver.Index) {
          this.policyNavValidateService.validateCurrentCategory(PathConstants.Submission.Driver.Index);
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

}
