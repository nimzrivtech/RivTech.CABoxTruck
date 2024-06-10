import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { RiskSpecificsData } from '../../../../../../data/risk-specifics.data';
import { Router } from '@angular/router';
import { Output } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { SelectItem } from '../../../../../../../../shared/models/dynamic/select-item';
import { BaseComponent } from '../../../../../../../../shared/base-component';
import { DriverInfoPageService } from '../../../../../../../../core/services/submission/risk-specifics/driver-info-page.service';
import { ToggleCollapseComponent } from '../../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { DriverInfoDto } from '../../../../../../../../shared/models/submission/riskspecifics/driver-info.dto';
import { IMvrPullingFrequencyDto } from '../../../../../../../../shared/models/submission/riskspecifics/mvr-pulling-frequency.dto';
import { DriverInfoData } from '../../../../../../data/riskspecifics/driver-info.data';
import { SubmissionData } from '../../../../../../data/submission.data';
import { BackgroundCheckOptionDto } from '../../../../../../../../shared/models/submission/riskspecifics/background-check-option.dto';
import { DriverTrainingOptionDto } from '../../../../../../../../shared/models/submission/riskspecifics/driver-training-option.dto';
import { DriverHiringCriteriaData } from '../../../../../../data/riskspecifics/driver-hiring-criteria.data';
import { DriverHiringCriteriaDto } from '../../../../../../../../shared/models/submission/riskspecifics/driver-hiring-criteria.dto';

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
      this.fg.markAllAsTouched();
    } else {
      this.driverHiringCriteriaData.initiateFormGroup(new DriverHiringCriteriaDto());
      // if retrieved from submission list but has no saved values yet, then trigger validations
      if (this.submissionData.validationList.driverInfo === false && !this.driverHiringCriteriaData.isInitial) { this.fg.markAllAsTouched(); }
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

  onItemSelect(item: BackgroundCheckOptionDto): void {
    this.multiSelectId = 'backgroundCheckIncludes';
    
    if(+item.id == 1)
    {
      this.includes.control?.setValue(this.includes.options.filter(i => +i.id === 1));
      this.toggleSelectAll(true);
    }
    else
    {
      const currentSelections = this.includes.control?.value as any[];
      
      const currentSelectionsNoneRemoved = currentSelections.filter(x => x.id != 1);
      
      this.includes.control?.setValue(currentSelectionsNoneRemoved);
      
      this.toggleSelectAll(currentSelectionsNoneRemoved.length < this.includes.options.length - 1);
    }

    //this.adjustSelection(this.includes);
  }

  onItemDeSelect(): void {
    this.multiSelectId = 'backgroundCheckIncludes';
    this.adjustSelection(this.includes);
  }

  onSelectAll() {
    this.multiSelectId = 'backgroundCheckIncludes';
    const values = this.selectAllTag.input.checked ? this.backgroundCheckOptions.filter(i => +i.id === 1) : this.backgroundCheckOptions.filter(i => +i.id !== 1);
    this.fc['backGroundCheckIncludes']?.setValue(values);
    this.toggleSelectAll(this.selectAllTag.input.checked);
  }

  onItemSelect2(item: BackgroundCheckOptionDto): void {
    this.multiSelectId = 'driverTrainingIncludes';

    if(+item.id == 1)
    {
      this.includes2.control?.setValue(this.includes2.options.filter(i => +i.id === 1));
      this.toggleSelectAll(true);
    }
    else
    {
      const currentSelections = this.includes2.control?.value as any[];
      
      const currentSelectionsNoneRemoved = currentSelections.filter(x => x.id != 1);
      
      this.includes2.control?.setValue(currentSelectionsNoneRemoved);
      
      this.toggleSelectAll(currentSelectionsNoneRemoved.length < this.includes2.options.length - 1);
    }
    //this.adjustSelection(this.includes2);
  }

  onItemDeSelect2(): void {
    this.multiSelectId = 'driverTrainingIncludes';
    this.adjustSelection(this.includes2);
  }

  onSelectAll2() {
    this.multiSelectId = 'driverTrainingIncludes';
    const values = this.selectAllTag.input.checked ? this.driverTrainingOptions.filter(i => +i.id === 1) : this.driverTrainingOptions.filter(i => +i.id !== 1);
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
