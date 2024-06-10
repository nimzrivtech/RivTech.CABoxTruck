import { HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DocumentApiService } from '@app/modules/documents/document-api.service';
import { BrokerGenInfoData } from '@app/modules/submission/data/broker/general-information.data';
import { BaseClass } from '@app/shared/base-class';
import NotifUtils from '@app/shared/utilities/notif-utils';
import Utils from '@app/shared/utilities/utils';
import { IAngularMyDpOptions } from 'angular-mydatepicker';

@Injectable()
export class BrokerCommissionReportData extends BaseClass {

    brokerCommissionForm: FormGroup;
    valuationDateOptions: IAngularMyDpOptions;

    constructor(private formBuilder: FormBuilder, public brokerInfoData: BrokerGenInfoData, private documentApiService: DocumentApiService) {
        super();
    }

    initializeFields(): void {
        this.initForm();
    }

    get reportHeaders() {
      return {
          creditedOffice: 'Credited Office',
          broker: 'Broker',
          policyNumber: 'Policy Number',
          insured: 'Insured',
          checkNumber: 'Check Number',
          postDate: 'Payment Post Date',
          paymentAmount: 'Premium Posted',
          brokerCommisionAL: 'AL Commission %',
          brokerCommisionPD: 'PD Commission %',
          commisionALAmount: 'AL Commission $',
          commisionPDAmount: 'PD Commission $',
          commissionTotal: 'Total $',
          percentageARS: 'ARS %',
          commissionARS: 'ARS Commission $'
      };
    }

    initForm(): void {
        this.brokerCommissionForm = this.formBuilder.group({
          fromDate: new FormControl(null, [Validators.required]),
          toDate: new FormControl(null, [Validators.required])
        });
      }

    saveBrokerCommissionPdf(file: any, fileName: string) {
        Utils.blockUI();
        const formData = new FormData();
        formData.append('file', file, fileName);
        this.documentApiService.saveBrokerCommission(formData).subscribe(data => {
            if (data.body?.message) {
                NotifUtils.showError(data.body.message);
                return;
            }

            if (data.type === HttpEventType.Response) {
                this.documentApiService.getDocumentSASURL(data.body).pipe().take(1).subscribe(data => {
                    const win = window.open(data, '_blank');
                    win.focus();
                  });
            }
            Utils.unblockUI();
            return;
        });
    }
}
