import { Component, OnDestroy, OnInit } from '@angular/core';
import { PolicyIssuanceData } from '@app/modules/policy/data/policy-issuance.data';
import { PreloadingStatus } from '@app/modules/rating/models/preloading-status';
import { RaterApiData } from '@app/modules/submission/data/rater-api.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import Utils from '@app/shared/utilities/utils';
import { of, Subject, Subscription } from 'rxjs';
import { switchMap, take, takeUntil, tap } from 'rxjs/operators';
import NotifUtils from '@app/shared/utilities/notif-utils';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { EndorsementProcessModalComponent } from '../endorsement-process-modal/endorsement-process-modal.component';
import { DatePipe } from '@angular/common';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';

@Component({
  selector: 'app-policy-changes-section',
  templateUrl: './policy-changes-section.component.html',
  styleUrls: ['./policy-changes-section.component.scss']
})
export class PolicyChangesSectionComponent implements OnInit, OnDestroy {

  private readonly destroy$ = new Subject();
  subscriptions: Subscription[] = [];
  hideEndorsementEffectiveDate: boolean = false;
  hidePolicyChanges: boolean = false;
  hideEndorsementText: boolean = false;
  preloadStatus: PreloadingStatus;

  lastSelectedDate: Date = new Date();

  endorsementProcessModal: BsModalRef;

  get endorsementEffectiveDateTitle() {
    if (this.preloadStatus === PreloadingStatus.InProgress) {
      return "Preloading rater...";
    } else if (this.preloadStatus === PreloadingStatus.Failed) {
      return "Preloading rater failed.";
    }
  }

  get canRate() {
    return this.preloadStatus === PreloadingStatus.Complete;
  }

  get isPendingEndorsement() { 
    return this.policySummaryData.isPendingEndorsement && !this.policySummaryData.isPolicyLocked;
  }

  get isPolicyChangesMoreThan10() {
    return  this.policyChangesCount > 10;
  }

  get policyChangesCount() {
    return (this.policyIssuanceData.policyChanges?.length ?? 0);
  }

  constructor(
    public policyIssuanceData: PolicyIssuanceData,
    private submissionData: SubmissionData,
    private raterApiData: RaterApiData,
    private modalService: BsModalService,
    private datePipe: DatePipe,
    private policySummaryData: PolicySummaryData
  ) {
    this.preloadStatus = PreloadingStatus.InProgress;
  }

  ngOnInit() {
    if (this.submissionData.isCancelledPolicy) { return; } // if policy is cancelled, should not continue here
    this.raterApiData.preloadingStatus.subscribe((status) => {
      if (status != null) { this.preloadStatus = status; }
    });

    var effSub = this.form.get('effectiveDate').valueChanges
                  .subscribe(this.effectiveDateChanged.bind(this));

    this.subscriptions.push(effSub);
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();

    if(this.endorsementProcessModal != null) {
      this.endorsementProcessModal.hide();
    }

    if (this.submissionData.isCancelledPolicy) { return; } // if policy is cancelled, should not continue here
    this.subscriptions.forEach(subs => subs.unsubscribe());
    console.info('Removed subscriptions...');
  }

  ToggleEndorsementEffectiveDate() {
    this.hideEndorsementEffectiveDate = !this.hideEndorsementEffectiveDate;
  }

  TogglePolicyChanges() {
    this.hidePolicyChanges = !this.hidePolicyChanges;
  }

  ToggleEndorsementText() {
    this.hideEndorsementText = !this.hideEndorsementText;
  }

  endorsementEffectiveDateChanged() {
    this.policyIssuanceData.isLoadingChanges = false;
  }

  get hasError() { return this.policyIssuanceData.hasError; }

  get form() { return this.policyIssuanceData.issuanceForm; }

  effectiveDateChanged(value) {
    if (this.policyIssuanceData.isNewlyIssued) { return; } // date changes upon issuance, should not continue here
    if (this.policyIssuanceData.isLoadingChanges) { return; } // if loading onInit already, should not continue here

    let selectedDate = value.singleDate.jsDate;
    const riskDetail = this.submissionData.riskDetail;

    //Utils.blockUIWithMessage("Updating effective date.");

    var formattedDate = this.datePipe.transform(selectedDate, "MM-dd-yyyy");
    this.policyIssuanceData.updateEndorsementEffectiveDate(riskDetail.id, formattedDate, true).pipe(
      takeUntil(this.destroy$),
      tap((result) => {
        this.lastSelectedDate = result;
        riskDetail.endorsementEffectiveDate = new Date(result);
      }),
    ).subscribe(() => {
        //Utils.unblockUI();
        // this.endorsementProcessModal = this.modalService.show(EndorsementProcessModalComponent, {
        //   keyboard: false,
        //   backdrop: 'static',
        //   ignoreBackdropClick: true,
        //   focus: true,
        //   animated: true,
        //   class: 'endorsement-process-modal modal-dialog-centered',
        // });
        // this.endorsementProcessModal.content?.close.subscribe(() => this.endorsementProcessModal.hide());
        
        this.policyIssuanceData.triggerIssuanceBackgroundProcess("Issuance");
    }, (e) => {
      console.error(e);
      if(e.status == 400) {
        NotifUtils.showWarning(e.error);
      } else {
        NotifUtils.showError("Error occured while updating endorsement effective date.");
      }
    });
  }
}
