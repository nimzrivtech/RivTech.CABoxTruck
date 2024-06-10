
import {Injectable} from '@angular/core';
import {FormGroup} from '@angular/forms';

@Injectable()
export class GeneralInformationData {
  agencyList: any[] = [
    {label: 'American Business Insurance Services', value: '1'},
    {label: 'American Specialty Insurance Group, Inc', value: '2'},
    {label: 'Preferred Risk Agency', value: '3'},
    {label: 'Risk Partners Inc.', value: '4'},
    {label: 'Miriam L Morency New America Insurance', value: '5'},
    {label: 'Onyx Transportation Services', value: '6'},
  ];

  subAgencyList: any[] = [
    {label: 'RivTech', value: '1'},
    {label: 'RivTech2', value: '1'},
  ];

  creditedOfficeList: any[] = [
    {label: 'Applied', value: '1'},
    {label: 'Rivington', value: '2'},
    {label: 'VALE', value: '3'},
  ];

  
  constructor() { }

  initiateFormValues(fb: FormGroup) {
  }

  retrieveDropDownValues() {
    //API call
  }
}
