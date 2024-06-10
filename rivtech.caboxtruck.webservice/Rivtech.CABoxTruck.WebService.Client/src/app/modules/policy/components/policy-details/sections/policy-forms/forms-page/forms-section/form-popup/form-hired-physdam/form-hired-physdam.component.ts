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
  selector: 'app-form-hired-physdam',
  templateUrl: './form-hired-physdam.component.html',
  styleUrls: ['./form-hired-physdam.component.scss']
})
export class FormHiredPhysdamComponent implements OnInit {
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
    this.resetForm();
    if (this.other) {
      this.formsData.hiredPhysdamForm.patchValue(JSON.parse(this.other));
    }
  }

  get f() { return this.formsData.hiredPhysdamForm.controls; }

  saveForm() {
    Utils.blockUI();
    const otherInfo = new FormOtherInfoDTO(this.formsData.hiredPhysdamForm.value);
    otherInfo.formId = this.formId;
    otherInfo.riskDetailId = this.submissionData?.riskDetail?.id;
    this.formsService.updatehiredPhysdam(otherInfo)
      .pipe(
        finalize(() => {
          this.formsData.isPolicyUI = true;
          this.formsData.refreshQuoteOptionsAndSummaryHeader();
          this.policySummaryData.updateSubtatusForPendingEndorsement();
          Utils.unblockUI();
          this.modalRef.hide();
        })
      )
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

  private resetForm() {
    const form = this.formsData.hiredPhysdamForm;
    form.reset();
    Object.keys(form.controls).forEach(key => {
      form.get(key).setValue(0);
    });
  }

}
