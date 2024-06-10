import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BindingData } from '../../../../../../data/binding/binding.data';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { UploadDocumentComponent } from '../upload-document/upload-document.component';
import { DocumentData } from '../../../../../../../../modules/documents/document.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';

@Component({
  selector: 'app-add-update-binding',
  templateUrl: './add-update-binding.component.html',
  styleUrls: ['./add-update-binding.component.scss']
})
export class AddUpdateBindingComponent implements OnInit {
  @ViewChild('fileSelection') fileSelection: ElementRef;
  modalRef: BsModalRef;

  modalTitle: string = '';
  modalButton: string = '';
  editItem: any;
  isEdit: boolean = false;
  data: string = '';


  constructor(
    public Data: BindingData,
    private documentData: DocumentData,
    private modalService: BsModalService,
    private submissionData: SubmissionData
  ) { }

  get fg() { return this.Data.formBindingRequirements; }
  get fc() { return this.fg.controls; }
  get fgF() { return this.Data.formFileUpload; }
  get fcF() { return this.fgF.controls; }

  ngOnInit() {
  }

  openUploadDocuModal() {
    if (!this.Data.formFileUpload || this.fc.id.value != this.Data.formFileUpload?.value.tableRefId) {
      // const obj = new FileUploadDocumentDto();
      // obj.createdBy = 12345;
      // obj.createdDate = new Date();
      // obj.description = this.fc.relevantDocumentDesc.value;
      // this.Data.initiateFileUploadDocumentDtoFormGroup(obj);
      this.fgF.get('createdDate').setValue({ isRange: false, singleDate: { jsDate: new Date(this.submissionData.currentServerDateTime) } });
    }

    const modalUpload = this.modalService.show(UploadDocumentComponent, {
      initialState: {
        modalTitle: 'Upload Document',
        modalButton: 'Save',
        editItem: Object.assign({}, this.fgF),
        isEdit: false
      },
      backdrop: 'static',
      //keyboard: false,
      class: 'modal-md modal-dialog-centered',
      // modal-dialog-centered makes modal centered
    });

    modalUpload.content.modalRef = modalUpload;

    modalUpload.content.event.subscribe((result) => {
      const obj = result.attachment;

      //this.fcF.tableRefId.setValue(this.fc.id.value);

      if (this.fc.relevantDocumentDesc.value !== obj.value.description) {
        this.fc.relevantDocumentDesc.setValue(this.fcF.description?.value);
      }
    });
  }

  SubmitBindRequirements() {
    this.Data.SubmitBindRequirements(null, () => {
      this.documentData.loadDocuments();
      this.modalRef.hide();
      this.Data.formFileUpload.reset();
    });
  }

  Cancel() {
    this.Data.formFileUpload.reset();
    this.modalRef.hide();
  }
}
