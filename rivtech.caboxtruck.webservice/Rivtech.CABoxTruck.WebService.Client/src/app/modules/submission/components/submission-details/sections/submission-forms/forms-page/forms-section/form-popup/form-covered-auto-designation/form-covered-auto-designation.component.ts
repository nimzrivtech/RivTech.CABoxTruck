import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { Subject } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { FormsData } from '../../../../../../../../../../modules/submission/data/forms/forms.data';
import { SubmissionData } from '../../../../../../../../../../modules/submission/data/submission.data';
import { FormsService } from '../../../../../../../../../../modules/submission/services/forms.service';
import { FormOtherInfoDTO } from '../../../../../../../../../../shared/models/submission/forms/FormOtherInfoDto';
import Utils from '../../../../../../../../../../shared/utilities/utils';
import { Validators } from '@angular/forms';

@Component({
  selector: 'app-form-covered-auto-designation',
  templateUrl: './form-covered-auto-designation.component.html',
  styleUrls: ['./form-covered-auto-designation.component.scss']
})
export class FormCoveredAutoDesignationComponent implements OnInit {
  formId: '';
  other: '';
  selectedCoveredAutoText: any;
  
  constructor(
    public modalRef: BsModalRef,
    public formsData: FormsData,
    private submissionData: SubmissionData,
    private formsService: FormsService
  ) {
  }

  coveredAutoTexts: any[];

  ngOnInit() {
    this.formsData.coveredAutoDesignationForm.reset();
    if (this.other) {
      this.selectedCoveredAutoText = JSON.parse(this.other)?.coveredAutoText;
      this.formsData.coveredAutoDesignationForm.patchValue(JSON.parse(this.other));
    }

    this.formsService.getSmybolTenResponses(this.submissionData?.riskDetail?.id).pipe().subscribe(data => {
      this.coveredAutoTexts = data.map(s => ({
        value: s.id,
        label: s.description
      }));
    });

    if(this.selectedCoveredAutoText == null)
      this.selectedCoveredAutoText = 1;

    this.onCoveredAutoTextChange();
  }

  get f() { return this.formsData.coveredAutoDesignationForm.controls; }

  setTooltip(field, dataList) {
    if (field && dataList) {
      const data = dataList.find(x => x.value == field);
      if (data) {
        return data.label;
      }
    }
  }

  saveForm() {
    Utils.blockUI();
    const otherInfo = new FormOtherInfoDTO(this.formsData.coveredAutoDesignationForm.value);
    console.log(otherInfo);
    otherInfo.formId = this.formId;
    otherInfo.riskDetailId = this.submissionData?.riskDetail?.id;
    this.formsService.updateCoveredAutoDesignationSymbol(otherInfo)
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

  onCoveredAutoTextChange() {
    var fg_textForEndorsement = this.formsData.coveredAutoDesignationForm.get('textForEndorsement');
    if(this.selectedCoveredAutoText != 6) {
      fg_textForEndorsement.setValidators(null);
      fg_textForEndorsement.disable();
    } else {
      fg_textForEndorsement.setValidators([Validators.required]);
      fg_textForEndorsement.enable();
    }
  }

  shortenTextWithEllipsis(text, maxLength) {
    if (text.length <= maxLength) {
      return text;
    } else {
      return text.substring(0, maxLength) + '...';
    }
  }
}
