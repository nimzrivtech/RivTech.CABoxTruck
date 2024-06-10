import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { FormArray } from '@angular/forms';
import { SelectItem } from '../../../../../../../shared/models/dynamic/select-item';
import { ErrorMessageConstant } from '../../../../../../../shared/constants/error-message.constants';
import { ClaimsHistoryoData } from '../../../../../../../modules/submission/data/claims/claims-history.data';
import { ApplicantData } from '../../../../../../../modules/submission/data/applicant/applicant.data';
import { CoverageData } from '../../../../../../../modules/submission/data/coverages/coverages.data';
import { PolicySummaryData } from '../../../../../../../modules/policy/data/policy-summary.data';
import { NavigationEnd, Router } from '@angular/router';
import { LayoutService } from '@app/core/services/layout/layout.service';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { PathConstants } from '@app/shared/constants/path.constants';
import { createPolicyDetailsMenuItems } from '../../../policy-details-navitems';
import FormUtils from '@app/shared/utilities/form.utils';
import * as _ from 'lodash';

@Component({
  selector: 'app-claims-history-section',
  templateUrl: './claims-history-section.component.html',
  styleUrls: ['./claims-history-section.component.scss']
})
export class ClaimsHistorySectionComponent implements OnInit, OnDestroy {
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;

  private reset$ = new Subject();
  private newCategoryRoute: string;
  private currentCategoryRoute: string;
  private saveCategoryEndSource = new Subject<void>();
  private saveCategoryEnded = this.saveCategoryEndSource.asObservable();
  lossRunErrorMessage = ErrorMessageConstant.lossRunErrorMessage;

  constructor(public Data: ClaimsHistoryoData,
              public submissionData: SubmissionData,
              public policySummaryData: PolicySummaryData,
              private applicantData: ApplicantData,
              private coverageData: CoverageData,
              private router: Router,
              private layoutService: LayoutService,
              private riskSpecificsData: RiskSpecificsData) { }

  get groundUpNetOptions(): SelectItem[] {
    return [
      { value: null, label: 'Please Select' },
      { value: 'G', label: 'G' },
      { value: 'N', label: 'N' },
    ]
  }

  ngOnInit() {
    this.currentCategoryRoute = this.getCurrentCategoryRoute();
    this.applicantData.initiateFormFields();
    this.applicantData.populateBusinessDetailsFields();
    this.coverageData.initiateFormFields();
    this.Data.initiateFormFields();
    this.coverageData.populateHistoryFields();
    this.Data.populateHistoryFields();
    this.Data.setRequiredValidator();
    this.Data.isTouched = true;
    this.savingUponNavigate();
  }

  ngOnDestroy() {
    this.revertDateIfInvalidInput();
  }

  get f() { return this.Data.formInfo.controls; }
  get fg() { return this.Data.formInfo; }

  private revertDateIfInvalidInput() {
    const currentHistories = this.submissionData.riskDetail?.claimsHistory;
    document.querySelectorAll('.loss-run-date input').forEach((dp, i) => {
      if (dp.className.includes('ng-invalid')) {
        const currentValue = currentHistories[i].lossRunDate;
        if (currentValue) {
          const runLossDates = <FormArray>this.Data.formInfo.controls['lossRunDate'];
          runLossDates.controls[i].patchValue({ isRange: false, singleDate: { jsDate: new Date(currentValue) } });
        }
      }
    });
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
        if (this.currentCategoryRoute === PathConstants.Submission.Claims.Index) {
          if (this.policySummaryData.canEditPolicy) {
            this.Data.save(() => {
              this.saveCategoryEndSource.next();
              this.policySummaryData.updateSubtatusForPendingEndorsement();
            });
          }
          this.submissionData.validationList.claimsHistory = this.checkClaimsSection;
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

  get checkClaimsSection() {
    this.fg.markAllAsTouched();
    return FormUtils.validateForm(this.fg);
  }
}
