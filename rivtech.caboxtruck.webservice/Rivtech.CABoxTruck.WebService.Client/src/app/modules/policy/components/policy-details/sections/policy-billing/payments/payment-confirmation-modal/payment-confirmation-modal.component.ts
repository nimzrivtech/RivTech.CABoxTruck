import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ErrorMessageConstant } from 'app/shared/constants/error-message.constants';

@Component({
  selector: 'app-payment-confirmation-modal',
  templateUrl: './payment-confirmation-modal.component.html',
  styleUrls: ['./payment-confirmation-modal.component.scss']
})
export class PaymentConfirmationModalComponent implements OnInit {
  public event: EventEmitter<any> = new EventEmitter();
  message: string;
  actionTypeLabel: string;

  ErrorMessageConstant = ErrorMessageConstant;

  paymentActionForm: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.paymentActionForm = this.fb.group({
      comment: new FormControl('', [Validators.required])
    });
  }

  confirm(): void {
    this.triggerEvent({ continue: true, comment: this.paymentActionForm.get('comment').value });
  }

  decline(): void {
    this.triggerEvent({ continue: false });
  }

  triggerEvent(res: any) {
    this.event.emit({ data: res, res: 200 });
  }

}
