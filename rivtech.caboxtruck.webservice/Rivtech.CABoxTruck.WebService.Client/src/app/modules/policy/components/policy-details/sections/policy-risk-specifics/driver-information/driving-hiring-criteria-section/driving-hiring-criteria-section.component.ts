import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { BaseComponent } from '../../../../../../../../shared/base-component';
import { DriverInfoPageService } from '../../../../../../../../core/services/submission/risk-specifics/driver-info-page.service';
import { ToggleCollapseComponent } from '../../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { BackgroundCheckOptionDto } from '../../../../../../../../shared/models/submission/riskspecifics/background-check-option.dto';
import { DriverTrainingOptionDto } from '../../../../../../../../shared/models/submission/riskspecifics/driver-training-option.dto';
import { DriverHiringCriteriaDto } from '../../../../../../../../shared/models/submission/riskspecifics/driver-hiring-criteria.dto';
import { DriverHiringCriteriaData } from '../../../../../../../../modules/submission/data/riskspecifics/driver-hiring-criteria.data';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';

@Component({
  selector: 'app-driving-hiring-criteria-section',
  templateUrl: './driving-hiring-criteria-section-static.component.html',
  styleUrls: ['./driving-hiring-criteria-section.component.scss']
})
export class DrivingHiringCriteriaSectionComponent extends BaseComponent implements OnInit {
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;
  backgroundCheckOptions: BackgroundCheckOptionDto[];
  driverTrainingOptions: DriverTrainingOptionDto[];
  multiSelectId: string = '';

  constructor(public driverHiringCriteriaData: DriverHiringCriteriaData, public submissionData: SubmissionData,
    private driverInfoPageService: DriverInfoPageService) {
    super();
  }

  ngOnInit() {
    if (this.submissionData?.riskDetail?.driverInfo && this.submissionData?.riskDetail?.driverHiringCriteria) {
      this.driverHiringCriteriaData.initiateFormGroup(this.submissionData.riskDetail.driverHiringCriteria);
    } else {
      this.driverHiringCriteriaData.initiateFormGroup(new DriverHiringCriteriaDto());
    }

    this.driverInfoPageService.getDriverInfoPageOptions().subscribe(driverInfoPageOptions => {
      if (driverInfoPageOptions?.backgroundCheckOptions) {
        this.backgroundCheckOptions = driverInfoPageOptions.backgroundCheckOptions.sort(this.sortByDesc);
      }
      if (driverInfoPageOptions?.driverTrainingOptions) {
        this.driverTrainingOptions = driverInfoPageOptions.driverTrainingOptions.sort(this.sortByDesc);
      }
    });

    this.driverHiringCriteriaData.isInitial = false;
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.driverHiringCriteriaData.driverHiringCriteriaForm; }
  get includes() { return { control: this.fc['backGroundCheckIncludes'], options: this.backgroundCheckOptions }; }
  get includes2() { return { control: this.fc['driverTrainingIncludes'], options: this.driverTrainingOptions }; }

  private sortByDesc(a: BackgroundCheckOptionDto, b: BackgroundCheckOptionDto) {
    if (a.description === 'None' || b.description === 'None') { return 1; }
    return a.description < b.description ? -1 : a.description > b.description ? 1 : 0;
  }

  onItemSelect(): void {
    this.multiSelectId = 'backgroundCheckIncludes';
    this.adjustSelection(this.includes);
  }

  onItemDeSelect(): void {
    this.multiSelectId = 'backgroundCheckIncludes';
    this.adjustSelection(this.includes);
  }

  onSelectAll() {
    this.multiSelectId = 'backgroundCheckIncludes';
    const values = this.selectAllTag.input.checked ? [] : this.backgroundCheckOptions.filter(i => +i.id !== 1);
    this.fc['backGroundCheckIncludes']?.setValue(values);
    this.toggleSelectAll(this.selectAllTag.input.checked);
  }

  onItemSelect2(): void {
    this.multiSelectId = 'driverTrainingIncludes';
    this.adjustSelection(this.includes2);
  }

  onItemDeSelect2(): void {
    this.multiSelectId = 'driverTrainingIncludes';
    this.adjustSelection(this.includes2);
  }

  onSelectAll2() {
    this.multiSelectId = 'driverTrainingIncludes';
    const values = this.selectAllTag.input.checked ? [] : this.driverTrainingOptions.filter(i => +i.id !== 1);
    this.fc['driverTrainingIncludes']?.setValue(values);
    this.toggleSelectAll(this.selectAllTag.input.checked);
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
    const noneSelected = currentSelections.find(cs => +cs.id === 1);
    if (noneSelected) {
      includes.control?.setValue(includes.options.filter(i => +i.id === 1));
      this.toggleSelectAll(true);
    } else {
      this.toggleSelectAll(currentSelections.length < includes.options.length - 1);
    }
  }

  private toggleSelectAll(value) {
    this.selectAllTag.input.checked = !value;
    this.selectAllTag.label.innerHTML = !value ? 'Unselect All' : 'Select All';
  }
}
