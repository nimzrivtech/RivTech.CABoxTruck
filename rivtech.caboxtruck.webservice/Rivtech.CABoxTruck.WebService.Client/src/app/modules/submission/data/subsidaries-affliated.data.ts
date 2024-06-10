
import { ApplicantSubsidiariesService } from '../services/applicant-subsidiaries.service';
import {Injectable} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import Utils from '../../../shared/utilities/utils';

@Injectable()
export class SubsidariesAffiliatedData {
  subsidiariesForm : FormGroup;
  businessTypeList: any[];
  
  constructor(public applicantSubsidiariesService: ApplicantSubsidiariesService, private fb: FormBuilder) {
   }

  initiateFormValues() {
    this.subsidiariesForm = this.fb.group({
      anySubsidiariesCompaniesSwitch:[''],
      subsidiariesNameInput: ['', Validators.required],
      typeOfBusinessDropdown: ['', Validators.required],
      numVechSizeComp: ['', Validators.required],
      relationshipInput:['', Validators.required],
      includedInsuranceSwitch:[''],
      ownedAnyOtherCompanyPastSwitch:[''],
      nameYearsOperationInput:['', Validators.required],
      doesApplicantAllowSwitch:[''],
      hasApplicantPreviouslyAllowSwitch:['']
    });
  }

  getBusinessTypes(){
    this.businessTypeList = [];
    this.applicantSubsidiariesService.businessTypesAll().subscribe(data => {
      this.businessTypeList = Utils.convertEnumerableListToSelectItemList(data);
    });
  }

  //Static Data
  // businessTypeList: any[] = [
  //   {label: 'Individual', value: '1'},
  //   {label: 'Sole Proprietorship', value: '2'},
  //   {label: 'Partnership', value: '3'},
  //   {label: 'Corporation', value: '4'},
  //   {label: 'LLC', value: '5'},
  //   {label: 'S-Corp', value: '6'},
  //   {label: 'Other', value: '7'},
  //   {label: 'Government Entity', value: '8'},
  //   {label: 'Joint Venture', value: '9'},
  // ];
}
