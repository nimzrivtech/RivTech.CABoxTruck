import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { PaymentSummaryDTO } from 'app/shared/models/policy/billing/payment-summary.dto';
import { PolicyBillingData } from 'app/modules/policy/data/policy-billing.data';
import { ErrorMessageConstant } from 'app/shared/constants/error-message.constants';
import { GenericLabelConstants } from 'app/shared/constants/generic.labels.constants';
import { PolicyBillingLabelsConstants } from 'app/shared/constants/policy-billing.labels.constants';
import { BsModalRef } from 'ngx-bootstrap';
import { DatePipe } from '@angular/common';
import { PaymentRequestDTO } from '../../../../../../../../shared/models/policy/billing/payment-request.dto';
import { BillingService } from 'app/core/services/billing/billing.service';
import Utils from 'app/shared/utilities/utils';
import NotifUtils from 'app/shared/utilities/notif-utils';
import { switchMap, takeUntil } from 'rxjs/operators';
import { BaseClass } from 'app/shared/base-class';
import { BillingPaymentInstrument } from 'app/shared/enum/billing-payment-instrument';
import { CurrencyMaskInputMode } from 'ngx-currency';
import { BillingValidators } from 'app/modules/submission/validators/billing.validator';
import { BillingErrorConstants } from 'app/modules/submission/validators/validator-error-constants/billing-error.constants';
import { SubmissionData } from '@app/modules/submission/data/submission.data';

@Component({
  selector: 'app-write-off-modal',
  templateUrl: './write-off-modal.component.html',
  styleUrls: ['./write-off-modal.component.scss']
})
export class WriteOffModalComponent extends BaseClass implements OnInit {

  writeOffForm: FormGroup;
  WriteOffConstants = PolicyBillingLabelsConstants.writeOff;
  ErrorMessageConstant = ErrorMessageConstant;
  GenericLabel = GenericLabelConstants;
  BillingErrorConstants = BillingErrorConstants;

  currencySettings: any = {
    align: 'left',
    allowNegative: false,
    precision: 2,
    nullable: false
  };

  datePipe: DatePipe;

  currencyInputMode = CurrencyMaskInputMode.NATURAL;

  constructor(private fb: FormBuilder,
    public bsModalRef: BsModalRef,
    private billingData: PolicyBillingData,
    private billingService: BillingService,
    private submissionData: SubmissionData) {
    super();
  }

  ngOnInit() {
    this.datePipe = new DatePipe('en-US');

    this.writeOffForm = this.fb.group({
      method: new FormControl({ value: null, disabled: true }, [Validators.required]),
      amount: new FormControl(null, [Validators.required, BillingValidators.amountCannotBeZeroValidator()]),
      premium: new FormControl(null, [Validators.required]),
      tax: new FormControl(null, [Validators.required]),
      fee: new FormControl(null, [Validators.required]),
      comment: new FormControl('')
    });

    this.writeOffForm.get('method').setValue('writeOff');

    this.writeOffForm.get('amount').valueChanges.subscribe(totalAmount => {
      const isSurchargePopulated: boolean = (this.writeOffForm.get('tax').value + this.writeOffForm.get('fee').value) > 0;

      if (totalAmount && !isSurchargePopulated) {
        this.writeOffForm.get('premium').setValue(totalAmount);

        this.writeOffForm.get('tax').setValue(0);
        this.writeOffForm.get('fee').setValue(0);
      }
    });
  }

  hideModal(): void {
    this.bsModalRef.hide();
  }

  get isAmountNotMatched(): boolean {
    const totalAmount = (this.writeOffForm.get('premium').value +
      this.writeOffForm.get('tax').value +
      this.writeOffForm.get('fee').value);

    return (totalAmount.toFixed(2) !== this.writeOffForm.get('amount').value?.toFixed(2));
  }

  onSave(): void {
    const writeOffRequest: PaymentRequestDTO = this.getWriteOffRequest();

    Utils.blockUI();
    this.billingService.postPaymentRequest(writeOffRequest)
      .pipe(
        takeUntil(this.stop$)
      ).subscribe(notes => {
        this.bsModalRef.hide();

        this.billingData.showSummary(writeOffRequest.riskId);
        this.billingData.listInstallmentInvoice(writeOffRequest.riskId);
        this.billingData.listPaymentsByRiskId(writeOffRequest.riskId);

        //this.policyNotesData.populateNotes(notes);

        Utils.unblockUI();
        NotifUtils.showSuccess(PolicyBillingLabelsConstants.writeOffSuccess);
      },
        err => {
          Utils.unblockUI();
          NotifUtils.showMultiLineError(err.error?.message);
        }
      );
  }

  getWriteOffRequest(): PaymentRequestDTO {
    const paymentSummary: PaymentSummaryDTO = {
      amount: this.writeOffForm.get('amount').value,
      premiumAmount: this.writeOffForm.get('premium').value,
      taxAmount: this.writeOffForm.get('tax').value,
      feeAmount: this.writeOffForm.get('fee').value,

      comment: this.writeOffForm.get('comment').value,
      effectiveDate: this.datePipe.transform(new Date(this.submissionData.currentServerDateTime), 'yyyy-MM-dd'),
      instrumentId: BillingPaymentInstrument.WriteOff,
    };

    const paymentRequest: PaymentRequestDTO = {
      riskId: this.billingData.data.riskId,
      riskBindId: this.billingData.data.binding.id,
      paymentSummary: paymentSummary
    };

    return paymentRequest;
  }
}
