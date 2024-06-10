import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { Subject } from 'rxjs';
import { FormsData } from '../../../../../../../../../../modules/submission/data/forms/forms.data';
import { SubmissionData } from '../../../../../../../../../../modules/submission/data/submission.data';
import Utils from '../../../../../../../../../../shared/utilities/utils';
import { finalize } from 'rxjs/operators';
import { FormsService } from '../../../../../../../../../../modules/submission/services/forms.service';
import { FormMcs90DTO } from '@app/shared/models/submission/forms/FormMcs90Dto';


@Component({
  selector: 'app-form-mcs90',
  templateUrl: './form-mcs90.component.html',
  styleUrls: ['./form-mcs90.component.scss']
})
export class FormMcs90Component implements OnInit {
  formId: '';
  other: '';

  public onClose: Subject<boolean>;
  constructor(
    public modalRef: BsModalRef,
    public formsData: FormsData,
    private submissionData: SubmissionData,
    private formsService: FormsService
  ) {
  }

  ngOnInit() {
    this.formsData.mcs90Form.reset();
    const otherData = this.other ? JSON.parse(this.other) : this.createDefaults();
    this.formsData.mcs90Form.patchValue(otherData);
    this.formatDateForPicker(otherData);
  }

  get form() { return this.formsData.mcs90Form; }

  get f() { return this.formsData.mcs90Form.controls; }

  saveForm() {
    Utils.blockUI();
    const otherInfo = new FormMcs90DTO(this.formsData.mcs90Form.value);
    otherInfo.formId = this.formId;
    otherInfo.riskDetailId = this.submissionData?.riskDetail?.id;
    this.formsService.updateMcs90(this.formatDatePickerForSave(otherInfo))
      .pipe(finalize(() => { Utils.unblockUI(); this.modalRef.hide(); }))
      .subscribe(data => {
        const riskForm = this.formsData.forms.find(f => f.id === data.id);
        riskForm.other = data.other;
      });
  }

  onCancel() {
    const riskForm = this.formsData.forms.find(f => f.form.id === this.formId);
    riskForm.isSelected = false;
    this.formsData.updateRiskForm(riskForm);
    this.modalRef.hide();
  }

  createDefaults(): FormMcs90DTO {
    const effectiveDateValue = this.submissionData.riskDetail?.brokerInfo?.effectiveDate;
    const effectiveDate = effectiveDateValue ? new Date(effectiveDateValue) : new Date();
    const mcs90 = new FormMcs90DTO({
      usDotNumber: this.submissionData.riskDetail?.filingsInformation?.usdotNumber,
      dateReceived: this.submissionData.riskDetail?.brokerInfo?.effectiveDate,
      issuedTo: this.submissionData.riskDetail?.nameAndAddress?.businessName,
      state: this.submissionData.riskDetail?.nameAndAddress?.state,
      datedAt: 'Omaha, NE',
      policyDateDay: effectiveDate?.getDate(),
      policyDateMonth: effectiveDate?.toLocaleString('default', { month: 'long' }),
      policyDateYear: effectiveDate?.getFullYear(),
      policyNumber: this.submissionData.riskDetail?.submissionNumber,
      effectiveDate: this.submissionData.riskDetail?.brokerInfo?.effectiveDate,
      insuranceCompany: 'Texas Insurance Company',
      counterSignedBy: 'Jeffrey A Silver',
      accidentLimit: 750000
    });

    return mcs90;
  }

  private formatDateForPicker(mcs90: FormMcs90DTO) {
    this.form.get('dateReceived').setValue({ isRange: false, singleDate: { jsDate: new Date(mcs90.dateReceived) } });
    this.form.get('effectiveDate').setValue({ isRange: false, singleDate: { jsDate: new Date(mcs90.effectiveDate) } });
  }

  private formatDatePickerForSave(mcs90: FormMcs90DTO) {
    mcs90.dateReceived = this.form.value?.dateReceived?.singleDate?.jsDate?.toLocaleDateString();
    mcs90.effectiveDate = this.form.value?.effectiveDate.singleDate?.jsDate?.toLocaleDateString();
    return mcs90;
  }

}
