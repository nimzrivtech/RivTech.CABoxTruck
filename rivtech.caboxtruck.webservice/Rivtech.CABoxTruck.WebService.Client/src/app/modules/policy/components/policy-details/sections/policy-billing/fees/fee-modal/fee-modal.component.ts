import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { PolicyBillingData } from '../../../../../../../../../app/modules/policy/data/policy-billing.data';
import { PolicyBillingLabelsConstants } from '../../../../../../../../../app/shared/constants/policy-billing.labels.constants';
import { BsModalRef } from 'ngx-bootstrap';
import { CurrencyMaskInputMode } from 'ngx-currency';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { DatePipe } from '@angular/common';
import { ErrorMessageConstant } from '../../../../../../../../../app/shared/constants/error-message.constants';

@Component({
  selector: 'app-fee-modal',
  templateUrl: './fee-modal.component.html',
  styleUrls: ['./fee-modal.component.scss']
})
export class FeeModalComponent implements OnInit {

  paymentDetailsForm: FormGroup;
  title: string;

  public feeForm: FormGroup;
  public ErrorMessageConstant = ErrorMessageConstant;
  public FeeLabelConstants = PolicyBillingLabelsConstants.fee;

  public event: EventEmitter<any> = new EventEmitter<any>();

  currencyInputMode = CurrencyMaskInputMode.NATURAL;
  datePickerDateOption: IAngularMyDpOptions;

  datePipe: DatePipe;

  constructor(
    public billingData: PolicyBillingData,
    public bsModalRef: BsModalRef
  ) { }

  ngOnInit(): void {
    this.datePipe = new DatePipe('en-US');

    this.datePickerDateOption = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };

    this.feeForm = new FormGroup({
      addDate: new FormControl(null, Validators.required),
      feeType: new FormControl('', Validators.required),
      amount: new FormControl(null, Validators.required),
    });

    this.feeForm.get('addDate').setValue({ isRange: false, singleDate: { jsDate: new Date() } });
  }

  onSubmit(): void {
    this.triggerEvent({
      continue: true,
      addDate: new Date(this.feeForm.get('addDate').value?.singleDate?.jsDate + 'UTC'),
      amount: this.feeForm.get('amount').value,
      feeType: this.feeForm.get('feeType').value,
    });
  }

  hideModal(): void {
    this.bsModalRef.hide();
  }

  triggerEvent(res: any): void {
    this.event.emit({ data: res, res: 200 });
  }
}
