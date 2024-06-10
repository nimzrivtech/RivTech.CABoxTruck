import { Component, OnInit, ViewChild } from '@angular/core';
import { BindingData } from '../../../../../../../modules/submission/data/binding/binding.data';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { BsModalService } from 'ngx-bootstrap';
import { BindingRequirementsDto } from '../../../../../../../shared/models/submission/binding/BindingRequirementsDto';
import { TableConstants } from '../../../../../../../shared/constants/table.constants';
import { FileUploadDocumentDto } from '../../../../../../../shared/models/submission/binding/FileUploadDocumentDto';
import { AddQuoteConditionComponent } from '../modals/add-quote-condition/add-quote-condition.component';
import { QuoteConditionsDto } from '../../../../../../../shared/models/submission/binding/QuoteConditionsDto';
import { Subject } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { AuthService } from '../../../../../../../core/services/auth.service';
import { UserRole } from '../../../../../../../shared/constants/user-roles.constants';
import { DocumentApiService } from '@app/modules/documents/document-api.service';


@Component({
  selector: 'app-quote-condition',
  templateUrl: './quote-condition.component.html',
  styleUrls: ['./quote-condition.component.scss']
})
export class QuoteConditionComponent implements OnInit {
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;
  TableConstants = TableConstants;
  dtElement: DataTableDirective;
  dtOptions: DataTables.Settings = {
    pagingType: 'full_numbers',
    responsive: true,
    processing: true,
    destroy: true
  };
  dtTrigger: Subject<any> = new Subject();
  isDtInitialized: boolean = false;

  constructor(
    public Data: BindingData,
    public submissionData: SubmissionData,
    private modalService: BsModalService,
    private authService: AuthService,
    private documentService: DocumentApiService
  ) { }


  ngOnInit() {
    this.renderTable();
    this.setConditionsPage(1);
    this.changeShowRowsNumberQuoteConditions(10);
  }

  setConditionsPage(page: number) {
    this.Data.setConditionsPage(page, this.Data.quoteConditionsList);
  }

  openAddItemModal() {
    const obj = new QuoteConditionsDto();
    this.Data.initiateQuoteConditionFormGroup(obj);
    this.Data.formQuoteConditons.reset();

    const modalRef = this.modalService.show(AddQuoteConditionComponent, {
      initialState: {
        modalTitle: 'Add Quote Condition',
        modalButton: 'Save',
        editItem: null,
        isEdit: false,
      },
      backdrop: 'static',
      keyboard: false,
      class: 'modal-md modal-dialog-centered',
      // modal-dialog-centered makes modal centered
    });

    modalRef.content.modalRef = modalRef;
  }

  openEditItemModal(obj) {
    if (!this.CanEditOrDeleteQuoteCondition(obj)) { return; }
    this.Data.initiateQuoteConditionFormGroup(obj);

    if (!this.Data.formFileUpload) {
      const file = new FileUploadDocumentDto();
      file.createdBy = Number(this.authService.getCurrentUserId());
      file.createdDate = new Date();
      this.Data.initiateFileUploadDocumentDtoFormGroup(file);
    }
    this.Data.formFileUpload.reset();

    this.Data.getFileUploadByRefId(obj.id);

    const modalRef = this.modalService.show(AddQuoteConditionComponent, {
      initialState: {
        modalTitle: 'Edit Quote Condition',
        modalButton: 'Save',
        editItem: obj,
        isEdit: true,
      },
      backdrop: 'static',
      keyboard: false,
      class: 'modal-md modal-dialog-centered'
    });

    modalRef.content.modalRef = modalRef;
  }

  CanEditOrDeleteQuoteCondition(quoteCondition: QuoteConditionsDto) {
    const currentUserId = Number(this.authService.getCurrentUserId());
    return (this.isRoleUnderwriter && quoteCondition.isDefault) ||
            (!quoteCondition.isDefault && quoteCondition.createdBy === currentUserId && this.isRoleUnderwriter);
  }

  get isRoleUnderwriter(): boolean {
    // Only UW can edit and delete Quote Conditions
    return this.authService.getCurrentUserRole() === UserRole.Underwriter;
  }

  deleteQuoteCondition(quoteCondition: QuoteConditionsDto) {
    if (!this.CanEditOrDeleteQuoteCondition(quoteCondition)) { return; }
    this.Data.DeleteQuoteConditions(quoteCondition);
  }

  renderTable() {
    if (this.isDtInitialized) {
      this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
        // Destroy the table first
        dtInstance.destroy();
        // Call the dtTrigger to rerender again
        this.dtTrigger.next();
      });
    } else {
      this.dtTrigger.next();
      this.isDtInitialized = true;
    }
  }

  viewDocument(obj) {
    // const docUrl = decodeURIComponent(obj.filePath);
    this.documentService.getDocumentSASURL(obj.filePath).pipe().take(1).subscribe(data => {
      const win = window.open(data, '_blank');
      win.focus();
    });
  }

  get isAttachmentExist() {
    return this.Data.conditionsPagedItems?.find(x => x.filePath != null) ? true : false;
  }
  
  changeShowRowsNumberQuoteConditions(showRowsNumber: number) {
    this.Data.changeShowRowsNumberQuoteConditions(1, showRowsNumber);
  }
}
