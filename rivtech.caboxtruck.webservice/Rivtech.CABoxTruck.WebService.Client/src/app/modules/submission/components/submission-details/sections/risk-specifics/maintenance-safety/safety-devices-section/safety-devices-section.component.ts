import { Component, OnInit } from '@angular/core';
import { RiskSpecificsData } from '../../../../../../data/risk-specifics.data';
import { BaseComponent } from '../../../../../../../../shared/base-component';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { SelectItem } from '../../../../../../../../shared/models/dynamic/select-item';
import { SafetyDeviceConstants } from '../../../../../../../../shared/constants/risk-specifics/maintenance-safety.label.constants';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
@Component({
  selector: 'app-safety-devices-section',
  templateUrl: './safety-devices-section.component.html',
  styleUrls: ['./safety-devices-section.component.scss']
})
export class SafetyDevicesSectionComponent extends BaseComponent implements OnInit {
  hideMe: boolean = false;
  public SafetyDeviceConstants = SafetyDeviceConstants;
  dropdownSettings: IDropdownSettings = {};
  multiSelectId: string = '';

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

    // if retrieved from submission list but has no saved values yet, then trigger validations
    if (this.submissionData.riskDetail?.maintenanceSafety?.safetyDevices.safetyDeviceCategory.length < 1
        && this.submissionData?.validationList?.maintenanceSafety === false && !this.riskSpecificsData.isInitialMaintenanceAndSatefy) {
      this.fg.markAllAsTouched();
    }

    this.riskSpecificsData.isInitialMaintenanceAndSatefy = false;
  }

  get fg() { return this.riskSpecificsData.riskSpecificsForms.safetyDevicesForm; }
  get fc() { return this.fg.controls; }
  get fa() { return this.fc.safetyDevice as FormArray; }
  get dropdownList() { return this.riskSpecificsData.riskSpecificsDropdownsList; }

  get safetyDeviceCategoryList() { return this.dropdownList.safetyDeviceCategoryList.sort(this.sortByLabel); }
  get includes() { return { control: this.fc['safetyDeviceCategory'], options: this.safetyDeviceCategoryList }; }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  onItemSelect(item: SelectItem) {
    this.multiSelectId = 'safetyDeviceCategory';

    if(+item.value == 1)
    {
      this.includes.control?.setValue(this.includes.options.filter(i => +i.value === 1));
      this.toggleSelectAll(true);
      let formValue = this.fc.safetyDevice as FormArray;
      formValue.clear();
      this.fg.setControl('safetyDevice', formValue);
    }
    else
    {
      const safetyDevice = this.riskSpecificsData.safetyDeviceForm(item);
      const formValue = this.fa;
      formValue.push(safetyDevice);
      this.fg.setControl('safetyDevice', formValue);

      const currentSelections = this.includes.control?.value as any[];
      const currentSelectionsNoneRemoved = currentSelections.filter(x => x.value != 1);
      
      this.fc['safetyDeviceCategory']?.setValue(currentSelectionsNoneRemoved);
     
      //const currentSelections = this.includes.control?.value as any[];
      
      this.toggleSelectAll(currentSelectionsNoneRemoved.length < this.includes.options.length - 1);
    }

    // const safetyDevice = this.riskSpecificsData.safetyDeviceForm(item);
    // const formValue = this.fa;
    // formValue.push(safetyDevice);
    // this.fg.setControl('safetyDevice', formValue);

    //this.multiSelectId = 'safetyDeviceCategory';
    //this.adjustSelection(this.includes);
  }

  onSelectAll(items: SelectItem[]) {

    this.multiSelectId = 'safetyDeviceCategory';
    const values = this.selectAllTag.input.checked ? this.safetyDeviceCategoryList.filter(i => +i.value === 1) : this.safetyDeviceCategoryList.filter(i => +i.value !== 1);

    this.fc['safetyDeviceCategory']?.setValue(values);
    this.toggleSelectAll(this.selectAllTag.input.checked);

    const formValue = this.fc.safetyDevice as FormArray;
    formValue.clear();
    if(this.selectAllTag.input.checked)
    {
      const itemsNoneExcluded = items.filter(x => x.value != 1);
      itemsNoneExcluded.forEach(item => {
        const itemIndex = formValue.controls.findIndex(x => x.get('safetyDeviceCategoryId').value === item.value);
        if (itemIndex === -1) {
          formValue.push(this.riskSpecificsData.safetyDeviceForm(item));
        }
      });

    }
    this.fg.setControl('safetyDevice', formValue);
  }

  onItemDeSelect(item: SelectItem) {
    const formValue = this.fc.safetyDevice as FormArray;
    formValue.removeAt(formValue.controls.findIndex(x => x.get('safetyDeviceCategoryId').value === item.value));
    this.fg.setControl('safetyDevice', formValue);

    this.multiSelectId = 'safetyDeviceCategory';
    this.adjustSelection(this.includes);
  }

  onFilterChange(value, controlName) {
    this.multiSelectId = controlName;
    this.selectAllTag.input.parentElement.hidden = (value != '');
  }

  onDropDownClose(controlName) {
    this.multiSelectId = controlName;
    if (this.selectAllTag.input !== null) {
      this.selectAllTag.input.parentElement.hidden = false;
    }
  }

  private sortByLabel(a: SelectItem, b: SelectItem) {
    if (a.label === 'None' || b.label === 'None') { return 1; }
    return a.label < b.label ? -1 : a.label > b.label ? 1 : 0;
  }

  get hasMaintenanceQuestions() { return this.submissionData.riskDetail?.vehicles?.length > 5; }

  get selectAllTag() {
    const id = this.multiSelectId;
    const selectAllCheckbox = <HTMLInputElement>document.querySelector(`ng-multiselect-dropdown#${id} li.multiselect-item-checkbox input`);
    const selectAllCheckboxLabel = <HTMLInputElement>document.querySelector(`ng-multiselect-dropdown#${id} li.multiselect-item-checkbox div`);
    return {
      input: selectAllCheckbox,
      label: selectAllCheckboxLabel
    };
  }
  
  private adjustSelection(includes: any) {
    const currentSelections = includes.control?.value as any[];
    const noneSelected = currentSelections.find(cs => +cs.value === 1);
    if (noneSelected) {
      includes.control?.setValue(includes.options.filter(i => +i.value === 1));
      this.toggleSelectAll(true);
      let formValue = this.fc.safetyDevice as FormArray;
      formValue.clear();
      this.fg.setControl('safetyDevice', formValue);
    } else {
      this.toggleSelectAll(currentSelections.length < includes.options.length - 1);
    }
  }

  private toggleSelectAll(value) {
    this.selectAllTag.input.checked = !value;
    this.selectAllTag.label.innerHTML = !value ? 'Unselect All' : 'Select All';
  }
}
