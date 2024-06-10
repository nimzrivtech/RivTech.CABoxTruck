import { Component, OnInit } from '@angular/core';
import { from } from 'rxjs';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { yearEstablishedInvalidYear, firstYearUnderCurrentInvalidYear } from '../../../../../../helpers/invalidYear.validator'
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { GeneralInformationData } from '../../../../../../data/applicant/general-information.data';

@Component({
  selector: 'app-general-information-section',
  templateUrl: './general-information-section.component.html',
  styleUrls: ['./general-information-section.component.scss']
})
export class GeneralInformationSectionComponent implements OnInit {
  applicantGeneralInformation: FormGroup;
  submitted = false;

  year = (new Date()).getFullYear();

  //DropdDown settings
  dropdownList = [];
  selectedItems = [];
  dropdownSettings: IDropdownSettings = {};

  selectedFilings: string[];
  hasRequiredFilings: boolean = true;
  hasApplicantRequire: boolean = false;
  hasAnySubsidiaries: boolean = false;
  hasOwnedOtherCompany: boolean = false;
  hasIncludedInsurance: boolean = false;
  hasDoesApplicantAllowSwitch: boolean = false;
  hasApplicantPreviouslyAllowSwitch: boolean = false;
  hasMailAndAddressSame: boolean = false;
  hasGaragingAndBusinessAddressSame: boolean = false;
  hasDotNumber: boolean = false;
  fillingValidation: any[] = [
    { checkIfBMC91x: false }
  ]

  constructor(private formBuilder: FormBuilder, public router: Router, public genInfoData: GeneralInformationData) { }

  ngOnInit() {
    this.selectedItems = [
    ];
    this.dropdownSettings = {
      singleSelection: false,
      idField: 'item_id',
      textField: 'item_text',
      selectAllText: 'Select All',
      unSelectAllText: 'Unselect All',
      itemsShowLimit: 3,
      allowSearchFilter: true
    };


    this.applicantGeneralInformation = this.formBuilder.group({
      businessNameInput: ['', Validators.required],
      insuredDbaInput: ['', Validators.required],
      businessAddressInput: ['', Validators.required],
      businessZipCodeInput: ['', Validators.required],
      businessCityDropdown: ['', Validators.required],
      businessStateInput: ['', Validators.required],
      mailingAndBusinessAddressSwitch: [''],
      mailingAddressInput: ['', Validators.required],
      mailingZipCodeInput: ['', Validators.required],
      mailingCityDropdown: ['', Validators.required],
      mailingStateInput: ['', Validators.required],
      garagingAndBusinessAddressSwitch: [''],
      garagingAddressInput: ['', Validators.required],
      garagingZipCodeInput: ['', Validators.required],
      garagingCityDropdown: ['', Validators.required],
      garagingStateInput: ['', Validators.required],
      cityOfOperationsDropdown: ['', Validators.required],
      yearEstablishedInput: ['', Validators.required],
      yearBusinessInput: ['', Validators.required],
      firstYearUnderCurrentInput: ['', Validators.required],
      yearUnderCurrentInput: ['', Validators.required],
      accountCategoryDropdown: ['', Validators.required],
      mainUseDropdown: ['', Validators.required],
      businessTypeDropdown: ['', Validators.required],
      federalIdInput: ['', Validators.required],
      iccDocketNumberInput: [''],
      usdotInput: ['', Validators.required],
      dotNumberPastFiveYearsSwitch: [''],
      dotNumberInput: ['', Validators.required],
      pucNumberInput: ['', Validators.required],
      naicsCodeInput: [''],
      additionalNaicsCodeInput: [''],
      doesApplicantRequireSwitch: [''],
      areFillingsRequiredSwitch: [''],
      requiredFillingsDropdown: ['', Validators.required],
      businessPrincipalInput: ['', Validators.required],
      personelEmailInput: ['', [Validators.required, Validators.email]],
      personelPhoneExtensionInput: ['', Validators.required],
      anySubsidiariesCompaniesSwitch: [''],
      subsidiariesNameInput: ['', Validators.required],
      typeOfBusinessDropdown: ['', Validators.required],
      numVechSizeComp: ['', Validators.required],
      relationshipInput: ['', Validators.required],
      includedInsuranceSwitch: [''],
      ownedAnyOtherCompanyPastSwitch: [''],
      nameYearsOperationInput: ['', Validators.required],
      doesApplicantAllowSwitch: [''],
      hasApplicantPreviouslyAllowSwitch: ['']
    }, {
      validator: [yearEstablishedInvalidYear(this.year, 'yearEstablishedInput'), firstYearUnderCurrentInvalidYear(this.year, 'firstYearUnderCurrentInput')]
    });

    this.initialInput();
  }

  // convenience getter for easy access to form fields
  get f() { return this.applicantGeneralInformation.controls; }

  onSubmit() {
    this.submitted = true;

    // // stop here if form is invalid
    // if (this.applicantGeneralInformation.invalid) {
    //     return;
    // }
    //this.router.navigate(['/submissions/new/applicant/personnel']);
    this.router.navigate(['/submissions/new/applicant/additional-interest']);
    // display form values on success
  }

  onReset() {
    this.submitted = false;
    this.applicantGeneralInformation.reset();
  }

  modelZipcodeChanged(value, prefix) {
    if (value == '91362') {
      this.applicantGeneralInformation.controls[prefix + 'CityDropdown'].patchValue('1');
      this.applicantGeneralInformation.controls[prefix + 'StateInput'].patchValue('CA');
      this.applicantGeneralInformation.controls[prefix + 'AddressInput'].patchValue('32107 Lindero Canyon Rd, #120');
    } else if (value == '11201') {
      this.applicantGeneralInformation.controls[prefix + 'CityDropdown'].patchValue('2');
      this.applicantGeneralInformation.controls[prefix + 'StateInput'].patchValue('NY');
      this.applicantGeneralInformation.controls[prefix + 'AddressInput'].patchValue('1 Main');
    } else {
      this.applicantGeneralInformation.controls[prefix + 'CityDropdown'].patchValue(null);
      this.applicantGeneralInformation.controls[prefix + 'StateInput'].patchValue(null);
      this.applicantGeneralInformation.controls[prefix + 'AddressInput'].patchValue(null);
    }

    if (value) {
      this.clearValidators([prefix + 'ZipCodeInput']);
    } else {
      this.addValidators([prefix + 'ZipCodeInput']);
    }
  }

  modelYearChanged(value, prefix, controlName) {
    let year;
    if (value) {
      this.addValidators([prefix]);
      year = this.year;
      if (year >= value) {
        this.clearValidators([controlName]);
        year -= value;
      } else {
        year = null;
      }
    } else {
      this.clearValidators([prefix]);
    }
    this.applicantGeneralInformation.controls[prefix].patchValue(year);
  }

  isMailingBusinessSameSwitch(value) {
    if (value == 'on') {
      this.clearValidators(['mailingAddressInput, mailingZipCodeInput, mailingCityDropdown, mailingStateInput']);
    } else if (value == 'off') {
      this.addValidators(['mailingAddressInput, mailingZipCodeInput, mailingCityDropdown, mailingStateInput']);
    }
  }

  isGaragingBusinessSameSwitch(value) {
    if (value == 'on') {
      this.clearValidators(['garagingAddressInput, garagingZipCodeInput, garagingCityDropdown, garagingStateInput']);
    } else if (value == 'off') {
      this.addValidators(['garagingAddressInput, garagingZipCodeInput, garagingCityDropdown, garagingStateInput']);
    }
  }

  isPastFiveYearsSwitch(value) {
    if (value == 'on') {
      this.addValidators(['dotNumberInput']);
    } else if (value == 'off') {
      this.clearValidators(['dotNumberInput']);
    }
  }

  isRequiredFilings(value) {
    if (value == 'on') {
      this.addValidators(['requiredFillingsDropdown']);
    } else if (value == 'off') {
      this.clearValidators(['requiredFillingsDropdown']);
    }
  }

  hasRequiredFilingsChanged($event): void {
    this.selectedFilings = [];
  }

  requireFocus(value) {
    console.log(value);
  }

  onItemSelect(item: any) {
    if (this.selectedItems.filter(x => x.item_text == 'BMC-91x').length >= 1) {
      this.fillingValidation[0].checkIfBMC91x = true;
    }
    //console.log(item);
  }

  onItemDeSelect(item: any) {
    if (this.selectedItems.filter(x => x.item_text == 'BMC-91x').length < 1) {
      this.fillingValidation[0].checkIfBMC91x = false;
    }
    //console.log(item);
  }

  onItemDeSelectAll(item: any) {
    if (item.filter(x => x.item_text == 'BMC-91x').length < 1) {
      this.fillingValidation[0].checkIfBMC91x = false;
    }
    //console.log(item);
  }

  onSelectAll(items: any) {
    if (items.filter(x => x.item_text == 'BMC-91x').length >= 1) {
      this.fillingValidation[0].checkIfBMC91x = true;
    }
    console.log(items.filter(x => x.item_text == 'BMC-91x').length);
  }

  isAnySubsidiariesCompaniesSwitch(value) {
    if (value == 'on') {
      this.addValidators(['subsidiariesNameInput', 'typeOfBusinessDropdown', 'numVechSizeComp', 'relationshipInput']);
    } else if (value == 'off') {
      this.clearValidators(['subsidiariesNameInput', 'typeOfBusinessDropdown', 'numVechSizeComp', 'relationshipInput']);
    }
  }

  isOwnedAnyOtherCompanyPastSwitch(value) {
    if (value == 'on') {
      this.addValidators(['nameYearsOperationInput']);
    } else if (value == 'off') {
      this.clearValidators(['nameYearsOperationInput']);
    }
  }

  initialInput() {
    this.clearValidators(['subsidiariesNameInput', 'typeOfBusinessDropdown', 'numVechSizeComp', 'relationshipInput', 'nameYearsOperationInput']);
  }

  modelNumberVehicleSizeCompanyChanged(value) {
    if (value) {
      this.applicantGeneralInformation.controls['numVechSizeComp'].clearValidators();
      this.applicantGeneralInformation.controls['numVechSizeComp'].updateValueAndValidity();
    } else {
      this.applicantGeneralInformation.controls['numVechSizeComp'].setValidators([Validators.required]);
      this.applicantGeneralInformation.controls['numVechSizeComp'].updateValueAndValidity();
    }
  }

  clearValidators(controlName: string[]) {
    controlName.forEach(eachControl => {
      this.applicantGeneralInformation.controls[eachControl].clearValidators();
      this.applicantGeneralInformation.controls[eachControl].updateValueAndValidity();
    });
  }

  addValidators(controlName: string[]) {
    controlName.forEach(eachControl => {
      this.applicantGeneralInformation.controls[eachControl].setValidators([Validators.required]);
      this.applicantGeneralInformation.controls[eachControl].updateValueAndValidity();
    });
  }
}
