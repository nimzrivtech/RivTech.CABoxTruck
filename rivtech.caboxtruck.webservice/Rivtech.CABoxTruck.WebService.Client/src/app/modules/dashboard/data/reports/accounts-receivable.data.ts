import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BrokerGenInfoData } from '@app/modules/submission/data/broker/general-information.data';
import { BaseClass } from '@app/shared/base-class';
import { AccountsReceivableDTO, AccountsReceivableInputDTO } from '@app/shared/models/dashboard/arDto';
import { SelectItem } from '@app/shared/models/dynamic/select-item';
import NotifUtils from '@app/shared/utilities/notif-utils';
import Utils from '@app/shared/utilities/utils';
import { AccountReceivableReportDTO } from '@app/shared/models/reports/accounts-receivable-report.model';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { takeUntil } from 'rxjs/operators';
import { ReportsService } from '../../services/reports.service';
import { DocumentApiService } from '@app/modules/documents/document-api.service';
import { HttpEventType } from '@angular/common/http';

@Injectable()
export class AccountReceivableReportData extends BaseClass {

    accountsReceivableReportForm: FormGroup;
    valuationDateOptions: IAngularMyDpOptions;

    constructor(private formBuilder: FormBuilder, public brokerInfoData: BrokerGenInfoData, private reportService: ReportsService, private documentApiService: DocumentApiService) {
        super();
    }

    initializeFields(): FormGroup {
        this.valuationDateOptions = {
            dateRange: false,
            dateFormat: 'mm/dd/yyyy',
            disableUntil: { year: 0, month: 0, day: 0 },
            disableSince: { year: 0, month: 0, day: 0 },
        };

        this.accountsReceivableReportForm = this.formBuilder.group({
            receivableType: ['All'],
            broker: ['All'],
            cancellationStatus: [null],
            valuationDate: [null],
            pastDueOnly: [false],
            noPaymentsMade: [false],
            sortBy: ['Insured Name'] // default selection
        });

        return this.accountsReceivableReportForm;
    }

    generateAccountsReceivable() {
        Utils.blockUI();
        const arInput: AccountsReceivableInputDTO = {
            sortNameBy: this.accountsReceivableReportForm.controls['sortBy'].value,
            receivableType: this.accountsReceivableReportForm.controls['receivableType'].value,
            broker: this.accountsReceivableReportForm.controls['broker'].value,
            pastDueOnly: this.accountsReceivableReportForm.controls['pastDueOnly'].value,
            cancellationStatus: this.accountsReceivableReportForm.controls['cancellationStatus'].value,
            noPaymentsMade: this.accountsReceivableReportForm.controls['noPaymentsMade'].value,
            valuationDate: this.accountsReceivableReportForm.controls['valuationDate'].value,
          };
        this.reportService.searchAccountsReceivable(arInput).pipe(takeUntil(this.stop$)).subscribe((data) => {
            console.log(data);
            Utils.unblockUI();
        }, err => {
            Utils.unblockUI();
            NotifUtils.showError(JSON.stringify(err));
        });
    }

    get accountReceivableTypes(): SelectItem[] {
        return [
            { value: 'Amount Due', label: 'Amount Due' },
            { value: 'Refund', label: 'Refund' },
        ];
    }

    get cancellationStatus(): SelectItem[] {
        return [
            { value: 'Canceled', label: 'Canceled' },
            { value: 'No Cancellation', label: 'No Cancellation' },
            { value: 'Pending Cancellation', label: 'Pending Cancellation' },
        ];
    }

    get reportHeaders() {
        return {
            broker: 'Broker',
            policyNumber: 'Policy Number',
            insured: 'Insured',
            insuredPhone: 'Insured Phone',
            inception: 'Inception Date',
            pendingCancellation: 'Pending Cancellation',
            cancellationDate: 'Cancellation Date',
            pastDueDate: 'Past Due Date',
            pastDueAmount: 'Past Due Amount',
            currentDueDate: 'Due Date',
            currentAmountDue: 'Total Amount Due',
            noPayments: 'No Payments',
            financed: 'Financed'
        };
    }

    saveAccountsReceivablePdf(file: any) {
        Utils.blockUI();
        const formData = new FormData();
        formData.append('file', file, 'AccountsReceivable.pdf');
        this.documentApiService.saveAccountsReceivable(formData).subscribe(data => {
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
