import { Component, OnInit } from '@angular/core';
import { PolicyBillingLabelsConstants } from '../../../../../../../shared/constants/policy-billing.labels.constants';
import { BaseClass } from '../../../../../../../shared/base-class';
import { PolicyBillingData } from '../../../../../data/policy-billing.data';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { PaymentDetailsModalComponent } from './payment-details-modal/payment-details-modal.component';
import { PostPaymentModalComponent } from './post-payments/post-payment-modal/post-payment-modal.component';
import { WriteOffModalComponent } from './write-off-modal/write-off-modal.component';
// import { AdjustmentModalComponent } from './adjustment-modal/adjustment-modal.component';
import { BillingService } from '../../../../../../../../app/core/services/billing/billing.service';
import { PaymentDetailsViewDTO } from '../../../../../../../shared/models/policy/billing/payment-details-view.dto';
import { catchError, takeUntil, tap } from 'rxjs/operators';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';
import { PolicyBillingConstants } from '../../../../../../../shared/constants/policy-billing.constants';
import { PaymentType } from '../../../../../../../shared/enum/payment-type.enum';
import Utils from '../../../../../../../shared/utilities/utils';
import { of } from 'rxjs';
import { ReportsService } from '@app/modules/dashboard/services/reports.service';
import { PaymentDTO } from '@app/shared/models/policy/billing/payment.dto';
import { DatePipe } from '@angular/common';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { PaymentViewDTO } from '@app/shared/models/policy/billing/payment-view.dto';
import { TableConstants } from '@app/shared/constants/table.constants';

@Component({
  selector: 'app-payments',
  templateUrl: './payments.component.html',
  styleUrls: ['./payments.component.scss']
})
export class PaymentsComponent extends BaseClass {

  TableConstants = TableConstants;

  isOpen: boolean = true;

  public paymentLabelConstants = PolicyBillingLabelsConstants.payments;

  paymentDetailsModalRef: BsModalRef | null;

  public paymentsHeaders: string[] = [
    this.paymentLabelConstants.postDate,
    this.paymentLabelConstants.amount,
    this.paymentLabelConstants.type,
    this.paymentLabelConstants.method,
    this.paymentLabelConstants.referenceNumber,
    this.paymentLabelConstants.comments,
    this.paymentLabelConstants.reversalDate,
    this.paymentLabelConstants.clearDate,
    ''
  ];

  constructor(public billingData: PolicyBillingData,
    private modalService: BsModalService,
    private billingService: BillingService,
    private reportsService: ReportsService,
    private datePipe: DatePipe,
    private submissionData: SubmissionData,
  ) {
    super();
  }

  viewPaymentDetails(paymentId: string, paymentType: string): void {

    const riskId = this.billingData.data.riskId;

    Utils.blockUI();
    this.billingService.getPaymentDetails(riskId, paymentId).pipe(takeUntil(this.stop$)).subscribe(res => {
      Utils.unblockUI();

      const paymentDetails: PaymentDetailsViewDTO = res;

      let modalTitle: string = PolicyBillingLabelsConstants.paymentDetails.paymentModalTitle;
      if (paymentDetails.paymentMethod === PolicyBillingConstants.paymentMethod.adjustment) {
        modalTitle = PolicyBillingLabelsConstants.paymentDetails.adjustmentModalTitle;
      } else if (paymentDetails.paymentMethod === PolicyBillingConstants.paymentMethod.writeOff) {
        modalTitle = PolicyBillingLabelsConstants.paymentDetails.writeOffModalTitle;
      }

      let isRefund: boolean = false;
      if (paymentType.toLowerCase() === PaymentType.Refund.toLowerCase()) {
        modalTitle = 'Refund Details';
        isRefund = true;
      }

      let isWriteOff: boolean = false;
      if (paymentType.toLowerCase() === PaymentType.WriteOff.toLowerCase()) {
        isWriteOff = true;
      }

      let isAllocation: boolean = false;
      if (paymentType.toLowerCase() === PaymentType.Allocation.toLowerCase()) {
        isAllocation = true;
      }

      const initialState = {
        title: modalTitle,
        paymentData: paymentDetails,
        isPostPayment: false,
        isRefund: isRefund,
        isWriteOff: isWriteOff,
        isAllocation: isAllocation
      };

      this.paymentDetailsModalRef = this.modalService.show(PaymentDetailsModalComponent, {
        initialState,
        backdrop: true,
        ignoreBackdropClick: true,
        class: 'modal-lg',
      });
    },
      err => {
        NotifUtils.showError(err.error);
      }
    );
  }

  collapse(): void {
    this.isOpen = !this.isOpen;
  }

  onAdjustment(): void {
    // this.paymentDetailsModalRef = this.modalService.show(AdjustmentModalComponent, {
    //   backdrop: true,
    //   ignoreBackdropClick: true,
    //   class: 'modal-md',
    // });
  }

  onWriteOff(): void {
    this.paymentDetailsModalRef = this.modalService.show(WriteOffModalComponent, {
      backdrop: true,
      ignoreBackdropClick: true,
      class: 'modal-md',
    });
  }

  onRefund(): void {
    const initialState = {
      isPostPayment: true,
      isRefund: true
    };

    this.paymentDetailsModalRef = this.modalService.show(PostPaymentModalComponent, {
      initialState,
      backdrop: true,
      ignoreBackdropClick: true,
      class: 'modal-md',
    });
  }

  generatePaymentReceipt(payment: PaymentViewDTO) {
    Utils.blockUIWithMessage("Generating payment receipt...");

    const riskId = this.billingData.data.riskId;
    this.reportsService.generatePaymentReceipt(riskId, payment.id).pipe(
      catchError((err) => {
        console.error(err?.message);
        NotifUtils.showError("Error generating payment receipt!");
        return of(err);
      }),
    ).subscribe((data) => {
      try {
        const paymentEffectiveDate =  this.datePipe.transform(payment.postDate, "MM-dd-yyyy");
        const riskDetail = this.submissionData.riskDetail;
        let filename = `${riskDetail.nameAndAddress.businessName}_Receipt_${paymentEffectiveDate}.pdf`;

        let url = window.URL.createObjectURL(data.body);
        let a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = url;
        a.download = filename;
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();

        Utils.unblockUI();
      } catch(e) {
        console.error(e);
      }
    });
  }
}
