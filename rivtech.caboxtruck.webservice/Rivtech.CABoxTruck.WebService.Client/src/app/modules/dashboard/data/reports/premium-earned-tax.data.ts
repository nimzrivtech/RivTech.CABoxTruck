import { Injectable } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
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
export class PremiumEarnedTaxReportData extends BaseClass {

  premiumEarnedTaxReportForm: FormGroup;
    valuationDateOptions: IAngularMyDpOptions;

    constructor(private formBuilder: FormBuilder, public brokerInfoData: BrokerGenInfoData, private reportService: ReportsService, private documentApiService: DocumentApiService) {
        super();
    }

    initializeFields(): void {
        this.initPremiumEarnedTaxReportForm();
    }

    initPremiumEarnedTaxReportForm(): void {
        this.premiumEarnedTaxReportForm = this.formBuilder.group({
          fromDate: new FormControl(null, [Validators.required]),
          toDate: new FormControl(null, [Validators.required])
        });
      }
}
