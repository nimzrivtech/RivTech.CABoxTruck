import { Component, OnInit, EventEmitter } from '@angular/core';
import { ErrorMessageConstant } from '../../../../../../../../shared/constants/error-message.constants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { PolicyBillingLabelsConstants } from '../../../../../../../../shared/constants/policy-billing.labels.constants';
import { PolicyBillingData } from '../../../../../../../../../app/modules/policy/data/policy-billing.data';
import { PaymentMethod } from '../../../../../../../../shared/enum/payment-method.enum';
import { PaymentConfirmationModalComponent } from '../payment-confirmation-modal/payment-confirmation-modal.component';
import { TransferPaymentModalComponent } from '../transfer-payment-modal/transfer-payment-modal.component';
import { AuthService } from '../../../../../../../../../app/core/services/auth.service';
import { switchMap, takeUntil, tap } from 'rxjs/operators';
import { BaseClass } from '../../../../../../../../shared/base-class';
// import { EscheatConfirmationModalComponent } from '../escheat-confirmation-modal/escheat-confirmation-modal.component';
import { PaymentDetailsViewDTO } from '../../../../../../../../shared/models/policy/billing/payment-details-view.dto';
import { ReversePaymentDTO } from '../../../../../../../../shared/models/policy/billing/reverse-payment.dto';
import { ReversalType } from '../../../../../../../../shared/enum/reversal-type.enum';
import { BillingService } from '../../../../../../../../../app/core/services/billing/billing.service';
import Utils from '../../../../../../../../shared/utilities/utils';
import NotifUtils from '../../../../../../../../shared/utilities/notif-utils';
import { TransferPaymentDTO } from '../../../../../../../../shared/models/policy/billing/transfer-payment.dto';
import { PolicyService } from '../../../../../../../../../app/core/services/policy/policy.service';
import { of } from 'rxjs';
import { CurrencyPipe } from '@angular/common';
import { CopyPaymentRelatedDocDTO } from '../../../../../../../../shared/models/policy/billing/copy-payment-related-doc.dto';

@Component({
  selector: 'app-payment-details-modal',
  templateUrl: './payment-details-modal.component.html',
  styleUrls: ['./payment-details-modal.component.scss']
})
export class PaymentDetailsModalComponent extends BaseClass implements OnInit {
  public event: EventEmitter<any> = new EventEmitter<any>();
  public ErrorMessageConstant = ErrorMessageConstant;
  public PaymentDetailConstants = PolicyBillingLabelsConstants.paymentDetails;
  PaymentMethod = PaymentMethod;
  isInternal: boolean;

  modalRef: BsModalRef | null;
  title: string;

  paymentData: PaymentDetailsViewDTO;

  isPaymentReversal: boolean = false;
  isRefund: boolean = false;
  isWriteOff: boolean = false;
  isAllocation: boolean = false;
  isEscheat: boolean = false;

  paymentConfirmationModalRef: BsModalRef | null;

  methodLabel: string;

  constructor(
    public bsModalRef: BsModalRef,
    public billingData: PolicyBillingData,
    private modalService: BsModalService,
    private authService: AuthService,
    private billingService: BillingService,
    private policyService: PolicyService,
    private currencyPipe: CurrencyPipe
  ) {
    super();
  }

  ngOnInit() {
    this.authService.userType.pipe(takeUntil(this.stop$)).subscribe(userType => {
      // this.isInternal = this.isInternalUser(userType);
      this.isInternal = true;
    });

    this.initForm();
  }

  initForm(): void {
    this.populateFields();
  }

  populateFields(): void {
    this.billingData.paymentDetailsForm.reset();
    this.billingData.paymentDetailsForm.patchValue(this.paymentData);
    this.billingData.paymentDetailsForm.get('paymentMethod').enable();
    this.billingData.paymentDetailsForm.get('paymentMethod').setValue(this.paymentData.paymentMethod);

    if (this.billingData.paymentDetailsForm.value.reversalType) {
      this.isPaymentReversal = true;
    }

    if (this.billingData.paymentDetailsForm.value.escheatDate) {
      this.isEscheat = true;
    }

    if (this.isRefund) {
      this.methodLabel = this.PaymentDetailConstants.refundMethod;
    } else {
      this.methodLabel = this.PaymentDetailConstants.method;
    }
  }

  hideModal(): void {
    this.bsModalRef.hide();
  }

  onVoid(): void {
    this.showActionConfirmation(this.PaymentDetailConstants.voidConfirmation, ReversalType.Void);
  }

  onNSF(): void {
    this.showActionConfirmation(this.PaymentDetailConstants.nsfConfirmation, ReversalType.NSF);
  }

  onStopPay(): void {
    this.showActionConfirmation(this.PaymentDetailConstants.stopPayConfirmation, ReversalType.StopPay);
  }

  onEscheat(): void {
    // this.modalRef = this.paymentConfirmationModalRef = this.modalService.show(EscheatConfirmationModalComponent, {
    //   initialState: { message: this.PaymentDetailConstants.escheatConfirmation },
    //   class: 'modal-dialog-centered',
    //   backdrop: true,
    //   ignoreBackdropClick: true,
    // });

    // this.modalRef.content.event.subscribe((result) => {
    //   if (result.data) {
    //   }

    //   this.modalRef.hide();
    // });
  }

  onTransferPayment(): void {
    this.modalRef = this.paymentConfirmationModalRef = this.modalService.show(TransferPaymentModalComponent, {
      initialState: { transferAmount: this.billingData.paymentDetailsForm.value.amount },
      class: 'modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true,
    });

    this.modalRef.content.event.subscribe((result) => {
      if (result.data.continue) {

        Utils.blockUI();

        let destinationRiskId: string;
        const request: TransferPaymentDTO = {
          fromRiskId: this.billingData.data.riskId,
          fromPolicyNumber: this.billingData.data.policyNumber,
          toPolicyNumber: result.data.policyNumber,
          paymentId: this.paymentData.paymentId,
          comments: result.data.comment
        };
        this.billingService.transferPayment(request).subscribe(res => {
          if (!res) {
            Utils.unblockUI();

            NotifUtils.showError(this.PaymentDetailConstants.transferPayment.invalidPolicyNumberError);
          } else {
            this.billingData.populateBillingSections(this.billingData.data.riskId);
            NotifUtils.showSuccess(`${this.PaymentDetailConstants.transferPayment.success} ${this.currencyPipe.transform(Math.abs(res.amount))}`);
            this.modalRef.hide();
            this.bsModalRef.hide();
          }
        },
        err => {
          Utils.unblockUI();
          NotifUtils.showMultiLineError(err.error?.message);
        });
      } else {
        this.modalRef.hide();
      }
    });
  }

  showActionConfirmation(msg: string, _actionType: string): void {

    let reverasalTypeLabel: string;
    switch (_actionType) {
      case ReversalType.Void:
        reverasalTypeLabel = this.PaymentDetailConstants.voidType;
        break;
      case ReversalType.NSF:
        reverasalTypeLabel = this.PaymentDetailConstants.nsfType;
        break;
      case ReversalType.StopPay:
        reverasalTypeLabel = this.PaymentDetailConstants.stopPayType;
        break;
    }

    this.modalRef = this.paymentConfirmationModalRef = this.modalService.show(PaymentConfirmationModalComponent, {
      initialState: { message: msg, actionTypeLabel: this.PaymentDetailConstants.save },
      class: 'modal-sm modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true,
    });

    this.modalRef.content.event.subscribe((result) => {
      if (result.data.continue) {
        const request: ReversePaymentDTO = {
          riskId: this.billingData.data.riskId,
          paymentId: this.paymentData.paymentId,
          reversalTypeId: _actionType,
          comment: result.data.comment,
          appId: 'CABT'
        };

        Utils.blockUI();
        this.billingService.reversePayment(request).pipe(takeUntil(this.stop$)).subscribe(res => {
          Utils.unblockUI();
          
          this.billingData.populateBillingSections(this.billingData.data.riskId);

          NotifUtils.showSuccess(`${reverasalTypeLabel} ${this.PaymentDetailConstants.successMessage}`);

          this.modalRef.hide();
          this.bsModalRef.hide();
        },
          err => {
            Utils.unblockUI();
            NotifUtils.showMultiLineError(err.error?.message);
          });
      } else {
        this.modalRef.hide();
      }
    });
  }

  public isInternalUser(userType: string): boolean {
    return userType.toLocaleLowerCase() === 'internal';
  }
}
