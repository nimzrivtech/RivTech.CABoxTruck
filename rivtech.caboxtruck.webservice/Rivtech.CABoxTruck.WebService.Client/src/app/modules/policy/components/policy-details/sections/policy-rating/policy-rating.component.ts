import { Component, OnInit, ViewChild } from '@angular/core';
import { UrlTree } from '@angular/router';
import { CanComponentDeactivate } from '@app/core/guards/candeactivate.guard';
import { NavigationService } from '@app/core/services/navigation/navigation.service';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { QuoteLimitsData } from '@app/modules/submission/data/quote/quote-limits.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { ClickTypes } from '@app/shared/constants/click-type.constants';
import { PathConstants } from '@app/shared/constants/path.constants';
import NotifUtils from '@app/shared/utilities/notif-utils';
import Utils from '@app/shared/utilities/utils';
import { forkJoin, Observable, of } from 'rxjs';
import { catchError, delay, map, switchMap, tap, timeout } from 'rxjs/operators';
import { PolicyRatingFactorsComponent } from './policy-rating-factors/policy-rating-factors.component';

@Component({
  selector: 'app-policy-rating',
  templateUrl: './policy-rating.component.html',
  styleUrls: ['./policy-rating.component.scss']
})
export class PolicyRatingComponent implements OnInit, CanComponentDeactivate {

  @ViewChild(PolicyRatingFactorsComponent) policyRatingFactorsComponent: PolicyRatingFactorsComponent; 

  constructor(
    private navigationService: NavigationService,
    private policySummaryData: PolicySummaryData,
    private submissionData: SubmissionData,
    private quoteLimitsData: QuoteLimitsData
  ) { }

  ngOnInit() {
    this.updateFactorSettings();
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Submission.FormsSelection.Index);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.Premium);
        break;
    }
  }

  updateFactorSettings() {
    let raterVersion = this.submissionData.riskDetail.raterVersionId;
    this.quoteLimitsData.updateFactorModSettings(raterVersion);
    console.log(this.quoteLimitsData.factorModsSettings);
  }

  canDeactivate() : Observable<boolean>  {
    if(!this.policySummaryData.canEditPolicy) 
      return of(true);

    Utils.blockUI();
    var saveFactors = this.policyRatingFactorsComponent.saveFactors();

    let savingObservables = forkJoin([saveFactors]);

    return savingObservables.pipe(
      switchMap(() => this.policySummaryData.updatePendingEndorsement$()),
      map(() => true),
      tap(() => Utils.unblockUI()),
      catchError((err) => {
        console.error(err);
        NotifUtils.showError("Error saving rating data.");
        return of(false);
      })
    );
  }
}
