import { AfterViewInit, Component, OnInit } from '@angular/core';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { ApplicantData } from '../../../../../../../../modules/submission/data/applicant/applicant.data';
import { FilingsInformationDto } from '../../../../../../../../shared/models/submission/applicant/FilingsInformationDto';
import FormUtils from '../../../../../../../../shared/utilities/form.utils';
import { ApplicantLabelsConstants } from '../../../../../../../../shared/constants/applicant.labels.constants';
import { SelectItem } from '../../../../../../../../shared/models/dynamic/select-item';

@Component({
  selector: 'app-filings-information-section',
  templateUrl: './filings-information-section.component.html',
  styleUrls: ['./filings-information-section.component.scss']
})
export class FilingsInformationSectionComponent implements OnInit, AfterViewInit {
  hideMe: boolean = false;
  riskDetailId: string = '';
  LabelMessage = ApplicantLabelsConstants;
  dropdownSettings: IDropdownSettings = {};
  selectedItems = [];

  constructor(
    public submissionData: SubmissionData,
    public applicantData: ApplicantData,
  ) { }

  ngAfterViewInit(): void {
    this.multiSelectDropdownUI();
  }

  get form() {
    return this.applicantData.applicantForms.filingsInformationForm;
  }

  get f() { return this.form.controls; }
  get formControl() { return this.form.controls; }
  get formName() { return 'filingsInformationForm'; }
  get filingsList() { return this.applicantData.filingsList.sort(this.sortByLabel); }
  get isMotorCarrierNumberApplicable() { return this.submissionData.isNewFeatureApplicableByQuoteDate('S38MotorCarrierNumber'); }
  ngOnInit() {
    this.riskDetailId = this.submissionData.riskDetail?.id;
    if (this.riskDetailId) {
      this.loadFilingsInformation();
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

    this.setFieldInputReset();
  }

  loadFilingsInformation() {
    let filingsInfo = this.submissionData.riskDetail.filingsInformation;
    if (filingsInfo == null || filingsInfo.isARatedCarrierRequired == null) {
      filingsInfo = new FilingsInformationDto();
    }
    this.form.patchValue(filingsInfo);
    if (filingsInfo.isFilingRequired) {
      this.selectedItems = this.applicantData.filingsList.filter(fl => filingsInfo.requiredFilingIds.includes(+fl.value));
    }
    this.requiredSwitchChanged(filingsInfo.isFilingRequired, 'requiredFilingsList');
    this.requiredSwitchChanged(filingsInfo.isDifferentUSDOTNumberLastFiveYears, 'dotNumberLastFiveYearsDetail');
    this.switchValidator(filingsInfo.isInsuredHasMotorCarrierNo, 'mcNumber');
  }

  requiredSwitchChanged(isChecked: boolean, targetControlName: string) {
    // if (isChecked) {
    //   FormUtils.addRequiredValidator(this.form, targetControlName);
    // } else {
    //   FormUtils.clearValidator(this.form, targetControlName);
    // }
  }

  private sortByLabel(a: SelectItem, b: SelectItem) {
    const aLabel = a.label?.toLowerCase();
    const bLabel = b.label?.toLowerCase();
    return aLabel < bLabel ? -1 : aLabel > bLabel ? 1 : 0;
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  onItemSelect(): void {
    this.multiSelectDropdownUI();
  }

  onSelectAll(): void {
    const multiSelectDropDownEl = document.querySelector('ng-multiselect-dropdown#requiredFilingsList').querySelector('span.dropdown-btn').lastElementChild;
    multiSelectDropDownEl.setAttribute('style', 'padding:0px;float:right;');
    multiSelectDropDownEl?.prepend((document.createElement('br')));
    if (multiSelectDropDownEl?.querySelectorAll('br').length > 1) {
      multiSelectDropDownEl?.querySelectorAll('br')[0].remove();
    }
  }

  onDeSelectAll(): void {
    const multiSelectDropDownEl = document.querySelector('ng-multiselect-dropdown#requiredFilingsList').querySelector('span.dropdown-btn').lastElementChild;
    if (multiSelectDropDownEl?.querySelectorAll('br').length > 0) {
      multiSelectDropDownEl?.querySelectorAll('br')[0].remove();
    }
  }

  onFilterChange(value) {
    this.selectAllTag.input.parentElement.hidden = (value != '');
  }

  onDropDownClose() {
    this.selectAllTag.input.parentElement.hidden = false;
  }

  multiSelectDropdownUI(): void {
    const multiSelectDropDownEl = document.querySelector('ng-multiselect-dropdown#requiredFilingsList')?.querySelector('span.dropdown-btn')?.lastElementChild;
    multiSelectDropDownEl?.setAttribute('style', 'padding:0px;float:right;');
    if (this.selectedItems.length > 3) {
      multiSelectDropDownEl?.prepend((document.createElement('br')));
    }
    if (multiSelectDropDownEl?.querySelectorAll('br').length > 1) {
      multiSelectDropDownEl?.querySelectorAll('br')[0].remove();
    }
  }

  get selectAllTag() {
    const id = 'requiredFilingsList';
    const selectAllCheckbox = <HTMLInputElement>document.querySelector(`ng-multiselect-dropdown#${id} li.multiselect-item-checkbox input`);
    const selectAllCheckboxLabel = <HTMLInputElement>document.querySelector(`ng-multiselect-dropdown#${id} li.multiselect-item-checkbox div`);
    return {
      input: selectAllCheckbox,
      label: selectAllCheckboxLabel
    };
  }

  private setFieldInputReset() {
    this.formControl['isInsuredHasMotorCarrierNo'].valueChanges.subscribe(value => {
      this.reset(value, 'mcNumber', '');
    });
  }

  reset(isChecked: boolean, targetControlName: string, defaultValue?: any) {
    if (!isChecked) {
      this.formControl[targetControlName].setValue(defaultValue);
    }
  }

  switchValidator(isChecked: boolean, targetControlName: string) {
    if (isChecked) {
      FormUtils.addRequiredValidator(this.form, targetControlName);
    } else {
      FormUtils.clearValidator(this.form, targetControlName);
    }
  }
}
