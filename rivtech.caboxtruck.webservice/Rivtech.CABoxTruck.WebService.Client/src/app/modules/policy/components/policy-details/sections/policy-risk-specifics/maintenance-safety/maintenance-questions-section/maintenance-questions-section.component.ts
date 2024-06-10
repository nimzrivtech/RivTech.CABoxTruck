import { Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '../../../../../../../../shared/base-component';
import { MaintenanceQuestionConstants } from '../../../../../../../../shared/constants/risk-specifics/maintenance-safety.label.constants';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { SelectItem } from '../../../../../../../../shared/models/dynamic/select-item';
import FormUtils from '../../../../../../../../shared/utilities/form.utils';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
@Component({
  selector: 'app-maintenance-questions-section',
  templateUrl: './maintenance-questions-section.component.html',
  styleUrls: ['./maintenance-questions-section.component.scss']
})

export class MaintenanceQuestionsSectionComponent extends BaseComponent implements OnInit {
  categoryList: any;
  hideMe: boolean = false;
  public MaintenanceQuestionConstants = MaintenanceQuestionConstants;
  multiSelectId: string = '';

  constructor(
    public riskSpecificsData: RiskSpecificsData,
    public router: Router,
    public submissionData: SubmissionData
  ) {
    super();
  }

  get fg() { return this.riskSpecificsData.riskSpecificsForms.maintenanceForm; }
  get fc() { return this.fg.controls; }
  get fn() { return 'maintenanceForm'; }
  get dropdownList() { return this.riskSpecificsData.riskSpecificsDropdownsList; }

  ngOnInit() {
    if (this.submissionData?.riskDetail?.maintenanceSafety) {
      this.fg.markAllAsTouched();
    }
    this.switchValidator(this.hasMaintenanceQuestions, 'safetyMeetings');
  }

  get companyPracticeList() { return this.dropdownList.companyPracticeList.sort(this.sortByLabel); }
  get garagingTypeList() { return this.dropdownList.garagingTypeList.sort(this.sortByLabel); }
  get hasMaintenanceQuestions() { return this.submissionData.riskDetail?.vehicles?.length > 5; }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  private sortByLabel(a: SelectItem, b: SelectItem) {
    return a.label < b.label ? -1 : a.label > b.label ? 1 : 0;
  }

  switchValidator(isChecked: boolean, targetControlName: string) {
    // if (isChecked) {
    //   FormUtils.addRequiredValidator(this.fg, targetControlName);
    // } else {
    //   FormUtils.clearValidator(this.fg, targetControlName);
    // }
  }

  onFilterChange(value, controlName: string) {
    this.multiSelectId = controlName;
    this.selectAllTag.input.parentElement.hidden = (value != '');
  }

  onDropDownClose(controlName: string) {
    this.multiSelectId = controlName;
    this.selectAllTag.input.parentElement.hidden = false;
  }

  get dropdownListSelection() {
    switch (this.multiSelectId) {
      case 'garagingType':
        return this.garagingTypeList;
      case 'vehicleMaintenance':
        return this.dropdownList.vehicleMaintenanceList;
      case 'companyPractice':
        return this.companyPracticeList;
      default:
        break;
    }
  }

  get selectAllTag() {
    const id = this.multiSelectId;
    const selectAllCheckbox = <HTMLInputElement>document.querySelector(`ng-multiselect-dropdown#${id} li.multiselect-item-checkbox input`);
    const selectAllCheckboxLabel = <HTMLInputElement>document.querySelector(`ng-multiselect-dropdown#${id} li.multiselect-item-checkbox div`);
    return {
      input: selectAllCheckbox,
      label: selectAllCheckboxLabel
    };
  }
}
