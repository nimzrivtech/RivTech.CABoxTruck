import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AgedReceivablesData } from '@app/modules/dashboard/data/reports/aged-receivables.data';
import { ReportsService } from '@app/modules/dashboard/services/reports.service';
import { AgedReceivablesReportRequestDTO } from '@app/shared/models/reports/aged-receivables-report-request.dto';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { BaseClass } from 'app/shared/base-class';
import { ErrorMessageConstant } from 'app/shared/constants/error-message.constants';
import NotifUtils from 'app/shared/utilities/notif-utils';
import Utils from 'app/shared/utilities/utils';
import { BsModalRef } from 'ngx-bootstrap';
import { takeUntil } from 'rxjs/operators';


@Component({
  selector: 'app-aged-receivables',
  templateUrl: './aged-receivables.component.html',
  styleUrls: ['./aged-receivables.component.scss']
})
export class AgedReceivablesComponent extends BaseClass implements OnInit {
  modalRef: BsModalRef | null;
  datePipe: DatePipe;
  datePickerDateOption: IAngularMyDpOptions;
  fromDate: string;
  toDate: string;
  public errorMessageConstant = ErrorMessageConstant;

  constructor(
    public bsModalRef: BsModalRef,
    public agedReceivablesData: AgedReceivablesData,
    private reportService: ReportsService,
  ) {
    super();
  }
  
  ngOnInit() {
    this.agedReceivablesData.initializeFields();
    this.datePipe = new DatePipe('en-US');
    
    this.datePickerDateOption = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };
  }

  exportAgedReceivablesReport(): void {
    Utils.blockUI();
    const agedReceivablesReportRequest: AgedReceivablesReportRequestDTO = {
      toDate: this.datePipe.transform(this.agedReceivablesData.agedReceivablesReportForm.get('toDate').value.singleDate.jsDate, 'MM/dd/yyyy')
    };

    this.reportService.generateAgedReceivablesReport(agedReceivablesReportRequest).pipe((takeUntil(this.stop$))).subscribe((resp) => {
      const toDate = new Date(agedReceivablesReportRequest.toDate);
      const formattedDate = `${String(toDate.getMonth() + 1).padStart(2, '0')}${String(toDate.getDate()).padStart(2, '0')}${toDate.getFullYear()}`;
      const filename = `Aging Report RC Boxtruck ${formattedDate}.xlsx`;

      let downloadLink = document.createElement('a');
      downloadLink.href = window.URL.createObjectURL(resp.body);
      downloadLink.setAttribute('download', filename);
      document.body.appendChild(downloadLink);
      downloadLink.click();
      downloadLink.remove();
      Utils.unblockUI();
    }, err => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  hideModal(): void {
    this.bsModalRef.hide();
  }
}
