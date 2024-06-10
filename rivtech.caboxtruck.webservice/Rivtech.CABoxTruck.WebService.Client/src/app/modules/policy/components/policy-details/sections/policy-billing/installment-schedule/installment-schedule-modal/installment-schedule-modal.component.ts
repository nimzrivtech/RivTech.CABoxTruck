import { Component, OnInit, Input } from '@angular/core';
import { BaseClass } from 'app/shared/base-class';
import { PolicyBillingLabelsConstants } from 'app/shared/constants/policy-billing.labels.constants';
import { PolicyBillingData } from 'app/modules/policy/data/policy-billing.data';
import { DatePipe } from '@angular/common';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { BsModalRef } from 'ngx-bootstrap';
import Utils from 'app/shared/utilities/utils';
import NotifUtils from 'app/shared/utilities/notif-utils';
import { ErrorMessageConstant } from 'app/shared/constants/error-message.constants';
import { AddEditInstallmentScheduleRequestDTO } from 'app/shared/models/policy/billing/add-edit-installment-schedule-request.dto';
import { takeUntil } from 'rxjs/operators';
import { BillingService } from 'app/core/services/billing/billing.service';

@Component({
  selector: 'app-installment-schedule-modal',
  templateUrl: './installment-schedule-modal.component.html',
  styleUrls: ['./installment-schedule-modal.component.scss']
})
export class InstallmentScheduleModalComponent extends BaseClass implements OnInit {
  modalRef: BsModalRef | null;
  datePipe: DatePipe;
  datePickerDateOption: IAngularMyDpOptions;
  public installmentScheduleModalConstants = PolicyBillingLabelsConstants.installmentScheduleModal;
  expirationDate: string;
  effectiveDate: string;
  public errorMessageConstant = ErrorMessageConstant;
  @Input() installmentId: string;
  @Input() billDate: string;
  @Input() dueDate: string;

  constructor(
    public billingData: PolicyBillingData,
    public bsModalRef: BsModalRef,
    private billingService: BillingService,
  ) {
    super();
  }
  
  ngOnInit() {
    this.datePipe = new DatePipe('en-US');
    this.expirationDate = this.datePipe.transform(this.billingData.summary.expirationDate, 'yyyy-MM-dd');
    this.effectiveDate = this.datePipe.transform(this.billingData.summary.effectiveDate, 'yyyy-MM-dd');
    this.datePickerDateOption = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };
    
    this.populateFields();
  }

  onAddEditInstallmentSchedule(): void {
    Utils.blockUI();
    const addEditInstallmentScheduleRequest: AddEditInstallmentScheduleRequestDTO = {
      installmentId: this.installmentId === "undefined" ? null : this.installmentId,
      invoiceDate: this.datePipe.transform(this.billingData.postInstallmentScheduleForm.get('invoiceDate').value.singleDate.jsDate, 'yyyy-MM-dd'),
      dueDate: this.datePipe.transform(this.billingData.postInstallmentScheduleForm.get('dueDate').value.singleDate.jsDate, 'yyyy-MM-dd')
    };

    this.billingService.addEditInstallmentScheduleFuture(this.billingData.data.riskId, addEditInstallmentScheduleRequest)
      .pipe(
        takeUntil(this.stop$)
      ).subscribe(_ => {
        this.billingData.populateBillingSections(this.billingData.data.riskId);
        //this.policyNotesData.populateNotes(notes);

        this.bsModalRef.hide();
        Utils.unblockUI();
        NotifUtils.showSuccess(typeof this.installmentId === "undefined" ? PolicyBillingLabelsConstants.successAddInstallmentSchedule : PolicyBillingLabelsConstants.successEditInstallmentSchedule);
      },
        err => {
          Utils.unblockUI();
          NotifUtils.showMultiLineError(err.error?.message);
        }
      );
  }

  populateFields(): void {
    this.billingData.postInstallmentScheduleForm.reset();

    if(typeof this.installmentId === "undefined")
    {
    this.billingData.postInstallmentScheduleForm.get('invoiceDate').setValue({ isRange: false, singleDate: { jsDate: new Date() } });
    
    var futureDate = new Date();
    futureDate.setDate(futureDate.getDate() + 10);
    
    this.billingData.postInstallmentScheduleForm.get('dueDate').setValue({ isRange: false, singleDate: { jsDate: futureDate } });
    }
    else
    {
      const billDate = new Date(this.billDate);
      const dueDate = new Date(this.dueDate);
      
      this.billingData.postInstallmentScheduleForm.get('invoiceDate').setValue({ isRange: false, singleDate: { jsDate: billDate } });
      this.billingData.postInstallmentScheduleForm.get('dueDate').setValue({ isRange: false, singleDate: { jsDate: dueDate } });
    }
    this.billingData.postInstallmentScheduleForm.get('invoiceDate').enable();
    this.billingData.postInstallmentScheduleForm.get('dueDate').enable();
  }

  hideModal(): void {
    this.bsModalRef.hide();
  }

  get isInvoiceDateExceedExpiration(): boolean {
    var invoiceDateExpired = this.datePipe.transform(this.billingData.postInstallmentScheduleForm.get('invoiceDate').value.singleDate.jsDate, 'yyyy-MM-dd');
    return invoiceDateExpired > this.expirationDate;
  }

  get isInvoiceDateLessThanEffectiveDate(): boolean {
    var invoiceDateEffective = this.datePipe.transform(this.billingData.postInstallmentScheduleForm.get('invoiceDate').value.singleDate.jsDate, 'yyyy-MM-dd');
    return invoiceDateEffective < this.effectiveDate;
  }
  get isDueDateExceedExpiration(): boolean {
    var dueDateEffective = this.datePipe.transform(this.billingData.postInstallmentScheduleForm.get('dueDate').value.singleDate.jsDate, 'yyyy-MM-dd');
    return dueDateEffective > this.expirationDate;
  }

  get isDueDateLessThanEffectiveDate(): boolean {
    var dueDateEffective = this.datePipe.transform(this.billingData.postInstallmentScheduleForm.get('dueDate').value.singleDate.jsDate, 'yyyy-MM-dd');
    return dueDateEffective < this.effectiveDate;
  }

  get isDueDateLessThanInvoiceDate(): boolean {
    var invoiceDate = this.datePipe.transform(this.billingData.postInstallmentScheduleForm.get('invoiceDate').value.singleDate.jsDate, 'yyyy-MM-dd');
    var dueDate = this.datePipe.transform(this.billingData.postInstallmentScheduleForm.get('dueDate').value.singleDate.jsDate, 'yyyy-MM-dd');
    return dueDate < invoiceDate;
  }
}
