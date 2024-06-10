import {Component, OnDestroy, OnInit} from '@angular/core';
import { ApplicantData } from '../../../../../../modules/submission/data/applicant/applicant.data';

import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { NavigationEnd, NavigationStart, Router } from '@angular/router';
import { catchError, finalize, map, takeUntil, tap } from 'rxjs/operators';
import { Observable, of, Subject, throwError } from 'rxjs';
import { createPolicyDetailsMenuItems } from '../../policy-details-navitems';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { LayoutService } from '@app/core/services/layout/layout.service';
import * as _ from 'lodash';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { PathConstants } from '@app/shared/constants/path.constants';
import FormUtils from '@app/shared/utilities/form.utils';
import { PolicySummaryHeaderConstants } from '@app/shared/constants/policy-header.constants';
import { CanComponentDeactivate } from '@app/core/guards/candeactivate.guard';
import { PolicyNavValidateService } from '@app/core/services/navigation/policy-nav-validate.service';

@Component({
  selector: 'app-policy-applicant',
  templateUrl: './policy-applicant.component.html',
  styleUrls: ['./policy-applicant.component.scss']
})
export class PolicyApplicantComponent implements OnInit, CanComponentDeactivate {

  private reset$ = new Subject();
  private newCategoryRoute: string;
  private currentCategoryRoute: string;

  constructor(private applicantData: ApplicantData,
    private router: Router,
    private layoutService: LayoutService,
    private submissionData: SubmissionData,
    private riskSpecificsData: RiskSpecificsData,
    private policyNavSaving: PolicyNavValidateService,
    private policySummaryData: PolicySummaryData) { }

  ngOnInit() {
    // this.applicantData.initiateFormFields();
    this.currentCategoryRoute = this.getCurrentCategoryRoute();
    //this.savingUponNavigate();
    // this.applicantData.populateNameAndAddressFields();
    // this.applicantData.populateBusinessDetailsFields();
    // this.applicantData.populateFilingsInformationFields();
    // this.applicantData.applicantForms.nameAndAddressForm.markAllAsTouched();
  }

  canDeactivate(): Observable<boolean> | boolean {
    return this.savingUponNavigate()
      .pipe(
        map(val => {
          return true;
        }),
        catchError(error => {
          console.log('Error saving upon navigate.');
          return of(false); //return from(['A','B','C'])
        })
      );
  }

  // routerNavigation(): Observable<any> {
  //   return this.router.events.filter(event => event instanceof NavigationStart)
  //     .pipe(takeUntil(this.reset$));
  // }

  savingUponNavigate(): Observable<any> {
    //this.router.events
      //.filter(event => event instanceof NavigationEnd)
      //.pipe(takeUntil(this.reset$)).subscribe((event: NavigationEnd) => {
        //this.newCategoryRoute = this.getCurrentCategoryRoute();
        // if (['dashboard', 'login'].includes(this.newCategoryRoute, 0)) {
        //   return of(false);
        // }
        // console.log(this.newCategoryRoute);
        this.submissionData.validationList.applicantNameAndAddress = this.checkNameAndAddressSection;
        this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificsData.hiddenNavItems));

        //if (this.currentCategoryRoute === PathConstants.Submission.Applicant.NameAndAddress) {
          if (this.policySummaryData.canEditPolicy) {
            return this.applicantData.updateNameAndAddressMidterm$().pipe(
              tap(() => this.policySummaryData.updateSubtatusForPendingEndorsement()),
              catchError((err) => throwError(err)),
              finalize(() => {
                this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificsData.hiddenNavItems));
              })
            )
          }
        //}
        //this.currentCategoryRoute = this.newCategoryRoute;
      //});
      return of({});
  }

  private getCurrentCategoryRoute(): string {
    const splitUrl = _.split(this.router.url, '/');
    const currentCategoryIndex = splitUrl.length - 1;
    return splitUrl[currentCategoryIndex];
  }

  get checkNameAndAddressSection() {
    this.applicantData.applicantForms.nameAndAddressForm.markAllAsTouched();
    return FormUtils.validateForm(this.applicantData.applicantForms.nameAndAddressForm);
  }
  
}
