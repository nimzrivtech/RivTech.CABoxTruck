import { Component, OnInit, Input } from '@angular/core';
import { BaseClass } from 'app/shared/base-class';
import { CurrencyPipe, DatePipe, formatDate, PercentPipe } from '@angular/common';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { BsModalRef } from 'ngx-bootstrap';
import Utils from 'app/shared/utilities/utils';
import NotifUtils from 'app/shared/utilities/notif-utils';
import { ErrorMessageConstant } from 'app/shared/constants/error-message.constants';
import { takeUntil } from 'rxjs/operators';
import { ReportsService } from '@app/modules/dashboard/services/reports.service';
import { ReportUtils } from '@app/shared/utilities/report.utils';
import { ToastrService } from 'ngx-toastr';
import { PreBindPaymentReportData } from '@app/modules/dashboard/data/reports/prebind-payment.data';
import { PreBindPaymentInputDTO } from '@app/shared/models/dashboard/prebindPaymentDto';

@Component({
  selector: 'app-prebind-payment-modal',
  templateUrl: './prebind-payment-modal.component.html',
  styleUrls: ['./prebind-payment-modal.component.scss']
})
export class PreBindPaymentModalComponent extends BaseClass implements OnInit {
  modalRef: BsModalRef | null;
  datePipe: DatePipe;
  datePickerDateOption: IAngularMyDpOptions;
  fromDate: string;
  toDate: string;
  public errorMessageConstant = ErrorMessageConstant;

  constructor(
    public bsModalRef: BsModalRef,
    public preBindPaymentReportData: PreBindPaymentReportData,
    private reportService: ReportsService,
    private currencyPipe: CurrencyPipe,
    private percentPipe: PercentPipe,
    private toastr: ToastrService
  ) {
    super();
  }

  ngOnInit() {
    this.preBindPaymentReportData.initializeFields();
    this.datePipe = new DatePipe('en-US');

    this.datePickerDateOption = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };
  }

  get fg() { return this.preBindPaymentReportData.prebindPaymentForm; }

  generateExcelReport() {
    Utils.blockUI();
    this.generatePreBindPayments((data) => {
      if (data.length > 0) {
        const fromDate = this.datePipe.transform(this.preBindPaymentReportData.prebindPaymentForm.get('fromDate').value.singleDate.jsDate, 'MMddyyyy');
        const toDate = this.datePipe.transform(this.preBindPaymentReportData.prebindPaymentForm.get('toDate').value.singleDate.jsDate, 'MMddyyyy');
        const reportfileName = `PreBindPayments_${fromDate}-${toDate}`;
        const rows: any[] = [];
        rows.push(this.preBindPaymentReportData.reportHeaders); // add headers

        data.forEach(row => {
          const record = {
            insuredName: row.insuredName,
            accessCode: row.accessCode,
            payeeName: row.payeeName,
            payeeEmail: row.payeeEmail,
            paymentDateTime: row.paymentDateTime == null ? '' : formatDate(row.paymentDateTime, 'MM/dd/yyyy', 'en'),
            paymentMethod: row.paymentMethod,
            paymentAmount: this.currencyPipe.transform(row.paymentAmount ?? 0),
            confirmationCode: row.confirmationCode,
          };
          rows.push(record);
        });

        ReportUtils.exportToCsv(reportfileName, rows);
      } else {
        this.toastr.warning(`No Payment Posted`);
      }
    });
  }

  private generatePreBindPayments(callbackFn?: Function) {
    Utils.blockUI();
    const prebindPaymentInput: PreBindPaymentInputDTO = {
      fromDate: this.datePipe.transform(this.preBindPaymentReportData.prebindPaymentForm.get('fromDate').value.singleDate.jsDate, 'MM/dd/yyyy'),
      toDate: this.datePipe.transform(this.preBindPaymentReportData.prebindPaymentForm.get('toDate').value.singleDate.jsDate, 'MM/dd/yyyy')
    };
    this.reportService.generatePreBindPayments(prebindPaymentInput).pipe(takeUntil(this.stop$)).subscribe((data) => {
        Utils.unblockUI();
        if (callbackFn) { callbackFn(data); }
    }, err => {
        Utils.unblockUI();
        NotifUtils.showError(JSON.stringify(err));
    });
  }

  onClose(): void {
    this.bsModalRef.hide();
  }

  get isToDateLessThanfromDate(): boolean {
    const fromDate = this.datePipe.transform(this.preBindPaymentReportData.prebindPaymentForm.get('fromDate').value?.singleDate?.jsDate, 'yyyy-MM-dd');
    const toDate = this.datePipe.transform(this.preBindPaymentReportData.prebindPaymentForm.get('toDate').value?.singleDate?.jsDate, 'yyyy-MM-dd');
    return toDate < fromDate;
  }
}
