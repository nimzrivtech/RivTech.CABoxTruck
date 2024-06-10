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
export class PaymentPostingReportData extends BaseClass {

    paymentPostingForm: FormGroup;
    valuationDateOptions: IAngularMyDpOptions;

    constructor(private formBuilder: FormBuilder, public brokerInfoData: BrokerGenInfoData, private documentApiService: DocumentApiService) {
        super();
    }

    initializeFields(): void {
        this.initForm();
    }

    get reportHeaders() {
      return {
        policyNumber: 'Policy Number',
        companyName: 'Insured',
        broker:'Broker',
        creditedOffice: 'Credited Office',
        checkNumber: 'Check Number',
        postDate: 'Post Date',
        total: 'Total Amount',
        premiumAmountPD : 'PD Premium Amount',
        premiumAmountAL : 'AL Premium Amount',
        premium: 'Premium Amount',
        tax: 'Tax Amount',
        nonPremium: 'Non-Premium Amount',
        overPayment: 'Over Payment Amount',
        brokerCommisionAL: 'AL Commission %',
        brokerCommisionPD: 'PD Commission %',
        commisionALAmount: 'AL Commission $',
        commisionPDAmount: 'PD Commission $',
        commissionTotal: 'Broker Commission $',
        percentageARS: 'ARS %',
        commissionARS: 'ARS Commission $',
        commisionRivAL: 'AL Riv Commission $',
        commisionRivPD: 'PD Riv Commission $',       
        commisionToRivAmnt: 'Commission to Riv $',
        premiumDueToCarier: 'Premium Due to Carrier',
        paymentTypesId : 'Payment Plan'
      };
    }

    initForm(): void {
        this.paymentPostingForm = this.formBuilder.group({
          fromDate: new FormControl(null, [Validators.required]),
          toDate: new FormControl(null, [Validators.required])
        });
      }

    savePaymentPostingPdf(file: any, fileName: string) {
        Utils.blockUI();
        const formData = new FormData();
        formData.append('file', file, fileName);
        this.documentApiService.savePaymentPosting(formData).subscribe(data => {
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
