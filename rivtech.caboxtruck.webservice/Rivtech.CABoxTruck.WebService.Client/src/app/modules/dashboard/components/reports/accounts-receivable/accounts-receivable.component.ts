import { CurrencyPipe, formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AccountReceivableReportData } from '@app/modules/dashboard/data/reports/accounts-receivable.data';
import { ReportsService } from '@app/modules/dashboard/services/reports.service';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { BaseClass } from '@app/shared/base-class';
import { AccountsReceivableDTO, AccountsReceivableInputDTO } from '@app/shared/models/dashboard/arDto';
import { AccountReceivableReportDTO } from '@app/shared/models/reports/accounts-receivable-report.model';
import NotifUtils from '@app/shared/utilities/notif-utils';
import { ReportUtils } from '@app/shared/utilities/report.utils';
import Utils from '@app/shared/utilities/utils';
import jspdf from 'jspdf';
import 'jspdf-autotable';
import { BsModalRef } from 'ngx-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-accounts-receivable',
  templateUrl: './accounts-receivable.component.html',
  styleUrls: ['./accounts-receivable.component.scss']
})
export class AccountsReceivableComponent extends BaseClass implements OnInit {

  title: string;

  constructor(public bsModalRef: BsModalRef, private currencyPipe: CurrencyPipe,
    private reportService: ReportsService,
    private toastr: ToastrService,
    public submissionData: SubmissionData,
    public accountsReceivableReportData: AccountReceivableReportData) { super(); }

  ngOnInit() {
    Utils.blockUI();
    this.accountsReceivableReportData.brokerInfoData.getAgency(() => Utils.unblockUI()); // fetch brokers
    this.accountsReceivableReportData.initializeFields(); // intialize fields
  }

  onClose(): void {
    this.bsModalRef.hide();
  }

  get fg() { return this.accountsReceivableReportData.accountsReceivableReportForm; }
  get fc() { return this.fg.controls; }
  get fn() { return 'accountReceivableReportForm'; }

  generatePDFReport() {
    // console.log('Generate PDF Report');
    // console.log(this.fg);
    const columns = [
      { title: 'Policy Number', dataKey: 'policyNumber', width: 95 },
      { title: 'Insured', dataKey: 'insured', width: 150 },
      { title: 'Insured Phone', dataKey: 'insuredPhone', width: 95 },
      { title: 'Inception Date', dataKey: 'inception', width: 60, align: 'center' },
      { title: 'Pending Cancellation', dataKey: 'pendingCancellation', width: 88, align: 'center'},
      { title: 'Past Due Date', dataKey: 'pastDueDate', width: 57, textalign: 'center' },
      { title: 'Past Due Amount', dataKey: 'pastDueAmount', width: 84, align: 'center' },
      { title: 'Due Date', dataKey: 'currentDueDate', width: 37, align: 'center' },
      { title: 'Amount Due', dataKey: 'currentAmountDue', width: 70, align: 'center' },
      { title: 'No Payments', dataKey: 'noPayments', width: 53, align: 'center' }
    ];

    const pdfDoc = new jspdf('l', 'pt');
    // pdfDoc.text('Hello World!', 10, 20);
    // pdfDoc.output('dataurlnewwindow', { filename: 'AccountsReceivable.pdf' });
    let totalPage = 0;
    let _list = [];
    let BrokerName = '';
    let counter = 0;

    this.searchAR((policyDetails) => {
      if (policyDetails.length > 0) {
        policyDetails.forEach(row => {
          const policyDetail = {
            policyNumber: row.policyNumber,
            insured: row.insured,
            insuredPhone: row.insuredPhone,
            inception: row.inception == null ? '' : formatDate(row.inception, 'MM/dd/yyyy', 'en'),
            pendingCancellation: row.pendingCancellation == null ? '' : formatDate(row.pendingCancellation, 'MM/dd/yyyy', 'en'),
            pastDueDate: row.pastDueDate == null ? '' : formatDate(row.pastDueDate, 'MM/dd/yyyy', 'en'),
            pastDueAmount: this.currencyPipe.transform(row.pastDueAmount),
            currentDueDate: row.currentDueDate == null ? '' : formatDate(row.currentDueDate, 'MM/dd/yyyy', 'en'),
            currentAmountDue: this.currencyPipe.transform(row.currentAmountDue),
            noPayments: row.noPayments
          };
          _list.push(policyDetail);

          if (policyDetails.length === counter + 1 || policyDetails[counter + 1].broker !== row.broker) {
            this.writeTableAR(pdfDoc, columns, _list, 75, counter, row.broker);
            // pdfDoc.setFontType("bolditalic");
            if (policyDetails.length !== counter + 1) { // if last page do not add extra page
              pdfDoc.addPage();
            }

            totalPage++;
            _list = [];
            BrokerName = row.broker;
          }
          counter++;
        });
        // pdfDoc.output('dataurlnewwindow', { filename: 'AccountsReceivable.pdf' });
        const blobfile = pdfDoc.output('blob');
        this.accountsReceivableReportData.saveAccountsReceivablePdf(blobfile);
      } else {
        this.toastr.warning(`No AR Records Posted`);
      }
    });
  }

  generateExcelReport() {
    Utils.blockUI();
    this.searchAR((data) => {
      if (data.length > 0) {
        const pdfDate = new Date().toLocaleString('en-US', { timeZone: 'America/New_York' });
        const fileNameValuationDate = this.fg.value.valuationDate === null ? formatDate(pdfDate, 'MM_dd_yyyy', 'en-US') : formatDate(Date.parse(this.fg.value.valuationDate.singleDate.jsDate), 'MM_dd_yyyy', 'en');
        const reportfileName = `AccountsReceivable_${fileNameValuationDate}`;
        const rows: any[] = [];
        rows.push(this.accountsReceivableReportData.reportHeaders); // add headers

        data.forEach(row => {
          const record: AccountsReceivableDTO = {
            policyDetailId: row.policyDetailId,
            broker: row.broker,
            policyNumber: row.policyNumber,
            insured: row.insured,
            insuredPhone: row.insuredPhone,
            inception: row.inception == null ? '' : formatDate(row.inception, 'MM/dd/yyyy', 'en'),
            pendingCancellation: row.pendingCancellation == null ? '' : formatDate(row.pendingCancellation, 'MM/dd/yyyy', 'en'),
            cancellationDate: row.cancellationDate == null ? '' : formatDate(row.cancellationDate, 'MM/dd/yyyy', 'en'),
            pastDueDate: row.pastDueDate == null ? '' : formatDate(row.pastDueDate, 'MM/dd/yyyy', 'en'),
            pastDueAmount: this.currencyPipe.transform(row.pastDueAmount),
            currentDueDate: row.currentDueDate == null ? '' : formatDate(row.currentDueDate, 'MM/dd/yyyy', 'en'),
            currentAmountDue: this.currencyPipe.transform(row.currentAmountDue),
            noPayments: row.noPayments,
            financed: row.financed
          };
          rows.push(record);
        });

        ReportUtils.exportToCsv(reportfileName, rows);
      } else {
        this.toastr.warning(`No AR Records Posted`);
      }
    });
  }

  private searchAR(callbackFn?: Function) {
    Utils.blockUI();
    const arInput: AccountsReceivableInputDTO = {
        sortNameBy: this.fg.value.sortBy,
        receivableType: this.fg.value.receivableType,
        broker: this.fg.value.broker,
        pastDueOnly: this.fg.value.pastDueOnly,
        cancellationStatus: this.fg.value.cancellationStatus,
        noPaymentsMade: this.fg.value.noPaymentsMade,
        valuationDate: this.fg.value.valuationDate !== null ? this.fg.value.valuationDate.singleDate.jsDate : null,
      };
    this.reportService.searchAccountsReceivable(arInput).pipe(takeUntil(this.stop$)).subscribe((data) => {
        Utils.unblockUI();
        if (callbackFn) { callbackFn(data); }
    }, err => {
        Utils.unblockUI();
        NotifUtils.showError(JSON.stringify(err));
    });
  }

  private writeTableAR(doc, columns, data, y, row, broker) {
    const valuationDate = this.fg.value.valuationDate !== null ? this.fg.value.valuationDate.singleDate.jsDate : null;
    const totalPagesExp = '{total_pages_count_string}';
    const pdfDate = new Date().toLocaleString('en-US', { timeZone: 'America/New_York' });
    doc.autoTable({
      columns: columns,
      body: data,
      margin: { right: 10, left: 10, top: 80, bottom: 50 },
      startY: y,
      tableLineColor: [229, 229, 229],
      tableLineWidth: 0.1,
      styles: {
        lineColor: [229, 229, 229],
        tableLineWidth: 0.1,
        fontSize: 9
      },
      headStyles: {
        fillColor: [51, 74, 94],
        fontSize: 9,
        valign: 'middle', halign: 'center'
      },
      bodyStyles: {
        lineColor: [229, 229, 229],
        tableLineWidth: 0.1,
        fontSize: 9
      },
      columnStyles: {
        0: {cellWidth: 150},
        1: {cellWidth: 116.89},
        2: {cellWidth: 80},
        3: {cellWidth: 70},
        4: {cellWidth: 70},
        5: {cellWidth: 70},
        6: {cellWidth: 70},
        7: {cellWidth: 70},
        8: {cellWidth: 70},
        9: {cellWidth: 55},
       },
      rowPageBreak: 'avoid',
      didDrawPage: function (data) {
        doc.setFontSize(12);
        doc.setFont(undefined, 'bold');
        doc.setTextColor(100, 100, 100);
        const textA = 'Texas Insurance Company';
        const xOffsetA = (doc.internal.pageSize.width / 2) - (doc.getStringUnitWidth(textA) * doc.internal.getFontSize() / 2);
        doc.text(textA, xOffsetA, 40);

        const textB = 'Accounts Receivable as of ' + (valuationDate < new Date('11-11-1980') ? formatDate(pdfDate, 'MM/dd/yyyy hh:mm a', 'en-US') : formatDate(valuationDate, 'MM/dd/yyyy hh:mm a', 'en') + ' 11:59PM');
        const xOffsetB = (doc.internal.pageSize.width / 2) - (doc.getStringUnitWidth(textB) * doc.internal.getFontSize() / 2);
        doc.text(textB, xOffsetB, 55);
        doc.setFont(undefined, 'normal');
        const textC = 'Broker: ' + broker;
        const xOffsetC = (doc.internal.pageSize.width / 2) - (doc.getStringUnitWidth(textC) * doc.internal.getFontSize() / 2);
        doc.text(textC, xOffsetC, 70);

        doc.setFontSize(9);
      },
      didDrawCell: function (data) {
        // if (data.row.section === 'head') {

        //   }
      },
      didParseCell: function (data) {
        if (data.row.section === 'body') {
          data.cell.styles.lineWidth = 0.1;
          if (data.column.dataKey === 'pastDueAmount' || data.column.dataKey === 'currentAmountDue') {
            data.cell.styles.halign = 'right';
          }

          if (data.column.dataKey === 'inception' || data.column.dataKey === 'pastDueDate' || data.column.dataKey === 'currentDueDate' || data.column.dataKey === 'noPayments') {
            data.cell.styles.halign = 'center';
          }
        }
        if (data.row.section === 'head') {
          data.cell.styles.minCellHeight = 27;
          data.cell.styles.halign = 'left';
        }
      },
      // avoidPageSplit: false,
      // horizontalPageBreak: true,
    });

    if (typeof doc.putTotalPages === 'function') {
        doc.putTotalPages(totalPagesExp);
    }
    doc.setFontSize(10);
  }
}


