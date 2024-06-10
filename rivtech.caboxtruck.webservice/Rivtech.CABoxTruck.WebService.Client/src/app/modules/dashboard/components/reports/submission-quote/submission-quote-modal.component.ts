import { Component, OnInit, Input } from '@angular/core';
import { BaseClass } from 'app/shared/base-class';
import { PolicyBillingLabelsConstants } from 'app/shared/constants/policy-billing.labels.constants';
import { DatePipe } from '@angular/common';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { BsModalRef } from 'ngx-bootstrap';
import Utils from 'app/shared/utilities/utils';
import NotifUtils from 'app/shared/utilities/notif-utils';
import { ErrorMessageConstant } from 'app/shared/constants/error-message.constants';
import { SubmissionQuoteReportRequestDTO } from 'app/shared/models/reports/submission-quote-report-request.dto';
import { takeUntil } from 'rxjs/operators';
import { BillingService } from 'app/core/services/billing/billing.service';
import { SubmissionQuoteReportData } from '@app/modules/dashboard/data/reports/submission-quote.data';
import { ReportsService } from '@app/modules/dashboard/services/reports.service';

@Component({
  selector: 'app-submission-quote-modal',
  templateUrl: './submission-quote-modal.component.html',
  styleUrls: ['./submission-quote-modal.component.scss']
})
export class SubmissionQuoteModalComponent extends BaseClass implements OnInit {
  modalRef: BsModalRef | null;
  datePipe: DatePipe;
  datePickerDateOption: IAngularMyDpOptions;
  fromDate: string;
  toDate: string;
  public errorMessageConstant = ErrorMessageConstant;

  constructor(
    public bsModalRef: BsModalRef,
    public submissionQuoteReportData: SubmissionQuoteReportData,
    private reportService: ReportsService,
  ) {
    super();
  }
  
  ngOnInit() {
    this.submissionQuoteReportData.initializeFields();
    this.datePipe = new DatePipe('en-US');
    
    this.datePickerDateOption = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };
    
    // this.populateFields();
  }

  exportSubmissionQuoteReport(): void {
    Utils.blockUI();
    const submissionQuoteReportRequestDTO: SubmissionQuoteReportRequestDTO = {
      fromDate: this.datePipe.transform(this.submissionQuoteReportData.submissionQuoteReportForm.get('fromDate').value.singleDate.jsDate, 'MM/dd/yyyy'),
      toDate: this.datePipe.transform(this.submissionQuoteReportData.submissionQuoteReportForm.get('toDate').value.singleDate.jsDate, 'MM/dd/yyyy')
    };

    this.reportService.generateSubmissionQuoteReport(submissionQuoteReportRequestDTO).pipe((takeUntil(this.stop$))).subscribe((resp) => {
      let downloadLink = document.createElement('a');
      downloadLink.href = window.URL.createObjectURL(resp.body);
      downloadLink.setAttribute('download', "SubmissionQuoteReport");
      document.body.appendChild(downloadLink);
      downloadLink.click();
      downloadLink.remove();
      Utils.unblockUI();
    }, err => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(err));
  });
  }

  downloadFile(data: any) {
    const url= window.URL.createObjectURL(data);
    const a = document.createElement('a');
    const fileName = "SubmissionQuoteReport";
    a.href = url;
    // a.download = fileName;
    a.download = fileName + '.xlsx';
    a.click();
    window.URL.revokeObjectURL(url);
  }

  hideModal(): void {
    this.bsModalRef.hide();
  }

  get isToDateLessThanfromDate(): boolean {
    var fromDate = this.datePipe.transform(this.submissionQuoteReportData.submissionQuoteReportForm.get('fromDate').value?.singleDate?.jsDate, 'yyyy-MM-dd');
    var toDate = this.datePipe.transform(this.submissionQuoteReportData.submissionQuoteReportForm.get('toDate').value?.singleDate?.jsDate, 'yyyy-MM-dd');
    return toDate < fromDate;
  }
}
