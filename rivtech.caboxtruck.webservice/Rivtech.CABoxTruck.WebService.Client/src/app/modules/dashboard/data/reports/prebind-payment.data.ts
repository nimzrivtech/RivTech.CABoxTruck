import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DocumentApiService } from '@app/modules/documents/document-api.service';
import { BrokerGenInfoData } from '@app/modules/submission/data/broker/general-information.data';
import { BaseClass } from '@app/shared/base-class';
import { IAngularMyDpOptions } from 'angular-mydatepicker';

@Injectable()
export class PreBindPaymentReportData extends BaseClass {

    prebindPaymentForm: FormGroup;
    valuationDateOptions: IAngularMyDpOptions;

    constructor(private formBuilder: FormBuilder, public brokerInfoData: BrokerGenInfoData, private documentApiService: DocumentApiService) {
        super();
    }

    initializeFields(): void {
        this.initForm();
    }

    get reportHeaders() {
      return {
        insuredName: 'Insured Name',
        accessCode: 'Access Code',
        payeeName: 'Payee Name',
        payeeEmail: 'Payee Email Address',
        paymentDateTime: 'Payment Date/Time',
        paymentMethod: 'Payment Method',
        paymentAmount: 'Payment Amount',
        confirmationCode: 'Confirmation Code'
      };
    }

    initForm(): void {
        this.prebindPaymentForm = this.formBuilder.group({
          fromDate: new FormControl(null, [Validators.required]),
          toDate: new FormControl(null, [Validators.required])
        });
      }
}
