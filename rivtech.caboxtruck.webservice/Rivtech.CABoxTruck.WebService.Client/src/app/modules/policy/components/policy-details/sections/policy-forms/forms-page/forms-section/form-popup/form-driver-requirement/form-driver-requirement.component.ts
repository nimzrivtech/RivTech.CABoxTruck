import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { Subject } from 'rxjs';
import { FormsData } from '../../../../../../../../../../modules/submission/data/forms/forms.data';
import { SubmissionData } from '../../../../../../../../../../modules/submission/data/submission.data';
import Utils from '../../../../../../../../../../shared/utilities/utils';
import { finalize } from 'rxjs/operators';
import { FormsService } from '../../../../../../../../../../modules/submission/services/forms.service';
import { FormOtherInfoDTO } from '../../../../../../../../../../shared/models/submission/forms/FormOtherInfoDto';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';

@Component({
  selector: 'app-form-driver-requirement',
  templateUrl: './form-driver-requirement.component.html',
  styleUrls: ['./form-driver-requirement.component.scss']
})
export class FormDriverRequirementComponent implements OnInit {
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
    this.formsData.driverRequirementForm.reset();
    if (this.other) {
      this.formsData.driverRequirementForm.patchValue(JSON.parse(this.other));
    }
  }

  get f() { return this.formsData.driverRequirementForm.controls; }

  saveForm() {
    Utils.blockUI();
    const otherInfo = new FormOtherInfoDTO(this.formsData.driverRequirementForm.value);
    otherInfo.formId = this.formId;
    otherInfo.riskDetailId = this.submissionData?.riskDetail?.id;
    this.formsService.updatedriverRequirement(otherInfo)
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
