import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AccountAchTypeConstants} from 'app/shared/constants/payment-portal-specifics/accountAchType.constants';
import {AccountAchUsageTypeConstants} from 'app/shared/constants/payment-portal-specifics/accountAchUsageType.constants';

@Component({
  selector: 'app-payment-eft',
  templateUrl: './payment-eft.component.html',
  styleUrls: ['./payment-eft.component.css']
})
export class PaymentEftComponent implements OnInit {
  @Output() submitEftForm: EventEmitter<FormGroup> = new EventEmitter<FormGroup>();
  @Output() isValid: EventEmitter<boolean> = new EventEmitter<boolean>();
  eftForm: FormGroup;
  accountTypes = AccountAchTypeConstants;
  accountUsageTypes = AccountAchUsageTypeConstants;
  Object = Object;

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.eftForm = this.fb.group({
      payer: this.fb.control('', Validators.required),
      email: this.fb.control('', Validators.required),
      routingNumber: this.fb.control('', Validators.required),
      accountNumber: this.fb.control('', Validators.required),
      accountType: this.fb.control('', Validators.required),
      accountUsageType: this.fb.control('', Validators.required),
      comments: this.fb.control('', Validators.required)
    });
    this.checkFormChanges();
  }

  checkFormChanges() {
    this.submitEftForm.emit(this.eftForm.value);
    this.isValid.emit(this.eftForm.valid);

    this.eftForm.valueChanges.subscribe(form => {
      this.submitEftForm.emit(form);
      this.isValid.emit(this.eftForm.valid);
    });
  }
}
