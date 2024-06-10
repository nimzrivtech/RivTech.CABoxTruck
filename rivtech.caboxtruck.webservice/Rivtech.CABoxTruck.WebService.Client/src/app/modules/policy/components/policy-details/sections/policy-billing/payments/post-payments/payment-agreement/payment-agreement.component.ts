import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BillingLabelConstants } from '../../../../../../../../../../app/shared/constants/billing.labels.constants';
import { PaymentMethod } from '../../../../../../../../../../app/shared/enum/payment-method.enum';

@Component({
  selector: 'app-payment-agreement',
  templateUrl: './payment-agreement.component.html',
  styleUrls: ['./payment-agreement.component.scss']
})
export class PaymentAgreementComponent implements OnInit {

  @Input() paymentForm: FormGroup;

  public BillingLabelConstants = BillingLabelConstants;

  constructor() { }

  ngOnInit() {
  }

  get isPaymentMethodRecurring(): boolean {
    return this.paymentForm.get('paymentMethod').value === PaymentMethod.RecurringCreditCard ||
      this.paymentForm.get('paymentMethod').value === PaymentMethod.RecurringECheck;
  }

  get isPaymentMethodOneTimeAuth(): boolean {
    return this.paymentForm.get('paymentMethod').value === PaymentMethod.CreditCard ||
      this.paymentForm.get('paymentMethod').value === PaymentMethod.EFT ||
      this.paymentForm.get('paymentMethod').value === PaymentMethod.ECheck;
  }
}
