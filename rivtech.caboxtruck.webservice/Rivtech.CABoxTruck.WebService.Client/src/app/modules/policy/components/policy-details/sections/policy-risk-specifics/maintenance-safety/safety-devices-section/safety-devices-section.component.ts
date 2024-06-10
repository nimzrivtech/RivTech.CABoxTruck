import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../../../../../shared/base-component';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { SelectItem } from '../../../../../../../../shared/models/dynamic/select-item';
import { SafetyDeviceConstants } from '../../../../../../../../shared/constants/risk-specifics/maintenance-safety.label.constants';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { RiskDTO } from '@app/shared/models/risk/riskDto';
@Component({
  selector: 'app-safety-devices-section',
  templateUrl: './safety-devices-section.component.html',
  styleUrls: ['./safety-devices-section.component.scss']
})
export class SafetyDevicesSectionComponent extends BaseComponent implements OnInit {
  hideMe: boolean = false;
  public SafetyDeviceConstants = SafetyDeviceConstants;
  dropdownSettings: IDropdownSettings = {};

  constructor(public riskSpecificsData: RiskSpecificsData, public submissionData: SubmissionData) {
    super();
  }

  ngOnInit() {
    this.dropdownSettings = {
      singleSelection: false,
      idField: 'value',
      textField: 'label',
      selectAllText: 'Select All',
      unSelectAllText: 'Unselect All',
      itemsShowLimit: 3,
      allowSearchFilter: true
    };
    this.riskSpecificsData.isInitialMaintenanceAndSatefy = false;
  }

  get fg() { return this.riskSpecificsData.riskSpecificsForms.safetyDevicesForm; }
  get fc() { return this.fg.controls; }
  get fa() { return this.fc.safetyDevice as FormArray; }
  get dropdownList() { return this.riskSpecificsData.riskSpecificsDropdownsList; }

  get safetyDeviceCategoryList() { return this.dropdownList.safetyDeviceCategoryList.sort(this.sortByLabel); }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  onItemSelect(item: SelectItem) {
    const safetyDevice = this.riskSpecificsData.safetyDeviceForm(item);
    const formValue = this.fa;
    formValue.push(safetyDevice);
    this.fg.setControl('safetyDevice', formValue);
  }

  onSelectAll(items: SelectItem[]) {
    const formValue = this.fc.safetyDevice as FormArray;

    items.forEach(item => {
      const itemIndex = formValue.controls.findIndex(x => x.get('safetyDeviceCategoryId').value === item.value)
      if (itemIndex === -1) {
        formValue.push(this.riskSpecificsData.safetyDeviceForm(item));
      }
    });

    this.fg.setControl('safetyDevice', formValue);
  }

  onItemDeSelect(item: SelectItem) {
    const formValue = this.fc.safetyDevice as FormArray;
    formValue.removeAt(formValue.controls.findIndex(x => x.get('safetyDeviceCategoryId').value === item.value));
    this.fg.setControl('safetyDevice', formValue);
  }

  onDeSelectAll(items: SelectItem[]) {
    let formValue = this.fc.safetyDevice as FormArray;
    formValue.clear();
    this.fg.setControl('safetyDevice', formValue);
  }

  onFilterChange(value) {
    this.selectAllTag.input.parentElement.hidden = (value != '');
  }

  onDropDownClose() {
    this.selectAllTag.input.parentElement.hidden = false;
  }

  private sortByLabel(a: SelectItem, b: SelectItem) {
    return a.label < b.label ? -1 : a.label > b.label ? 1 : 0;
  }

  get hasMaintenanceQuestions() { return this.submissionData.riskDetail?.vehicles?.length > 5; }

  get selectAllTag() {
    const id = 'safetyDeviceCategory';
    const selectAllCheckbox = <HTMLInputElement>document.querySelector(`ng-multiselect-dropdown#${id} li.multiselect-item-checkbox input`);
    const selectAllCheckboxLabel = <HTMLInputElement>document.querySelector(`ng-multiselect-dropdown#${id} li.multiselect-item-checkbox div`);
    return {
      input: selectAllCheckbox,
      label: selectAllCheckboxLabel
    };
  }

}
