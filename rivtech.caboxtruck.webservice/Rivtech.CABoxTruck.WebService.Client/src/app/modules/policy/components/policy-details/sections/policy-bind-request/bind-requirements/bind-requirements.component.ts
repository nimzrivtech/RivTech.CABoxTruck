import { Component, OnInit, ViewChild } from '@angular/core';
import { BindingData } from '../../../../../../../modules/submission/data/binding/binding.data';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { AddUpdateBindingComponent } from '../modals/add-binding/add-update-binding.component';
import { BsModalService } from 'ngx-bootstrap';
import { BindingRequirementsDto } from '../../../../../../../shared/models/submission/binding/BindingRequirementsDto';
import { TableConstants } from '../../../../../../../shared/constants/table.constants';
import { FileUploadDocumentDto } from '../../../../../../../shared/models/submission/binding/FileUploadDocumentDto';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { DocumentApiService } from '@app/modules/documents/document-api.service';

@Component({
  selector: 'app-bind-requirements',
  templateUrl: './bind-requirements.component.html',
  styleUrls: ['./bind-requirements.component.scss']
})
export class BindRequirementsComponent implements OnInit {
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
    private modalService: BsModalService,
    public submissionData: SubmissionData,
    private documentService: DocumentApiService
  ) { }

  ngOnInit() {
    this.renderTable();
    this.setRequirementsPage(1);
    this.changeShowRowsNumberRequirements(10);
  }

  setRequirementsPage(page: number) {
    this.Data.setRequirementsPage(page, this.Data.bindingRequirementsList);
  }

  openAddItemModal() {
    const obj = new BindingRequirementsDto();
    obj.bindStatusId = '1';
    obj.isPreBind = true;
    this.Data.initiateBindingRequirementsFormGroup(obj);
    this.Data.formFileUpload.reset();

    const modalRef = this.modalService.show(AddUpdateBindingComponent, {
      initialState: {
        modalTitle: 'Add Bind Requirements',
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
    this.Data.initiateBindingRequirementsFormGroup(obj);

    if (!this.Data.formFileUpload) {
      const file = new FileUploadDocumentDto();
      file.createdBy = 12345;
      file.createdDate = new Date();
      this.Data.initiateFileUploadDocumentDtoFormGroup(file);
    }
    this.Data.formFileUpload.reset();

    this.Data.getFileUploadByRefId(obj.id);

    const modalRef = this.modalService.show(AddUpdateBindingComponent, {
      initialState: {
        modalTitle: 'Edit Bind Requirements',
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

  checkChanged(obj: BindingRequirementsDto, isChecked) {
    obj.isPreBind = isChecked;
    this.Data.SubmitBindRequirements(obj);
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

  changeShowRowsNumberRequirements(showRowsNumber: number) {
    this.Data.changeShowRowsNumberRequirements(1, showRowsNumber);
  }
}
