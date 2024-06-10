import { Component, OnInit } from '@angular/core';
import { PolicyBillingLabelsConstants } from '../../../../../../../shared/constants/policy-billing.labels.constants';
import { BaseClass } from '../../../../../../../shared/base-class';
import { PolicyBillingData } from '../../../../../data/policy-billing.data';
import { InstallmentInvoiceDTO } from '../../../../../../../shared/models/policy/billing/installment-invoice.dto';
import { SubmissionData } from '../../../../../../submission/data/submission.data';
import { BillingService } from 'app/core/services/billing/billing.service';
import { DocumentData } from '../../../../../../../modules/documents/document.data';
import { DocumentFile } from '../../../../../../../modules/documents/document-file';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import NotifUtils from '../../../../../../..//shared/utilities/notif-utils';
import Utils from '../../../../../../..//shared/utilities/utils';
import { NotificationMessageConstant } from 'app/shared/constants/notification-message.constants';
import { InstallmentScheduleModalComponent } from './installment-schedule-modal/installment-schedule-modal.component';
import { FormsData } from '@app/modules/submission/data/forms/forms.data';
import { TableConstants } from 'app/shared/constants/table.constants';

@Component({
  selector: 'app-installment-schedule',
  templateUrl: './installment-schedule.component.html',
  styleUrls: ['./installment-schedule.component.scss']
})
export class InstallmentScheduleComponent extends BaseClass implements OnInit {
  isOpen: boolean = true;
  public documentFiles: any[];

  public installmentScheduleConstants = PolicyBillingLabelsConstants.installmentSchedule;

  installmentSchedules: InstallmentInvoiceDTO[];
  
  TableConstants = TableConstants;

  constructor(public policyBillingData: PolicyBillingData,
    public submissionData: SubmissionData,
    public documentData: DocumentData,
    private formsData: FormsData,
    public billingService: BillingService,
    private modalService: BsModalService) {
    super();
  }

  ngOnInit() {
    
    this.documentData.policy = true;
    this.documentData.loadDocuments();
    const risk = this.submissionData.riskDetail;
    if (risk?.id) {
      this.policyBillingData.listInstallmentInvoice(risk?.riskId);
    }
  }

  collapse(): void {
    this.isOpen = !this.isOpen;
  }

  onViewClick(invoice: InstallmentInvoiceDTO): void {
    const invoiceId = invoice.invoiceId;
    this.documentFiles = this.documentData.billingDocumentsListData;

    const documentFile = this.documentFiles?.find(x => x.tableRefId === invoiceId);

    if (documentFile !== undefined && documentFile !== null) {
      this.documentData.checkDocumentIsUploaded(documentFile.id).subscribe(data => {
        if (data == true) {
          // const docUrl = decodeURIComponent(documentFile.filePath);
          this.documentData.getSASURL(documentFile.filePath).pipe().take(1).subscribe(data => {
            const win = window.open(data, '_blank');
            win.focus();
          });
        } else {
          NotifUtils.showInfo(NotificationMessageConstant.fileNotYetReadyErrorMessage);
        }
      });
      return;
    }

    Utils.blockUI();
    this.formsData.viewInvoice(invoice);
    return;
  }

  onAddEditInstallment(installmentSchedule: InstallmentInvoiceDTO): void {
    
    const initialState = {
      installmentId : installmentSchedule?.installmentId,
      billDate : installmentSchedule?.billDate,
      dueDate : installmentSchedule?.dueDate
    };

    this.modalService.show(InstallmentScheduleModalComponent, {
      initialState,
      backdrop: true,
      ignoreBackdropClick: true,
      class: 'modal-md',
    });
  }

  get isInstallmentPlan(): boolean {
    return this.policyBillingData?.summary?.paymentPlan !== 'Full Pay' && this.policyBillingData?.summary?.paymentPlan !== 'Premium Financed';
  }
}
