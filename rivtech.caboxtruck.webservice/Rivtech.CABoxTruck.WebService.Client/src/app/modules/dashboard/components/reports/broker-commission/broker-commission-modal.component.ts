import { Component, OnInit } from '@angular/core';
import { BaseClass } from 'app/shared/base-class';
import { CurrencyPipe, DatePipe, formatDate, PercentPipe } from '@angular/common';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { BsModalRef } from 'ngx-bootstrap';
import Utils from 'app/shared/utilities/utils';
import NotifUtils from 'app/shared/utilities/notif-utils';
import { ErrorMessageConstant } from 'app/shared/constants/error-message.constants';
import { takeUntil } from 'rxjs/operators';
import { ReportsService } from '@app/modules/dashboard/services/reports.service';
import { PaymentPostingInputDTO } from '@app/shared/models/dashboard/paymentPostingDto';
import jspdf from 'jspdf';
import { ReportUtils } from '@app/shared/utilities/report.utils';
import { BrokerCommissionReportData } from '@app/modules/dashboard/data/reports/broker-commission.data';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-broker-commission-modal',
  templateUrl: './broker-commission-modal.component.html',
  styleUrls: ['./broker-commission-modal.component.scss']
})
export class BrokerCommissionModalComponent extends BaseClass implements OnInit {
  modalRef: BsModalRef | null;
  datePipe: DatePipe;
  datePickerDateOption: IAngularMyDpOptions;
  fromDate: string;
  toDate: string;
  public errorMessageConstant = ErrorMessageConstant;

  constructor(
    public bsModalRef: BsModalRef,
    public brokerCommissionData: BrokerCommissionReportData,
    private reportService: ReportsService,
    private currencyPipe: CurrencyPipe,
    private percentPipe: PercentPipe,
    private toastr: ToastrService
  ) {
    super();
  }

  ngOnInit() {
    this.brokerCommissionData.initializeFields();
    this.datePipe = new DatePipe('en-US');

    this.datePickerDateOption = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };
  }

  get fg() { return this.brokerCommissionData.brokerCommissionForm; }

  generateExcelReport() {
    Utils.blockUI();
    this.generateBrokerCommission((data) => {
      if (data.length > 0) {
        const pdfDate = new Date().toLocaleString('en-US', { timeZone: 'America/New_York' });
        const fromDate = this.datePipe.transform(this.brokerCommissionData.brokerCommissionForm.get('fromDate').value.singleDate.jsDate, 'MMddyyyy');
        const toDate = this.datePipe.transform(this.brokerCommissionData.brokerCommissionForm.get('toDate').value.singleDate.jsDate, 'MMddyyyy');
        const reportfileName = `BrokerCommission_${fromDate}-${toDate}`;
        const rows: any[] = [];
        rows.push(this.brokerCommissionData.reportHeaders); // add headers

        data.forEach(row => {
          const record = {
            creditedOffice: row.creditedOffice,
            broker: row.broker,
            policyNumber: row.policyNumber,
            insured: row.companyName,
            checkNumber: row.checkNumber,
            postDate: row.paymentDate == null ? '' : formatDate(row.paymentDate, 'MM/dd/yyyy', 'en'),
            paymentAmount: this.currencyPipe.transform(row.paymentAmount ?? 0),
            brokerCommisionAL: this.percentPipe.transform((row.brokerCommisionAL ?? 0) / 100, '0.0-10'),
            brokerCommisionPD: this.percentPipe.transform((row.brokerCommisionPD ?? 0) / 100, '0.0-10'),
            commisionALAmount: this.currencyPipe.transform(row.commisionALAmount ?? 0),
            commisionPDAmount: this.currencyPipe.transform(row.commisionPDAmount ?? 0),
            commissionTotal: this.currencyPipe.transform(row.commissionTotal ?? 0),
            percentageARS: this.percentPipe.transform((row.percentageARS ?? 0) / 100, '0.0-10'),
            commissionARS: this.currencyPipe.transform(row.commissionARS ?? 0)
          };
          rows.push(record);
        });

        ReportUtils.exportToCsv(reportfileName, rows);
      } else {
        this.toastr.warning(`No Payment Posted`);
      }
    });
  }

  generatePDFReport() {
    const columns = [
      { title: 'Credited Office', dataKey: 'creditedOffice', width: 20 },
      { title: 'Policy Number', dataKey: 'policyNumber', width: 95 },
      { title: 'Insured', dataKey: 'insured', width: 150 },
      { title: 'Check #', dataKey: 'checkNumber', width: 95 },
      { title: 'Payment Post Date', dataKey: 'postDate', width: 95 },
      { title: 'Premium Posted', dataKey: 'paymentAmount', width: 60},
      { title: 'AL Commission %', dataKey: 'brokerCommisionAL', width: 60 },
      { title: 'PD Commission %', dataKey: 'brokerCommisionPD', width: 60 },
      { title: 'AL Commission $', dataKey: 'commisionALAmount', width: 60 },
      { title: 'PD Commission $', dataKey: 'commisionPDAmount', width: 60 },
      { title: 'Total $', dataKey: 'commissionTotal', width: 60 },
      { title: 'ARS %', dataKey: 'percentageARS', width: 60 },
      { title: 'ARS Commission $', dataKey: 'commissionARS', width: 60 },
    ];

    const pdfDoc = new jspdf('l', 'pt');
    let totalPage = 0;
    let _list = [];
    let BrokerName = '';
    let counter = 0;
    let totalTransactions = 0;
    let totalCommissions = 0;

    const fromDate = this.datePipe.transform(this.brokerCommissionData.brokerCommissionForm.get('fromDate').value.singleDate.jsDate, 'MM_dd_yyyy');
    const toDate = this.datePipe.transform(this.brokerCommissionData.brokerCommissionForm.get('toDate').value.singleDate.jsDate, 'MM_dd_yyyy');
    const reportfileName = `BrokerCommission_${fromDate}-${toDate}`;

    this.generateBrokerCommission((policyDetails) => {
      if (policyDetails.length > 0) {
        // const totalTransactions = this.currencyPipe.transform(policyDetails.map(a => a.paymentAmount).reduce((sum, x) => sum + (x ?? 0)));
        // const totalCommissions = this.currencyPipe.transform(policyDetails.map(a => a.commissionTotal).reduce((sum, x) => sum + (x ?? 0)));
        policyDetails.forEach(row => {
          const policyDetail = {
            creditedOffice: row.creditedOffice,
            policyNumber: row.policyNumber,
            insured: row.companyName,
            checkNumber: row.checkNumber,
            postDate: row.paymentDate == null ? '' : formatDate(row.paymentDate, 'MM/dd/yyyy', 'en'),
            paymentAmount: this.currencyPipe.transform(row.paymentAmount ?? 0),
            brokerCommisionAL: this.percentPipe.transform((row.brokerCommisionAL ?? 0) / 100, '0.0-10'),
            brokerCommisionPD: this.percentPipe.transform((row.brokerCommisionPD ?? 0) / 100, '0.0-10'),
            commisionALAmount: this.currencyPipe.transform(row.commisionALAmount ?? 0),
            commisionPDAmount: this.currencyPipe.transform(row.commisionPDAmount ?? 0),
            commissionTotal: this.currencyPipe.transform(row.commissionTotal ?? 0),
            percentageARS: this.percentPipe.transform((row.percentageARS ?? 0) / 100, '0.0-10'),
            commissionARS: this.currencyPipe.transform(row.commissionARS ?? 0)
          };
          _list.push(policyDetail);
          totalTransactions = totalTransactions + (row.paymentAmount ?? 0);
          totalCommissions = totalCommissions + (row.commissionTotal ?? 0);
          if (policyDetails.length === counter + 1 || policyDetails[counter + 1].broker !== row.broker) {
            this.writeTablePP(pdfDoc, columns, _list, 75, policyDetails.length, this.currencyPipe.transform(totalTransactions), this.currencyPipe.transform(totalCommissions), row.broker);
            if (policyDetails.length !== counter + 1) { // if last page do not add extra page
              pdfDoc.addPage();
            }

            totalPage++;
            _list = [];
            BrokerName = row.broker;
            totalTransactions = 0;
            totalCommissions = 0;
          }
          counter++;
        });
        // pdfDoc.output('dataurlnewwindow', { filename: 'PaymentPosting.pdf' });
        const blobfile = pdfDoc.output('blob');
        this.brokerCommissionData.saveBrokerCommissionPdf(blobfile, reportfileName);
      } else {
        this.toastr.warning(`No Payment Posted`);
      }
    });
  }

  private generateBrokerCommission(callbackFn?: Function) {
    Utils.blockUI();
    const brokerCommissionInput: PaymentPostingInputDTO = {
      fromDate: this.datePipe.transform(this.brokerCommissionData.brokerCommissionForm.get('fromDate').value.singleDate.jsDate, 'MM/dd/yyyy'),
      toDate: this.datePipe.transform(this.brokerCommissionData.brokerCommissionForm.get('toDate').value.singleDate.jsDate, 'MM/dd/yyyy')
    };
    this.reportService.generateBrokerCommission(brokerCommissionInput).pipe(takeUntil(this.stop$)).subscribe((data) => {
        Utils.unblockUI();
        if (callbackFn) { callbackFn(data); }
    }, err => {
        Utils.unblockUI();
        NotifUtils.showError(JSON.stringify(err));
    });
  }

  private writeTablePP(doc, columns, data, y, totalCount, totalTransactions, totalCommissions, broker) {
    const fromDate = this.datePipe.transform(this.brokerCommissionData.brokerCommissionForm.get('fromDate').value.singleDate.jsDate, 'MM/dd/yyyy');
    const toDate = this.datePipe.transform(this.brokerCommissionData.brokerCommissionForm.get('toDate').value.singleDate.jsDate, 'MM/dd/yyyy');
    const totalPagesExp = '{total_pages_count_string}';
    const pdfDate = new Date().toLocaleString('en-US', { timeZone: 'America/New_York' });
    doc.autoTable({
      columns: columns,
      body: data,
      margin: { right: 40, left: 40, top: 80, bottom: 70 },
      theme: 'plain',
      startY: y,
      styles: {
        fontSize: 7
      },
      headStyles: {
        // fillColor: [51, 74, 94],
        fontSize: 9,
        valign: 'middle', halign: 'center'
      },
      columnStyles: {
        0: {cellWidth: 50},
        1: {cellWidth: 80},
        2: {cellWidth: 70},
        3: {cellWidth: 50},
        4: {cellWidth: 55},
        5: {cellWidth: 50},
        6: {cellWidth: 68},
        7: {cellWidth: 68},
        8: {cellWidth: 68},
        9: {cellWidth: 68},
        10: {cellWidth: 40},
        11: {cellWidth: 40},
        12: {cellWidth: 68},
      },
      rowPageBreak: 'avoid',
      didDrawPage: function (data) {
        doc.setFontSize(12);
        doc.setFont(undefined, 'bold');
        // doc.setTextColor(100, 100, 100);
        // const textA = 'Texas Insurance Company';
        // const xOffsetA = (doc.internal.pageSize.width / 2) - (doc.getStringUnitWidth(textA) * doc.internal.getFontSize() / 2);
        // doc.text(textA, xOffsetA, 40);

        const textB = 'Commission Report';
        const xOffsetB = (doc.internal.pageSize.width / 2) - (doc.getStringUnitWidth(textB) * doc.internal.getFontSize() / 2);
        doc.text(textB, xOffsetB, 40);
        doc.setFont(undefined, 'normal');
        const textC = 'Broker: ' + broker;
        const xOffsetCC = (doc.internal.pageSize.width / 2) - (doc.getStringUnitWidth(textC) * doc.internal.getFontSize() / 2);
        doc.text(textC, xOffsetCC, 55);

        const textD =  'Payments Posted Between ' + formatDate(fromDate, 'MM/dd/yyyy', 'en') + ' - ' + formatDate(toDate, 'MM/dd/yyyy', 'en');
        const xOffsetDD = (doc.internal.pageSize.width / 2) - (doc.getStringUnitWidth(textD) * doc.internal.getFontSize() / 2);
        doc.text(textD, xOffsetDD, 70);

        doc.setFontSize(9);
      },
      didDrawCell: function (data) {
        // if (data.row.section === 'head') {

        //   }
      },
      didParseCell: function (data) {
        if (data.row.section === 'body') {
          // data.cell.styles.lineWidth = 0.1;
          if (data.column.dataKey === 'paymentAmount' || data.column.dataKey === 'brokerCommisionAL' || data.column.dataKey === 'brokerCommisionPD' ||
                    data.column.dataKey === 'commisionALAmount' || data.column.dataKey === 'commisionPDAmount' || data.column.dataKey === 'commissionTotal' ||
                      data.column.dataKey === 'percentageARS' || data.column.dataKey === 'commissionARS') {
            data.cell.styles.halign = 'right';
          }

        }
        if (data.row.section === 'head') {
          data.cell.styles.minCellHeight = 27;
          data.cell.styles.halign = 'left';
          if (data.column.dataKey === 'paymentAmount' || data.column.dataKey === 'brokerCommisionAL' || data.column.dataKey === 'brokerCommisionPD' ||
                    data.column.dataKey === 'commisionALAmount' || data.column.dataKey === 'commisionPDAmount' || data.column.dataKey === 'commissionTotal' ||
                      data.column.dataKey === 'percentageARS' || data.column.dataKey === 'commissionARS') {
            data.cell.styles.halign = 'right';
          }
        }
      },
      // avoidPageSplit: false,
      // horizontalPageBreak: true,
    });

    doc.setFont(undefined, 'bold');
    doc.setFontSize(10);
    const textFooterA = `${totalCount} Transactions`;
    doc.text(textFooterA, 50, doc.lastAutoTable.finalY + 30);

    // const textFooterB = 'Totals:';
    // doc.text(textFooterB, 300, doc.lastAutoTable.finalY + 30);

    const textFooterC = `Totals: ${totalTransactions}`;
    const xOffsetC = (doc.internal.pageSize.width) - (doc.getStringUnitWidth(textFooterC) * doc.internal.getFontSize()) - 385;
    doc.text(textFooterC, xOffsetC, doc.lastAutoTable.finalY + 30);

    const textFooterD = `${totalCommissions}`;
    const xOffsetD = (doc.internal.pageSize.width) - (doc.getStringUnitWidth(textFooterD) * doc.internal.getFontSize()) - 40;
    doc.text(textFooterD, xOffsetD, doc.lastAutoTable.finalY + 30);

    if (typeof doc.putTotalPages === 'function') {
        doc.putTotalPages(totalPagesExp);
    }
    doc.setFontSize(10);
  }

  onClose(): void {
    this.bsModalRef.hide();
  }

  get isToDateLessThanfromDate(): boolean {
    const fromDate = this.datePipe.transform(this.brokerCommissionData.brokerCommissionForm.get('fromDate').value?.singleDate?.jsDate, 'yyyy-MM-dd');
    const toDate = this.datePipe.transform(this.brokerCommissionData.brokerCommissionForm.get('toDate').value?.singleDate?.jsDate, 'yyyy-MM-dd');
    return toDate < fromDate;
  }
}
