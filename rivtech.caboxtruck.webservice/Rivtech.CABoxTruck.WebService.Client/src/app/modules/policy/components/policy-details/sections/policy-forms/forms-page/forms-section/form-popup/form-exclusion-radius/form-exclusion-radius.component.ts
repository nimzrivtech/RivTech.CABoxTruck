import { Component, OnInit } from '@angular/core';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { BsModalRef } from 'ngx-bootstrap';
import { Subject } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { FormsData } from '../../../../../../../../../../modules/submission/data/forms/forms.data';
import { SubmissionData } from '../../../../../../../../../../modules/submission/data/submission.data';
import { FormsService } from '../../../../../../../../../../modules/submission/services/forms.service';
import { FormOtherInfoDTO } from '../../../../../../../../../../shared/models/submission/forms/FormOtherInfoDto';
import Utils from '../../../../../../../../../../shared/utilities/utils';

@Component({
  selector: 'app-exclusion-radius',
  templateUrl: './form-exclusion-radius.component.html',
  styleUrls: ['./form-exclusion-radius.component.scss']
})
export class FormExclusionRadiusComponent implements OnInit {
  formId: '';
  other: '';

  public onClose: Subject<boolean>;
  constructor(
    public modalRef: BsModalRef,
    public formsData: FormsData,
    private submissionData: SubmissionData,
    private formsService: FormsService,
    private policySummaryData: PolicySummaryData
  ) {
  }

  ngOnInit() {
    this.formsData.exclusionRadiusForm.reset();
    if (this.other) {
      this.formsData.exclusionRadiusForm.patchValue(JSON.parse(this.other));
    }
  }

  get f() { return this.formsData.exclusionRadiusForm.controls; }

  saveForm() {
    Utils.blockUI();
    const otherInfo = new FormOtherInfoDTO(this.formsData.exclusionRadiusForm.value);
    otherInfo.formId = this.formId;
    otherInfo.riskDetailId = this.submissionData?.riskDetail?.id;
    this.formsService.updateExclusionRadius(otherInfo)
      .pipe(finalize(() => { 
        this.policySummaryData.updateSubtatusForPendingEndorsement();
        Utils.unblockUI(); 
        this.modalRef.hide(); }))
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
}
