import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { DocumentsAddDialogComponent } from '../documents-add-dialog/documents-add-dialog.component';

@Component({
  selector: 'app-upload-document-button',
  templateUrl: './upload-document-button.component.html',
  styleUrls: ['./upload-document-button.component.scss']
})
export class UploadDocumentButtonComponent implements OnInit {

  modalRef: BsModalRef;

  constructor(
    private modalService: BsModalService
  ) { }

  ngOnInit() {
  }

  upload() {
    this.modalRef = this.modalService.show(DocumentsAddDialogComponent, {
      backdrop: 'static',
      keyboard: false,
      class: 'modal-md'
    });
  }

}
