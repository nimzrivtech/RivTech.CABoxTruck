import { Component, ElementRef, EventEmitter, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { BindingData } from '../../../../../../../../modules/submission/data/binding/binding.data';
import { BsModalRef } from 'ngx-bootstrap';
import { FileUploadDocumentDto } from '../../../../../../../../shared/models/submission/binding/FileUploadDocumentDto';
import { DocumentsConstants } from '@app/shared/constants/documents.constants';

@Component({
  selector: 'app-upload-document',
  templateUrl: './upload-document.component.html',
  styleUrls: ['./upload-document.component.scss'],
  encapsulation: ViewEncapsulation.None // Add this line
})
export class UploadDocumentComponent implements OnInit, OnDestroy {
  modalRef: BsModalRef;
  modalTitle: string = '';
  editItem: any;
  modalButton: string = '';
  filesSelected = 'No File Chosen';
  acceptedFileTypes = DocumentsConstants.acceptedFileTypes.join(', ');
  selectedFiles: File[] = [];
  dragOver: boolean = false;
  selectedFile: any;
  selectedFileName = '';

  @ViewChild('fileSelection') fileSelection: ElementRef;
  public event: EventEmitter<any> = new EventEmitter();

  constructor(
    public modalRefUpload: BsModalRef,
    public Data: BindingData) { }


  get fg() { return this.Data.formFileUpload; }
  get fc() { return this.fg.controls; }

  ngOnInit() {
    const body = document.getElementsByTagName('modal-container')[1];
    body.classList.add('backdropStatic');

    this.filesSelected = this.getSelectedFileCount();
  }

  selectFile() {
    this.fileSelection.nativeElement.value = null;
    this.fileSelection.nativeElement.click();
  }

  getSelectedFileCount() {
    const fname = this.Data.formFileUpload.controls.fileName.value ?? '';
    const selectedFiles = (fname !== '') ? (fname.split('\n')) : [];
    return (selectedFiles.length === 0) ? 'No File Chosen' : `(${selectedFiles.length}) File(s) Chosen`;
  }

  fileChange(fileInfo: any) {
    const fileNames: any = new Array<string>();

    Array.from(fileInfo).forEach(f => {
      fileNames.push((<File>f).name);
    });

    const fileValidation = {
      fileList: fileInfo,
      invalidTypeNames: [],
      invalidSizeNames: []
    };
    this.fc.fileValidation.setValue(fileValidation);
    this.fc.fileInfo.setValue(fileInfo);
    this.fc.fileName.setValue(fileNames.join('\r\n'));
    this.filesSelected = `(${fileNames.length}) File(s) Chosen`;
    this.fc.fileInfo.markAsTouched();
  }

  onCancel() {
    this.modalRefUpload.hide();
    this.Data.initiateFileUploadDocumentDtoFormGroup(new FileUploadDocumentDto(this.editItem.value));
    // this.Data.formFileUpload = Object.assign({}, this.editItem);
  }

  SubmitAttachment(fileInfo: any) {
    if (this.fg.valid) {
      this.modalRefUpload.hide();
    }
  }

  ngOnDestroy() {
    this.event.emit({ attachment: this.fg });
  }

  get invalidFileSizeErrorMessage() {
    return DocumentsConstants.maxFileSizeExceeded;
  }

  get invalidSizeNames() {
    return this.fc['fileValidation'].value?.invalidSizeNames.join(', ');
  }

  get invalidFileTypeErrorMessage() {
    const fileTypes = `"${DocumentsConstants.acceptedFileTypes.join('", "')}"`;
    return DocumentsConstants.invalidFileTypeErrorMessage
      .replace('{0}', this.fc['fileValidation'].value?.invalidTypeNames.join(', '))
      .replace('{1}', fileTypes);
  }

  setFileForUpload(fileInfo: any) {
    const fileNames: any = new Array<string>();

    Array.from(fileInfo).forEach(f => {
      fileNames.push((<File>f).name);
    });

    const fileValidation = {
      fileList: fileInfo,
      invalidTypeNames: [],
      invalidSizeNames: []
    };

    this.fc.fileValidation.setValue(fileValidation);
    this.fc.fileInfo.setValue(fileInfo);
    this.fc.fileName.setValue(fileNames.join('\r\n'));
    this.filesSelected = `(${fileNames.length}) File(s) Chosen`;
    console.log(this.fg);
    this.selectedFile = fileInfo;
    this.selectedFileName = fileNames.join('\r\n');
  }

  onDragOver(event: any) {
    event.preventDefault();
    event.stopPropagation();
    this.dragOver = true;
  }

  onDrop(event: any) {
    event.preventDefault();
    event.stopPropagation();

    this.dragOver = false;
    this.selectedFiles = event.dataTransfer.files;
    this.setFileForUpload(this.selectedFiles);
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.dragOver = false;
  }

  onFileSelected(event: any) {
    this.selectedFiles = event.target.files;
    this.setFileForUpload(this.selectedFiles);
  }
}
