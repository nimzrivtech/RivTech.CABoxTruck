
import {Injectable} from '@angular/core';
import {FormGroup} from '@angular/forms';

@Injectable()
export class GeneralInformationData {
  // accountCategoryList: any[] = [
  //   {label: 'Building Materials & Hardware Stores', value: '1'},
  //   {label: 'Courier', value: '2'},
  //   {label: 'Food & Grocery Stores', value: '3'},
  //   {label: 'Furniture & Home Furnishing Stores', value: '4'},
  //   {label: 'Manufacturing', value: '5'},
  //   {label: 'Newspaper & Mail Delivery', value: '6'},
  //   {label: 'Other For-Hire Trucking Operations', value: '7'},
  //   {label: 'Other Retail Trade Operations', value: '8'},
  //   {label: 'Refrigerated Goods', value: '9'},
  //   {label: 'Retail Operations (Renewals Only)', value: '10'},
  //   {label: 'Wholesale Manufacturing (Renewals Only)', value: '11'},
  //   {label: 'Wholesale Trade', value: '12'},
  //   {label: '#N/A', value: '13'},
  // ];
  //
  // mainUseList: any[] = [
  //   {label: 'Service', value: '1'},
  //   {label: 'Heavy/Special', value: '2'},
  //   {label: 'Logging', value: '3'},
  //   {label: 'Non-Business', value: '4'},
  //   {label: 'Pizza/Prepared Food', value: '5'},
  //   {label: 'Courier', value: '6'},
  //   {label: 'Funeral', value: '7'},
  //   {label: 'Child Care', value: '8'},
  //   {label: 'Church', value: '9'},
  //   {label: 'Social Service Passenger Transport', value: '10'},
  //   {label: 'Livery', value: '11'},
  //   {label: 'Commercial', value: '12'},
  //   {label: 'Repossession', value: '13'},
  //   {label: 'Coal Hauling', value: '14'},
  //   {label: 'Towing 24/7', value: '15'},
  // ];
  //
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
  //
  // cityList: any[] = [
  //
  //   {label: 'Brooklyn', value: '1'},
  //   {label: 'Chula Vista', value: '2'},
  //   {label: 'Newark', value: '3'},
  //   {label: 'New York City', value: '4'},
  //   {label: 'Westlake Village', value: '5'},
  // ];

  constructor() { }

  initiateFormValues(fb: FormGroup) {
  }

  retrieveDropDownValues() {
    //API call
  }
}
