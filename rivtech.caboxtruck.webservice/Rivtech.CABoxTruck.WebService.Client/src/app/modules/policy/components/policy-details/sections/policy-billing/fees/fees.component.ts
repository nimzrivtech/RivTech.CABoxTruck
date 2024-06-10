import { Component, OnInit } from '@angular/core';
import { PolicyBillingLabelsConstants } from '../../../../../../../shared/constants/policy-billing.labels.constants';
import { BaseClass } from '../../../../../../../shared/base-class';
import { PolicyBillingData } from '../../../../../data/policy-billing.data';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FeeModalComponent } from './fee-modal/fee-modal.component';
import { ConfirmationModalComponent } from '../../../../../../../shared/components/confirmation-modal/confirmation-modal.component';
import { TransactionFeeRequestDTO } from '../../../../../../../shared/models/policy/billing/transaction-fee-request.dto';
import Utils from '../../../../../../../shared/utilities/utils';
import { BillingService } from '../../../../../../../core/services/billing/billing.service';
import NotifUtils from '../../../../../../..//shared/utilities/notif-utils';
import { VoidTransactionFeeRequestDTO } from '../../../../../../../shared/models/policy/billing/void-transaction-fee-request.dto';
import { takeUntil } from 'rxjs/operators';
import { DatePipe } from '@angular/common';
import { SubmissionData } from '../../../../../../submission/data/submission.data';

@Component({
  selector: 'app-fees',
  templateUrl: './fees.component.html',
  styleUrls: ['./fees.component.scss']
})
export class FeesComponent extends BaseClass implements OnInit {
  isOpen: boolean = true;

  public feesConstants = PolicyBillingLabelsConstants.fees;

  feeModalRef: BsModalRef | null;
  confirmationModalRef: BsModalRef | null;

  datePipe: DatePipe;

  constructor(public billingData: PolicyBillingData,
    private modalService: BsModalService,
    private billingService: BillingService,
    public submissionData: SubmissionData,) {
    super();
  }

  ngOnInit() {
    this.datePipe = new DatePipe('en-US');
    
    if (this.submissionData.riskDetail.riskId) {
      this.billingData.listTransactionFeesByRiskId(this.submissionData.riskDetail.riskId);
    }
  }

  collapse(): void {
    this.isOpen = !this.isOpen;
  }

  onAddFee(): void {
    const initialState = {
      title: this.feesConstants.title,
      isAdd: false,
    };

    this.feeModalRef = this.modalService.show(FeeModalComponent, {
      initialState,
      backdrop: true,
      ignoreBackdropClick: true,
      class: 'modal-md',
    });
    this.feeModalRef.content.event.subscribe((result) => {
      if (result.data.continue) {
        const request: TransactionFeeRequestDTO = {
          riskId: this.billingData.data.riskId,
          amount: result.data.amount,
          amountSubTypeId: result.data.feeType,
          addDate: result.data.addDate
        };

        Utils.blockUI();
        this.billingService.addTransactionFee(request).pipe(takeUntil(this.stop$)).subscribe(_ => {
          Utils.unblockUI();

          this.billingData.populateBillingSections(this.billingData.data.riskId);
          NotifUtils.showSuccess(this.feesConstants.addFeeSuccess);

          this.feeModalRef.hide();
        },
          err => {
            Utils.unblockUI();
            NotifUtils.showMultiLineError(err.error?.message);
          });
      } else {
        this.feeModalRef.hide();
      }
    });
  }

  onVoidFee(feeId: string): void {
    // this.confirmationModalRef = this.modalService.show(ConfirmationModalComponent, {
    //   initialState: { message: this.feesConstants.voidFeeConfirmation },
    //   class: 'modal-sm modal-dialog-centered',
    //   backdrop: 'static',
    //   keyboard: false
    // });
    // this.confirmationModalRef.content.event.subscribe((result) => {
    //   if (result.data) {
    //     const voidRequest: VoidTransactionFeeRequestDTO = {
    //       id: feeId,
    //       riskId: this.billingData.data.riskId
    //     };

    //     Utils.blockUI();
    //     this.billingService.voidTransactionFee(voidRequest).pipe(takeUntil(this.stop$)).subscribe(res => {
    //       Utils.unblockUI();

    //       this.billingData.populateBillingSections(this.billingData.data.riskId);
    //       NotifUtils.showSuccess(this.feesConstants.voidFeeSuccess);
    //     },
    //       err => {
    //         Utils.unblockUI();
    //         NotifUtils.showMultiLineError(err.error?.message);
    //       });
    //   }
    //   this.confirmationModalRef.hide();
    // });
  }
}
