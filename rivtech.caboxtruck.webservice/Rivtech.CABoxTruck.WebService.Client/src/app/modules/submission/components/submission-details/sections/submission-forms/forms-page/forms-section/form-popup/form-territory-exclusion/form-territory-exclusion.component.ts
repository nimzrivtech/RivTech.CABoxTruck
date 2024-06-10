import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { Subject } from 'rxjs';
import { FormsData } from '../../../../../../../../../../modules/submission/data/forms/forms.data';
import { SubmissionData } from '../../../../../../../../../../modules/submission/data/submission.data';
import Utils from '../../../../../../../../../../shared/utilities/utils';
import { finalize } from 'rxjs/operators';
import { FormsService } from '../../../../../../../../../../modules/submission/services/forms.service';
import { FormOtherInfoDTO } from '../../../../../../../../../../shared/models/submission/forms/FormOtherInfoDto';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { SelectItem } from '@app/shared/models/dynamic/select-item';

@Component({
  selector: 'app-form-territory-exclusion',
  templateUrl: './form-territory-exclusion.component.html',
  styleUrls: ['./form-territory-exclusion.component.scss']
})
export class FormTerritoryExclusionComponent implements OnInit {
  formId: '';
  other: '';
  dropdownSettings: IDropdownSettings = {};

  selectedStateCodes = [];
  selectedStates = [];

  public onClose: Subject<boolean>;
  constructor(
    public modalRef: BsModalRef,
    public formsData: FormsData,
    private submissionData: SubmissionData,
    private formsService: FormsService
  ) {
  }

  ngOnInit() {
    this.formsData.territoryExclusionForm.reset();
    if (this.other) {
      this.selectedStateCodes = JSON.parse(this.other)?.states?.split(",");
      this.selectedStates = this.formsData.stateList.filter(s => this.selectedStateCodes.includes(s.value));
    }

    this.dropdownSettings = {
      singleSelection: false,
      idField: 'value',
      textField: 'label',
      selectAllText: 'Select All',
      unSelectAllText: 'Unselect All',
      itemsShowLimit: 3,
      allowSearchFilter: true
    };
  }

  get f() { return this.formsData.territoryExclusionForm.controls; }

  saveForm() {
    Utils.blockUI();
    const selectedStates = this.formsData.territoryExclusionForm.value.states as SelectItem[];
    const otherInfo = new FormOtherInfoDTO({ states: selectedStates.map(s => s.value).join(","), statesDefault: false });

    otherInfo.formId = this.formId;
    otherInfo.riskDetailId = this.submissionData?.riskDetail?.id;
    this.formsService.updateTerritoryExclusion(otherInfo)
      .pipe(finalize(() => { Utils.unblockUI(); this.modalRef.hide(); }))
      .subscribe(data => {
        const riskForm = this.formsData.forms.find(f => f.id === data.id);
        riskForm.other = data.other;
      });
  }

  get stateList() {
    return this.formsData.stateList.sort((a, b) => a.label < b.label ? -1 : 1);
  }

  onCancel() {
    const riskForm = this.formsData.forms.find(f => f.form.id === this.formId);
    riskForm.isSelected = false;
    this.formsData.updateRiskForm(riskForm);
    this.modalRef.hide();
  }

}
