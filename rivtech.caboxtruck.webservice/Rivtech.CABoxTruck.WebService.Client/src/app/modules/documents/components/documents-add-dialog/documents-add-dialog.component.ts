import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import Utils from '../../../../shared/utilities/utils';
import { BsModalRef } from 'ngx-bootstrap';
import { DocumentData } from '../../document.data';
import { DocumentFile } from '../../document-file';
import { DocumentsConstants } from '../../../../shared/constants/documents.constants';
import { invalidFileTypeValidator, invalidFileSizeValidator } from '../../../../modules/submission/helpers/file.validator';
import { fileSizeValidatorMultiple, fileTypeValidatorMultiple } from '../../../../modules/submission/helpers/file.validator';
import { SubmissionData } from '@app/modules/submission/data/submission.data';

@Component({
  selector: 'app-documents-add-dialog',
  templateUrl: './documents-add-dialog.component.html',
  styleUrls: ['./documents-add-dialog.component.css']
})
export class DocumentsAddDialogComponent implements OnInit {

  frmDocument: FormGroup;
  selectedFile: any;
  selectedFileName = '';
  selectedFiles: File[] = [];
  dragOver: boolean = false;
  acceptedFileTypes = DocumentsConstants.acceptedFileTypes.join(', ');
  @ViewChild('fileSelection') fileSelection: ElementRef;
  filesSelected = 'No File Chosen';
  @Input() category: string;
  fileCategories: any[];

  constructor(
    public modalRef: BsModalRef,
    public documentData: DocumentData,
    private formBuilder: FormBuilder,
    private submissionData: SubmissionData
  ) { }

  get isNew() {
    return this.documentData.isFormDocumentNew;
  }

  get f() { return this.frmDocument.controls; }

  ngOnInit(): void {
    this.initializeForm();

    this.selectedFileName = this.documentData.documentFormData.fileName ?? '';
    this.filesSelected = this.getSelectedFileCount();
    this.fileCategories = this.documentData.categories;

    if (this.category === 'Billing') {
      const billingCategory = this.documentData.categories.find(x => x.description === this.category);
      this.f['fileCategoryId'].patchValue(billingCategory?.id);
    } else if (this.category === 'Policy') {
      if (this.documentData.policy) {
        this.fileCategories = this.documentData.categories.filter(x => x.description !== 'Billing');
      }
    }
  }

  initializeForm() {
    this.frmDocument = this.formBuilder.group({
      id: [null],
      tableRefId: [null],
      riskDetailId: [null],
      fileCategoryId: [null, Validators.required],
      category: [null],
      description: [null, Validators.required],
      fileName: [null, [Validators.required]],
      filePath: [null],
      createdDate: [null],
      dateAdded: [null],
      checked: [null],
      fileinfo: [null],
      // fileInfo: [null, [invalidFileSizeValidator, invalidFileTypeValidator]],
      fileValidation: [null, [fileTypeValidatorMultiple, fileSizeValidatorMultiple]]
    });

    if (this.documentData.documentForm?.id === null) {
      this.documentData.documentForm.dateAdded = { isRange: false, singleDate: { jsDate: new Date(this.submissionData.currentServerDateTime) } };
    }
    this.frmDocument.patchValue(this.documentData.documentForm);
  }

  save(): void {
    try {
      this.documentData.saveDocument(this.frmDocument.value as DocumentFile, this.selectedFile, this.modalRef);
    } catch (e) {
      console.log(e);
    } finally { }
  }

  close() {
    this.modalRef.hide();
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

    this.f.fileValidation.setValue(fileValidation);
    this.f.fileinfo.setValue(fileInfo);
    this.f.fileName.setValue(fileNames.join('\r\n'));
    this.filesSelected = `(${fileNames.length}) File(s) Chosen`;


    this.selectedFile = fileInfo;
    this.selectedFileName = fileNames.join('\r\n');
    // this.f['fileName'].setValue(this.selectedFileName);
    // this.f['fileInfo'].setValue(fileInfo);
  }

  getSelectedFileCount() {
    const fname = this.frmDocument.controls.fileName.value ?? '';
    const selectedFiles = (fname !== '') ? (fname.split('\n')) : [];
    return (selectedFiles.length === 0) ? 'No File Chosen' : `(${selectedFiles.length}) File(s) Chosen`;
  }

  selectFile() {
    this.fileSelection.nativeElement.value = null;
    this.fileSelection.nativeElement.click();
  }

  get isDisabled() {
    return true;
  }

  get invalidFileSizeErrorMessage() {
    return DocumentsConstants.maxFileSizeExceeded;
  }

  get invalidSizeNames() {
    return this.f['fileValidation'].value?.invalidSizeNames.join(', ');
  }

  get invalidFileTypeErrorMessage() {
    const fileTypes = `"${DocumentsConstants.acceptedFileTypes.join('", "')}"`;
    return DocumentsConstants.invalidFileTypeErrorMessage
      // .replace('{0}', this.selectedFileName)
      .replace('{0}', this.f['fileValidation'].value?.invalidTypeNames.join(', '))
      .replace('{1}', fileTypes);
  }

  get isInvalidFilename() {
    if (this.selectedFileName.length > 500) {
      return true;
    }

    return false;
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
