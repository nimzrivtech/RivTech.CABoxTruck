import { PolicyIssuanceData } from '../../../../../../modules/policy/data/policy-issuance.data';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { RaterApiData } from '@app/modules/submission/data/rater-api.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import Utils from '@app/shared/utilities/utils';
import NotifUtils from '@app/shared/utilities/notif-utils';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { Observable, of, Subject } from 'rxjs';
import { catchError, delay, finalize, map, switchMap, take, takeUntil, tap } from 'rxjs/operators';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { EndorsementProcessStatusLogRetriever } from '@app/modules/policy/data/endorsement-process-status-log-retriever';
import { EndorsementProcessModalComponent } from './endorsement-process-modal/endorsement-process-modal.component';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { EffectiveDateModalComponent } from './effective-date-modal/effective-date-modal.component';
import { PolicyNavValidateService } from '@app/core/services/navigation/policy-nav-validate.service';

@Component({
  selector: 'app-policy-issuance',
  templateUrl: './policy-issuance.component.html',
  styleUrls: ['./policy-issuance.component.scss']
})
export class PolicyIssuanceComponent implements OnInit, OnDestroy {

  private readonly destroy$ = new Subject();
  endorsementProcessModal: BsModalRef;
  effectiveDateModal: BsModalRef;

  constructor(
    private raterApiData: RaterApiData,
    private submissionData: SubmissionData,
    private policyIssuanceData: PolicyIssuanceData,
    private riskSpecificsData: RiskSpecificsData,
    private modalService: BsModalService,
    private policySummaryData: PolicySummaryData,
    private policyNavValidateService: PolicyNavValidateService,
    private toastr: ToastrService,
    private datePipe: DatePipe,
  ) { }


  ngOnInit() {
    Utils.blockUI();
    // Utils.unblockUI();
    console.log('Policy Issuance component init.');
    const riskDetail = this.submissionData.riskDetail;

    const bindOption = Number(riskDetail?.binding?.bindOptionId ?? 1);
    this.riskSpecificsData.resetGeneralLiabilityAndCargoIfDisabledInBindOption(bindOption);
    this.policyIssuanceData.initiateFormFields();
    this.policyIssuanceData.clearPolicyChanges();
    this.policyIssuanceData.clearPremiumChangesTable();
    //this.policyNavValidateService.validateCategories();

    /**
     * No processing of endorsement if policy is already canceled.
     */
    if (this.submissionData.isCancelledPolicy) {
      Utils.unblockUI();
      this.policyIssuanceData.clearPremiumChangesTable(); // remove table values
      return;
    }
    
    // if (this.policySummaryData.isPendingEndorsement) {
    //   this.policyIssuanceData.getPolicyChanges$().pipe(takeUntil(this.destroy$)).subscribe();
    // }

    /**
     * No processing if there's missing required field data.
     */
    if (!this.raterApiData.validateRequiredToRatePremiumModules()) {
      Utils.unblockUI();
      return;
    }
    
    this.policyIssuanceData.isPolicyLocked$('InProgress').subscribe();
    this.policyIssuanceData.isIssuanceFailed$('Failed').subscribe();
    this.policyIssuanceData.isIssuanceLoadSuccessful$('Success', 'Issuance').subscribe();
    this.policyIssuanceData.isIssuanceDefaultLoadSuccessful$('Success', 'IssuanceDefault').subscribe();
    this.policyIssuanceData.getPolicyChanges$().pipe(takeUntil(this.destroy$)).subscribe(changes => {
      this.policyIssuanceData.hasEndorsementEffectiveDatePrompted$().pipe(takeUntil(this.destroy$)).subscribe(prompted => {
       
        const isIssuancePolicyLocked = typeof this.policySummaryData.policyNotification != 'undefined' && this.policySummaryData.policyNotification != null;
          
        const isIssuanceFailed = typeof this.policyIssuanceData.issuanceFailedNotification != 'undefined' && this.policyIssuanceData.issuanceFailedNotification != null;
  
        if(isIssuancePolicyLocked == false) {
          const isIssuanceLoadSuccessful = typeof this.policyIssuanceData.issuanceNotification != 'undefined' && this.policyIssuanceData.issuanceNotification != null;
          const isIssuanceDefaultLoadSuccessful = typeof this.policyIssuanceData.issuanceDefaultNotification != 'undefined' && this.policyIssuanceData.issuanceDefaultNotification != null;

          let isPendingChanged = false;
  
          if(isIssuanceLoadSuccessful) {
            const notificationDetails = JSON.parse(this.policyIssuanceData.issuanceNotification.notificationDetails);

            isPendingChanged = JSON.stringify(this.policyIssuanceData.policyChanges) != notificationDetails.policyChanges;
            Utils.unblockUI();
          }
          else if(isIssuanceDefaultLoadSuccessful) {
            const notificationDetails = JSON.parse(this.policyIssuanceData.issuanceDefaultNotification.notificationDetails);

            isPendingChanged = JSON.stringify(this.policyIssuanceData.policyChanges) != notificationDetails.policyChanges;
            Utils.unblockUI();
          }
  
          if (prompted == true && this.policySummaryData.isPendingEndorsement && ((isIssuanceLoadSuccessful == false && isIssuanceDefaultLoadSuccessful == false) || isPendingChanged == true)) {
  
          var effectiveDate = this.policyIssuanceData.issuanceForm.controls["effectiveDate"].value.singleDate.jsDate;
          var effectiveDateText = this.datePipe.transform(effectiveDate, "MM-dd-yyyy");
          var effectiveDate$ = this.policyIssuanceData.updateEndorsementEffectiveDate(riskDetail.id, effectiveDateText);
  
            this.policyIssuanceData.triggerIssuanceBackgroundProcess("Issuance");
          }
          else if(prompted != true && this.policySummaryData.isPendingEndorsement) {
            Utils.unblockUI();
            this.showEffectiveDateModal();
          }
          else if(isIssuanceLoadSuccessful == false && isIssuanceDefaultLoadSuccessful == false)
            {
              this.policyIssuanceData.triggerIssuanceDefaultBackgroundProcess("IssuanceDefault");
            }
          else {
            this.policyIssuanceData.loadPremiumChanges$(false).subscribe();
            Utils.unblockUI();
          }
        }
        else {
          Utils.unblockUI();
        }
      });
    }
    );
    

  }

  showEndorsementProcessModal() {
    if (this.endorsementProcessModal == null) {
      this.endorsementProcessModal = this.modalService.show(EndorsementProcessModalComponent, {
        keyboard: false,
        backdrop: 'static',
        ignoreBackdropClick: true,
        focus: true,
        animated: true,
        class: 'endorsement-process-modal  modal-dialog-centered',
      });
      this.endorsementProcessModal.content?.close.subscribe(() => {
        this.endorsementProcessModal?.hide();
      });
    }
  }

  showEffectiveDateModal() {
    if (this.effectiveDateModal == null) {
      this.effectiveDateModal = this.modalService.show(EffectiveDateModalComponent, {
        initialState: {
          effectiveDate: this.submissionData.riskDetail.endorsementEffectiveDate
        },
        keyboard: false,
        backdrop: 'static',
        ignoreBackdropClick: true,
        focus: true,
        animated: true,
        class: 'modal-dialog-centered',
      });

      this.effectiveDateModal.content?.close.pipe(takeUntil(this.destroy$)).subscribe((effectiveDate: Date) => {
        this.effectiveDateModal?.hide();
        if (effectiveDate != null) {
          // Patch value of effectiveDate to trigger
          // loading changes of endorsement.

          this.policyIssuanceData.updateEndorsementEffectiveDatePrompt$(true).pipe(
            takeUntil(this.destroy$),
            delay(500),
          ).subscribe(() => {
              this.policyIssuanceData.issuanceForm.controls['effectiveDate'].patchValue({
                singleDate: {
                  jsDate: new Date(effectiveDate)
                }
              });
          });
        }
      });
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.endorsementProcessModal?.hide();
    this.effectiveDateModal?.hide();
  }
}
