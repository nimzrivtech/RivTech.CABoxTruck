import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { PolicyBillingData } from 'app/modules/policy/data/policy-billing.data';
import { ErrorMessageConstant } from 'app/shared/constants/error-message.constants';
import { GenericLabelConstants } from 'app/shared/constants/generic.labels.constants';
import { PolicyBillingLabelsConstants } from 'app/shared/constants/policy-billing.labels.constants';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
  selector: 'app-transfer-payment-modal',
  templateUrl: './transfer-payment-modal.component.html',
  styleUrls: ['./transfer-payment-modal.component.scss']
})
export class TransferPaymentModalComponent implements OnInit {
  public event: EventEmitter<any> = new EventEmitter();

  transferPaymentForm: FormGroup;
  TransferPaymentConstants = PolicyBillingLabelsConstants.transferPayment;
  ErrorMessageConstant = ErrorMessageConstant;
  GenericLabel = GenericLabelConstants;
  transferAmount: number;

  constructor(private fb: FormBuilder,
    public bsModalRef: BsModalRef,
    private billingData: PolicyBillingData) { }

  ngOnInit() {
    this.transferPaymentForm = this.fb.group({
      transferToPolicy: new FormControl(null, [Validators.required]),
      amount: new FormControl(null, [Validators.required]),
      comment: new FormControl('')
    });

    this.transferPaymentForm.get('amount').setValue(this.transferAmount);
  }

  hideModal(): void {
    this.bsModalRef.hide();
  }

  onTransfer(): void {
    this.triggerEvent({
      continue: true,
      policyNumber: this.transferPaymentForm.get('transferToPolicy').value,
      comment: this.transferPaymentForm.get('comment').value
    });
  }

  onCancel(): void {
    this.triggerEvent({ continue: false });
  }

  triggerEvent(res: any) {
    this.event.emit({ data: res, res: 200 });
  }

  isTransferToSelf(): boolean {
    let policyNumber: string = this.transferPaymentForm.get('transferToPolicy').value;

    if(this.transferPaymentForm.get('transferToPolicy').value != null)
    {
      policyNumber = this.transferPaymentForm.get('transferToPolicy').value.toLowerCase();
    }
    
    return this.billingData.data.policyNumber.toLowerCase() === policyNumber;
  }
}
