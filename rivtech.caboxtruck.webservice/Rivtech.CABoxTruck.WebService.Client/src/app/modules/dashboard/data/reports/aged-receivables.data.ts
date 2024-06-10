import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DocumentApiService } from '@app/modules/documents/document-api.service';
import { BrokerGenInfoData } from '@app/modules/submission/data/broker/general-information.data';
import { BaseClass } from '@app/shared/base-class';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { ReportsService } from '../../services/reports.service';

@Injectable()
export class AgedReceivablesData extends BaseClass {

  agedReceivablesReportForm: FormGroup;
  valuationDateOptions: IAngularMyDpOptions;

  constructor(private formBuilder: FormBuilder, public brokerInfoData: BrokerGenInfoData, private reportService: ReportsService, private documentApiService: DocumentApiService) {
    super();
  }

  initializeFields(): void {
    this.initAgedReceivablesReportForm();
  }

  initAgedReceivablesReportForm(): void {
    this.agedReceivablesReportForm = this.formBuilder.group({
      toDate: new FormControl(null, [Validators.required])
    });
  }
}
