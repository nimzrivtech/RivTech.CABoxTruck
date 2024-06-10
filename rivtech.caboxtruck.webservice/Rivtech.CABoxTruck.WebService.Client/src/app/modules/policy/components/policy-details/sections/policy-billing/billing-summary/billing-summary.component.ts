import { Component, OnInit } from '@angular/core';
import { PolicyBillingLabelsConstants } from '../../../../../../../shared/constants/policy-billing.labels.constants';
import { BaseClass } from '../../../../../../../shared/base-class';
import { PolicyBillingData } from '../../../../../data/policy-billing.data';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { SummaryData } from '../../../../../../submission/data/summary.data';
import { BillingService } from '../../../../../../../core/services/billing/billing.service';
import { BillingSummaryDTO } from '../../../../../../../shared/models/policy/billing/billing-summary.dto';
import { PostPaymentModalComponent } from '../payments/post-payments/post-payment-modal/post-payment-modal.component';
import Utils from '../../../../../../../shared/utilities/utils';
// import { ChangePaymentPlanModalComponent } from './change-payment-plan-modal/change-payment-plan-modal.component';
import { LvPayPlanOptions } from '../../../../../../../shared/constants/billing.options.constants';
import { PaymentPlanListConstants } from '../../../../../../../shared/constants/bind-and-issue.labels.constants';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';
import { takeUntil } from 'rxjs/operators';
import { LvRiskStatus } from '../../../../../../../shared/constants/risk-status';
// import { PolicyService } from 'app/core/services/submission/policy.service';
// import { OutstandingReinstatementRequirementsModalComponent } from '../../../../../app/modules/dashboard/components/dashboard/make-payment/outstanding-reinstatement-requirements-modal/outstanding-reinstatement-requirements-modal.component';
// import { ReinstatementRequirementsMetModalComponent } from '../../../../../app/modules/dashboard/components/dashboard/make-payment/reinstatement-requirements-met-modal/reinstatement-requirements-met-modal.component';
import { AllowedPaymentRangeDTO } from '../../../../../../../shared/models/policy/billing/allowed-payment-range.dto';
import { PaymentProcedure } from '../../../../../../../shared/enum/payment-procedure.enum';


import { SubmissionData } from '../../../../../../submission/data/submission.data';

@Component({
  selector: 'app-billing-summary',
  templateUrl: './billing-summary.component.html',
  styleUrls: ['./billing-summary.component.scss']
})
export class BillingSummaryComponent extends BaseClass implements OnInit {
  isOpen: boolean = true;

  summary: BillingSummaryDTO;

  hasActiveRenewalOffer: boolean;

  modalRef: BsModalRef | null;
  public billingSummaryConstants = PolicyBillingLabelsConstants.billingSummary;
  public paymentLabelConstants = PolicyBillingLabelsConstants.payments;
  public paymentPlanListConstants = PaymentPlanListConstants;

  constructor(
    public billingData: PolicyBillingData,
    public submissionData: SubmissionData,
    public billingService: BillingService,
    private modalService: BsModalService) {
    super();
  }

  ngOnInit() {
    const c = this.submissionData.riskDetail;
    const b = this.billingData.data;
    if (this.submissionData.riskDetail.riskId) {
      this.billingData.showSummary(this.submissionData.riskDetail.riskId);
    }
  }

  collapse(): void {
    this.isOpen = !this.isOpen;
  }

  onPaymentPlanValueChange(): void {

    // Utils.blockUI();
    // const riskId: string = this.billingData.data.risks[0].parentRiskId;
    // this.billingService.getChangePayplan(riskId).subscribe(data => {
    //   Utils.unblockUI();

    //   const initialState = {
    //     getChangePayplanResponse: data,
    //   };

    //   this.modalRef = this.modalService.show(ChangePaymentPlanModalComponent, {
    //     initialState,
    //     class: 'modal-md modal-dialog-centered',
    //     backdrop: 'static',
    //     keyboard: false
    //   });

    // });
  }

  changeValue(value: string): void {
    this.billingData.selectedPaymentPlanId = value;
  }

  getPaymentPlanName(code: string): string {
    return LvPayPlanOptions.find(result => result.code === code).description;
  }

  onOpenPaymentModal(): void {
    this.billingData.makePaymentData = null;

    if (this.billingData.data.status === 'Cancelled') {
      this.showReinstatementModal();
    } else {
      this.showMakePaymentModal();
    }
  }

  showMakePaymentModal(): void {
    Utils.blockUI();
    this.billingService.getAllowedPaymentRange(this.billingData.data.riskId)
      .pipe(takeUntil(this.stop$)).subscribe(res => {
        Utils.unblockUI();

        const initialState = {
          allowedPaymentRange: res,
          paymentProcedure: PaymentProcedure.PostBasic
        };

        this.modalService.show(PostPaymentModalComponent, {
          initialState,
          backdrop: true,
          ignoreBackdropClick: true,
          class: 'modal-md',
        });
      },
        err => {
          Utils.unblockUI();
          NotifUtils.showMultiLineError(err.error?.message);
        });
  }

  showReinstatementModal(): void {
    // Utils.blockUI();
    // this.policyService.getIsApprovedByUWForReinstatement(this.billingData.data.risks[0].parentRiskId).pipe(takeUntil(this.stop$)).subscribe(isApprovedByUW => {
    //   Utils.unblockUI();

    //   const outstandingRequirements: string[] = [];

    //   if (isApprovedByUW === false) {
    //     outstandingRequirements.push(PolicyBillingLabelsConstants.outstandingReinstatementRequirements.notApprovedByUW);

    //     let hasOutstandingBalance: boolean = false;
    //     if (this.billingData.summary.balance > 0) {
    //       outstandingRequirements.push(PolicyBillingLabelsConstants.outstandingReinstatementRequirements.outstandingBalance);
    //       hasOutstandingBalance = true;
    //     }

    //     Utils.unblockUI();
    //     this.showReinstatementOutstandingRequirementsModal(outstandingRequirements, hasOutstandingBalance);
    //   } else {
    //     Utils.unblockUI();
    //     this.showReinstatementRequirementsMetModal();
    //   }
    // },
    //   err => {
    //     Utils.unblockUI();
    //     NotifUtils.showMultiLineError(err.error?.message);
    //   });
  }

  showReinstatementOutstandingRequirementsModal(outstandingRequirementsList: string[], hasOutstandingBalance: boolean): void {
    // const allowedPaymentRange: AllowedPaymentRangeDTO = {
    //   minimumPaymentAmount: 0,
    //   maximumPaymentAmount: this.billingData.summary.balance
    // };

    // const initialState = {
    //   riskId: this.billingData.data.risks[0].parentRiskId,
    //   accessedFromPortal: false,
    //   outstandingRequirementsList: outstandingRequirementsList,
    //   hasOutstandingBalance: hasOutstandingBalance,
    //   allowedPaymentRange: allowedPaymentRange
    // };

    // this.modalRef = this.modalService.show(OutstandingReinstatementRequirementsModalComponent, {
    //   initialState,
    //   backdrop: true,
    //   ignoreBackdropClick: true,
    // });

    // this.modalRef.hide();
  }

  showReinstatementRequirementsMetModal(): void {
    // const initialState = {
    //   accessedFromPortal: false,
    //   balance: this.billingData.summary.balance,
    //   payoffAmount: this.billingData.summary.payoffAmount,
    //   pastDueAmount: this.billingData.summary.pastDueAmount,
    // };

    // this.modalRef = this.modalService.show(ReinstatementRequirementsMetModalComponent, {
    //   initialState,
    //   backdrop: true,
    //   ignoreBackdropClick: true,
    // });

    // this.modalRef.hide();
  }
}
