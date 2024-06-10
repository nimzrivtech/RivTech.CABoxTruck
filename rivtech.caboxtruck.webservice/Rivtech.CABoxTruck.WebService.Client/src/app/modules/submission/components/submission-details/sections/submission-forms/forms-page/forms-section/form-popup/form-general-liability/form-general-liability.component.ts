import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { Subject } from 'rxjs';
import { FormsData } from '../../../../../../../../../../modules/submission/data/forms/forms.data';
import { SubmissionData } from '../../../../../../../../../../modules/submission/data/submission.data';
import Utils from '../../../../../../../../../../shared/utilities/utils';
import { finalize } from 'rxjs/operators';
import { FormsService } from '../../../../../../../../../../modules/submission/services/forms.service';
import { FormOtherInfoDTO } from '../../../../../../../../../../shared/models/submission/forms/FormOtherInfoDto';


@Component({
  selector: 'app-form-general-liability',
  templateUrl: './form-general-liability.component.html',
  styleUrls: ['./form-general-liability.component.scss']
})
export class FormGeneralLiabilityComponent implements OnInit {
  formId: '';
  other: '';
  default: any;

  public onClose: Subject<boolean>;
  constructor(
    public modalRef: BsModalRef,
    public formsData: FormsData,
    private submissionData: SubmissionData,
    private formsService: FormsService
  ) {
    // Set Defaults
    this.default = {
      injuryLimit: 1000000,
      medicalExpenses: 5000,
      damagesRentedToYou: 100000,
      aggregateLimit: 2000000
    };
  }

  ngOnInit() {
    this.formsData.generalLiabilityForm.reset();
    if (this.other) {
      this.formsData.generalLiabilityForm.patchValue(JSON.parse(this.other));
    } else {
      // Set Defaults
      this.formsData.generalLiabilityForm.get('injuryLimit').patchValue(this.default.injuryLimit);
      this.formsData.generalLiabilityForm.get('medicalExpenses').patchValue(this.default.medicalExpenses);
      this.formsData.generalLiabilityForm.get('damagesRentedToYou').patchValue(this.default.damagesRentedToYou);
      this.formsData.generalLiabilityForm.get('aggregateLimit').patchValue(this.default.aggregateLimit);
    }
  }


  get f() { return this.formsData.generalLiabilityForm.controls; }

  saveForm() {
    Utils.blockUI();
    const otherInfo = new FormOtherInfoDTO(this.formsData.generalLiabilityForm.value);
    otherInfo.formId = this.formId;
    otherInfo.riskDetailId = this.submissionData?.riskDetail?.id;
    this.formsService.updateGeneralLiability(otherInfo)
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

}
