import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { Subject } from 'rxjs';
import { FormsData } from '../../../../../../../../../modules/submission/data/forms/forms.data';
import { FormDTO, FormInfoDTO } from '../../../../../../../../../shared/models/submission/forms/FormDto';
import { SubmissionData } from '../../../../../../../../../modules/submission/data/submission.data';
import Utils from '../../../../../../../../../shared/utilities/utils';
import { finalize } from 'rxjs/operators';
import { FormsService } from '../../../../../../../../../modules/submission/services/forms.service';
import { HttpEventType } from '@angular/common/http';
import { DocumentsConstants } from '@app/shared/constants/documents.constants';

@Component({
  selector: 'app-form-upload',
  templateUrl: './form-upload.component.html',
  styleUrls: ['./form-upload.component.scss']
})
export class FormUploadComponent implements OnInit {
  selectStatus = 'No File Chosen';
  selectedFileNames = '';
  selectedFile: any;
  acceptedFileTypes = '.pdf'; // DocumentsConstants.acceptedFileTypes.join(', ');
  @ViewChild('fileSelection') fileSelection: ElementRef;

  public onClose: Subject<boolean>;
  constructor(
    public modalRef: BsModalRef,
    public formsData: FormsData,
    private submissionData: SubmissionData,
    private formsService: FormsService
  ) {
  }

  ngOnInit() {
    this.formsData.uploadForm.reset();
  }

  get f() { return this.formsData.uploadForm.controls; }

  selectFile() {
    this.fileSelection.nativeElement.value = null;
    this.fileSelection.nativeElement.click();
  }

  setFileForUpload(fileList: FileList) {
    this.selectedFile = fileList;
    this.selectStatus = `(${fileList.length}) File(s) Chosen`;
    this.selectedFileNames = Array.from(fileList)?.map(o => o.name).join('\n');

    const fileInfo = {
      fileList: fileList,
      invalidTypeNames: [],
      invalidSizeNames: []
    };
    this.f['fileInfo'].setValue(fileInfo);
    this.f['fileName'].setValue(this.selectedFileNames?.replace(/\n/g, '|'));
  }

  saveForm() {
    const fileInfo = this.selectedFile;
    if (fileInfo.length === 0) {
      return;
    }

    const formData = new FormData();

    Array.from(fileInfo).forEach(f => {
      const file = <File>f;
      formData.append('file', file, file.name);
    });

    const riskForm = new FormDTO();
    riskForm.riskDetailId = this.submissionData.riskDetail?.id;
    riskForm.isSelected = true; // selected already by default
    const description = this.f['description'].value;
    const fileName = this.f['fileName'].value;
    const form = new FormInfoDTO();
    form.isSupplementalDocument = true;
    form.name = description;
    form.fileName = fileName;
    riskForm.form = form;
    formData.append('riskForm', JSON.stringify(riskForm));

    Utils.blockUI();
    this.formsService.postUserForm(formData)
      .pipe(finalize(() => { Utils.unblockUI(); this.modalRef.hide(); }))
      .subscribe(data => {
        if (data.type === HttpEventType.Response) {
          data.body.include = true;
          this.formsData.forms.push(data.body);
          this.submissionData.riskDetail.riskForms = this.formsData.forms;
        }
      }, error => {
        console.error(`There was an error on Saving: ${error}`);
      });
  }

  get invalidFileSizeErrorMessage() {
    return DocumentsConstants.maxFileSizeExceeded;
  }

  get invalidSizeNames() {
    return this.f['fileInfo'].value?.invalidSizeNames.join(', ');
  }

  get invalidFileTypeErrorMessage() {
    const fileTypes = `"${this.acceptedFileTypes}"`;
    return DocumentsConstants.customInvalidFileTypeErrorMsg
      .replace('{0}', this.f['fileInfo'].value?.invalidTypeNames.join(', '))
      .replace('{1}', fileTypes);
  }

}
