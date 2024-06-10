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
import { PaymentPostingReportData } from '@app/modules/dashboard/data/reports/payment-posting.data';
import { PaymentPostingDTO, PaymentPostingInputDTO } from '@app/shared/models/dashboard/paymentPostingDto';
import jspdf from 'jspdf';
import { ReportUtils } from '@app/shared/utilities/report.utils';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-payment-posting-modal',
  templateUrl: './payment-posting-modal.component.html',
  styleUrls: ['./payment-posting-modal.component.scss']
})
export class PaymentPostingModalComponent extends BaseClass implements OnInit {
  modalRef: BsModalRef | null;
  datePipe: DatePipe;
  datePickerDateOption: IAngularMyDpOptions;
  fromDate: string;
  toDate: string;
  public errorMessageConstant = ErrorMessageConstant;

  constructor(
    public bsModalRef: BsModalRef,
    public paymentPostingData: PaymentPostingReportData,
    private reportService: ReportsService,
    private currencyPipe: CurrencyPipe,
    private percentPipe: PercentPipe,
    private toastr: ToastrService
  ) {
    super();
  }

  ngOnInit() {
    this.paymentPostingData.initializeFields();
    this.datePipe = new DatePipe('en-US');

    this.datePickerDateOption = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };
  }

  get fg() { return this.paymentPostingData.paymentPostingForm; }

  generateExcelReport() {
    Utils.blockUI();
    this.generatePaymentPosting((data) => {
      if (data.length > 0) {
        const pdfDate = new Date().toLocaleString('en-US', { timeZone: 'America/New_York' });
        const fromDate = this.datePipe.transform(this.paymentPostingData.paymentPostingForm.get('fromDate').value.singleDate.jsDate, 'MMddyyyy');
        const toDate = this.datePipe.transform(this.paymentPostingData.paymentPostingForm.get('toDate').value.singleDate.jsDate, 'MMddyyyy');
        const reportfileName = `PaymentPosting_${fromDate}-${toDate}`;
        const rows: any[] = [];
        rows.push(this.paymentPostingData.reportHeaders); // add headers

        data.forEach(row => {
          const record = {
            policyNumber: row.policyNumber,
            companyName: row.companyName,
            broker: row.broker,
            creditedOffice: row.creditedOffice,
            checkNumber: row.checkNumber,
            postDate: row.paymentDate == null ? '' : formatDate(row.paymentDate, 'MM/dd/yyyy', 'en'),
            total: this.currencyPipe.transform(row.paymentAmount ?? 0),
            premiumAmountPD: this.currencyPipe.transform(row.premiumAmountPD ?? 0),
            premiumAmountAL: this.currencyPipe.transform(row.premiumAmountAL ?? 0),
            premium: this.currencyPipe.transform(row.premium ?? 0),
            tax: this.currencyPipe.transform(row.tax ?? 0),
            nonPremium: this.currencyPipe.transform(row.nonPremium ?? 0),
            overPayment: this.currencyPipe.transform(row.overPayment ?? 0),
            brokerCommisionAL: this.percentPipe.transform((row.brokerCommisionAL ?? 0) / 100, '0.0-10'),
            brokerCommisionPD: this.percentPipe.transform((row.brokerCommisionPD ?? 0) / 100, '0.0-10'),
            commisionALAmount: this.currencyPipe.transform(row.commisionALAmount ?? 0),
            commisionPDAmount: this.currencyPipe.transform(row.commisionPDAmount ?? 0),
            commissionTotal: this.currencyPipe.transform(row.commissionTotal ?? 0),
            percentageARS: this.percentPipe.transform((row.percentageARS ?? 0) / 100, '0.0-10'),
            commissionARS: this.currencyPipe.transform(row.commissionARS ?? 0),
            commisionRivAL: this.currencyPipe.transform(row.commisionRivAL ?? 0),
            commisionRivPD: this.currencyPipe.transform(row.commisionRivPD ?? 0),
            commisionToRivAmnt: this.currencyPipe.transform(row.commisionToRivAmnt ?? 0),
            premiumDueToCarier: this.currencyPipe.transform(row.premiumDueToCarier ?? 0),
            paymentTypesId : row.paymentTypesId == 1 ? 'Direct Bill Full Pay' : row.paymentTypesId == 2 ? 'Direct Bill Installment' : 'Premium Financed'
          };
          console.log(row.premiumAmountPD);
          console.log(row.premiumAmountAL);
          console.log(record.premiumAmountPD);
          console.log(record.premiumAmountAL);
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
        { title: 'Policy Number', dataKey: 'policyNumber', width: 95 },
        { title: 'Insured', dataKey: 'insured', width: 150 },
        { title: 'Check #', dataKey: 'checkNumber', width: 95 },
        { title: 'Total', dataKey: 'total', width: 60 },
        { title: 'Premium', dataKey: 'premium', width: 60 },
        { title: 'Tax', dataKey: 'tax', width: 60 },
        { title: 'Non-Premium', dataKey: 'nonPremium', width: 60 },
        { title: 'Over Payment', dataKey: 'overPayment', width: 60 },
    ];

    const pdfDoc = new jspdf('l', 'pt');
    let totalPage = 0;
    let _list = [];
    let counter = 0;

    const fromDate = this.datePipe.transform(this.paymentPostingData.paymentPostingForm.get('fromDate').value.singleDate.jsDate, 'MM_dd_yyyy');
    const toDate = this.datePipe.transform(this.paymentPostingData.paymentPostingForm.get('toDate').value.singleDate.jsDate, 'MM_dd_yyyy');
    const reportfileName = `PaymentPosting_${fromDate}-${toDate}`;

    this.generatePaymentPosting((policyDetails) => {
        if (policyDetails.length > 0) {
            const totalTransactions = this.currencyPipe.transform(policyDetails.map(a => a.paymentAmount).reduce((sum, x) => sum + (x ?? 0)));
            policyDetails.forEach(row => {
                const policyDetail = {
                    policyNumber: row.policyNumber,
                    insured: row.companyName,
                    checkNumber: row.checkNumber,
                    total: this.currencyPipe.transform(row.paymentAmount ?? 0),
                    premium: this.currencyPipe.transform(row.premium ?? 0),
                    tax: this.currencyPipe.transform(row.tax ?? 0),
                    nonPremium: this.currencyPipe.transform(row.nonPremium ?? 0),
                    overPayment: this.currencyPipe.transform(row.overPayment ?? 0)
                };
                _list.push(policyDetail);

                if (policyDetails.length === counter + 1) {
                    this.writeTablePP(pdfDoc, columns, _list, 75, policyDetails.length, totalTransactions);
                    if (policyDetails.length !== counter + 1) { // if last page do not add extra page
                        pdfDoc.addPage();
                    }

                    totalPage++;
                    _list = [];
                }
                counter++;
            });
            // pdfDoc.output('dataurlnewwindow', { filename: 'PaymentPosting.pdf' });
            const blobfile = pdfDoc.output('blob');
            this.paymentPostingData.savePaymentPostingPdf(blobfile, reportfileName);
        } else {
            this.toastr.warning(`No Payment Posted`);
        }
    });
}

  private generatePaymentPosting(callbackFn ?: Function) {
    Utils.blockUI();
    const paymentPostingInput: PaymentPostingInputDTO = {
        fromDate: this.datePipe.transform(this.paymentPostingData.paymentPostingForm.get('fromDate').value.singleDate.jsDate, 'MM/dd/yyyy'),
        toDate: this.datePipe.transform(this.paymentPostingData.paymentPostingForm.get('toDate').value.singleDate.jsDate, 'MM/dd/yyyy')
    };
    this.reportService.generatePaymentPosting(paymentPostingInput).pipe(takeUntil(this.stop$)).subscribe((data) => {
        Utils.unblockUI();
        if (callbackFn) { callbackFn(data); }
    }, err => {
        Utils.unblockUI();
        NotifUtils.showError(JSON.stringify(err));
    });
}

  private writeTablePP(doc, columns, data, y, totalCount, totalTransactions) {
    const fromDate = this.datePipe.transform(this.paymentPostingData.paymentPostingForm.get('fromDate').value.singleDate.jsDate, 'MM/dd/yyyy');
    const toDate = this.datePipe.transform(this.paymentPostingData.paymentPostingForm.get('toDate').value.singleDate.jsDate, 'MM/dd/yyyy');
    const totalPagesExp = '{total_pages_count_string}';
    const pdfDate = new Date().toLocaleString('en-US', { timeZone: 'America/New_York' });
    doc.autoTable({
        columns: columns,
        body: data,
        margin: { right: 50, left: 50, top: 80, bottom: 70 },
        theme: 'plain',
        startY: y,
        styles: {
            fontSize: 9
        },
        headStyles: {
            // fillColor: [51, 74, 94],
            fontSize: 9,
            valign: 'middle', halign: 'center'
        },
        columnStyles: {
            0: { cellWidth: 150 },
            1: { cellWidth: 130 },
            2: { cellWidth: 90 },
            3: { cellWidth: 75 },
            4: { cellWidth: 75 },
            5: { cellWidth: 75 },
            6: { cellWidth: 75 },
            7: { cellWidth: 75 },
        },
        rowPageBreak: 'avoid',
        didDrawPage: function (data) {
            doc.setFontSize(12);
            doc.setFont(undefined, 'bold');
            // doc.setTextColor(100, 100, 100);
            const textA = 'Texas Insurance Company';
            const xOffsetA = (doc.internal.pageSize.width / 2) - (doc.getStringUnitWidth(textA) * doc.internal.getFontSize() / 2);
            doc.text(textA, xOffsetA, 40);

            const textB = 'Payment Postings';
            const xOffsetB = (doc.internal.pageSize.width / 2) - (doc.getStringUnitWidth(textB) * doc.internal.getFontSize() / 2);
            doc.text(textB, xOffsetB, 55);
            doc.setFont(undefined, 'normal');
            const textC = formatDate(fromDate, 'MM/dd/yyyy', 'en') + ' - ' + formatDate(toDate, 'MM/dd/yyyy', 'en');
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
                // data.cell.styles.lineWidth = 0.1;
                if (data.column.dataKey === 'total' || data.column.dataKey === 'premium' || data.column.dataKey === 'tax' || data.column.dataKey === 'nonPremium' || data.column.dataKey === 'overPayment') {
                    data.cell.styles.halign = 'right';
                }

            }
            if (data.row.section === 'head') {
                data.cell.styles.minCellHeight = 27;
                data.cell.styles.halign = 'left';
                if (data.column.dataKey === 'total' || data.column.dataKey === 'premium' || data.column.dataKey === 'tax' || data.column.dataKey === 'nonPremium' || data.column.dataKey === 'overPayment') {
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

    const textFooterB = `Total: ${totalTransactions}`;
    const xOffset = (doc.internal.pageSize.width) - (doc.getStringUnitWidth(textFooterB) * doc.internal.getFontSize()) - 50;
    doc.text(textFooterB, xOffset, doc.lastAutoTable.finalY + 30);

    if (typeof doc.putTotalPages === 'function') {
        doc.putTotalPages(totalPagesExp);
    }
    doc.setFontSize(10);
}

onClose(): void {
    this.bsModalRef.hide();
}

  get isToDateLessThanfromDate(): boolean {
    const fromDate = this.datePipe.transform(this.paymentPostingData.paymentPostingForm.get('fromDate').value?.singleDate?.jsDate, 'yyyy-MM-dd');
    const toDate = this.datePipe.transform(this.paymentPostingData.paymentPostingForm.get('toDate').value?.singleDate?.jsDate, 'yyyy-MM-dd');
    return toDate < fromDate;
}
}
