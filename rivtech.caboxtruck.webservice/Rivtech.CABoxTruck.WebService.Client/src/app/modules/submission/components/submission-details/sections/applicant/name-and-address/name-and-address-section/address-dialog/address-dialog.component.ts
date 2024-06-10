import { Component, OnInit } from '@angular/core';
import { AddressDTO } from '../../../../../../../../../shared/models/submission/addressDto';
import { EntityAddressDTO } from '../../../../../../../../../shared/models/submission/entityAddressDto';
import { BsModalRef } from 'ngx-bootstrap';
import { ApplicantData } from '../../../../../../../../../modules/submission/data/applicant/applicant.data';
import { AddressType, LvAddressTypeList } from '../../../../../../../../../shared/constants/name-and-address.options.constants';
import Utils from '../../../../../../../../../shared/utilities/utils';
import { ZipcodeService } from '../../../../../../../../../core/services/generics/zipcode.service';
import { take } from 'rxjs/operators';
import NotifUtils from '../../../../../../../../../shared/utilities/notif-utils';
import { Subject } from 'rxjs/Rx';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SubmissionData } from '@app/modules/submission/data/submission.data';

@Component({
  selector: 'app-address-dialog',
  templateUrl: './address-dialog.component.html',
  styleUrls: ['./address-dialog.component.scss']
})
export class AddressDialogComponent implements OnInit {
  modalTitle: string = 'Add Entity Address';
  modalButton: string = 'Add Entity Address';
  editItem: any;
  isEdit: boolean = false;
  isDisable: boolean = false;
  isAddNewAddress: boolean = false;

  unavailableTypes = [AddressType.Billing, AddressType.Business, AddressType.Mailing];

  // model
  addressType: any;
  address: any;
  completeAddress: string;

  addressForm: FormGroup;

  addessTypeList: any[];
  addressList: any[] = [];
  cityList: any[] = [];

  public onClose: Subject<boolean>;
  disableCity: boolean;
  constructor(private applicantData: ApplicantData,
    private submissionData: SubmissionData,
    public modalRef: BsModalRef,
    private fb: FormBuilder,
    private zipcodeService: ZipcodeService) {
   }

  ngOnInit() {
    this.addressForm = this.fb.group({
      street: [null, Validators.required],
      city: [null, Validators.required],
      state: [null, Validators.required],
      zipcode: [null, Validators.required],
    });

    this.onClose = new Subject();
    this.addressList = this.applicantData.addressList.map((x) => ({
      item: x,
      value: x.address.streetAddress1 + ', ' + x.address.city + ', ' +  x.address.state + ', ' +  x.address.zipCode
    }));

    // Distinct
    this.addressList = this.addressList.filter((item, i, arr) => arr.findIndex((t) => t.value === item.value) === i);
    if (!this.isEdit) {
      this.addessTypeList = LvAddressTypeList.filter(x => this.unavailableTypes.indexOf(x.id) === -1);
      this.addressType = this.addessTypeList[0];
      this.address = this.addressList[0].item;
      this.completeAddress = this.addressList[0].value;
    } else {
      this.completeAddress = this.editItem.address.streetAddress1 + ', ' + this.editItem.address.city + ', ' +  this.editItem.address.state + ', ' +  this.editItem.address.zipCode;
      this.addessTypeList = LvAddressTypeList;
      this.addressType = LvAddressTypeList.find(x => x.id === this.editItem.addressTypeId);
      this.address = this.addressList.find(x => x.value === this.completeAddress).item;
    }

    if (this.editItem?.addressTypeId == AddressType.Billing) {
      this.applicantData.applicantForms.billingInfoForm.controls['contactName'].patchValue(this.editItem.contactName);
      this.applicantData.applicantForms.billingInfoForm.controls['phone'].patchValue(this.editItem.phone);
      this.applicantData.applicantForms.billingInfoForm.controls['email'].patchValue(this.editItem.email);
    }

    if (!this.isEdit) {
      this.address = null;
    }
    this.resetZipCode();
  }

  get f() { return this.addressForm.controls; }
  get ff() { return this.applicantData.applicantForms.billingInfoForm.controls; }

  cancel() {
    if (this.editItem?.addressTypeId == AddressType.Billing) {
      this.applicantData.applicantForms.billingInfoForm.controls['contactName'].patchValue(this.editItem.contactName);
      this.applicantData.applicantForms.billingInfoForm.controls['phone'].patchValue(this.editItem.phone);
      this.applicantData.applicantForms.billingInfoForm.controls['email'].patchValue(this.editItem.email);
    }
    this.modalRef.hide()
  }

  setTooltip(item) {
    this.completeAddress = '';
    if (item != null) {
      this.completeAddress = item.address.streetAddress1 + ', ' + item.address.city + ', ' +  item.address.state + ', ' +  item.address.zipCode;
    }
  }

  get isExist() {
    if (this.address == null) {
      return true;
    }

    if (this.addressType.id == AddressType.Billing) {
      return this.applicantData.addressList.find(x => x.addressTypeId === this.addressType.id
        && x.address.streetAddress1 === this.address.address?.streetAddress1
        && x.address.zipCode === this.address.address?.zipCode
        && x.address.city === this.address.address?.city 
        && x.contactName === this.applicantData.applicantForms.billingInfoForm.controls['contactName'].value 
        && x.email === this.applicantData.applicantForms.billingInfoForm.controls['email'].value 
        && x.phone === this.applicantData.applicantForms.billingInfoForm.controls['phone'].value);
    }

    return this.applicantData.addressList.find(x => x.addressTypeId === this.addressType.id
      && x.address.streetAddress1 === this.address.address?.streetAddress1
      && x.address.zipCode === this.address.address?.zipCode
      && x.address.city === this.address.address?.city);
  }

  saveAddress() {
    let businessUpdate = false;
    let exist = this.applicantData.addressList.find(x => x.addressTypeId === this.addressType.id
      && x.address.streetAddress1 === this.address.address.streetAddress1
      && x.address.zipCode === this.address.address.zipCode
      && x.address.city === this.address.address.city);
    
    if (this.addressType.id == AddressType.Billing) {
      exist = this.applicantData.addressList.find(x => x.addressTypeId === this.addressType.id
        && x.address.streetAddress1 === this.address.address.streetAddress1
        && x.address.zipCode === this.address.address.zipCode
        && x.address.city === this.address.address.city 
        && x.contactName === this.applicantData.applicantForms.billingInfoForm.controls['contactName'].value 
        && x.email === this.applicantData.applicantForms.billingInfoForm.controls['email'].value 
        && x.phone === this.applicantData.applicantForms.billingInfoForm.controls['phone'].value);
    }

    if (!this.isEdit) {
      if (exist) {
        NotifUtils.showError('Address already exist');
        return;
      }
      let newAddress = new EntityAddressDTO({
        addressTypeId: this.addressType.id,
        addressType: this.addressType.value,
        address: new AddressDTO({
          streetAddress1: this.address.address.streetAddress1,
          zipCode: this.address.address.zipCode,
          state: this.address.address.state,
          city: this.address.address.city,
          isMainGarage: false })
        });
      if (this.addressType.id == AddressType.Billing) {
        newAddress.contactName = this.applicantData.applicantForms.billingInfoForm.controls['contactName'].value;
        newAddress.email = this.applicantData.applicantForms.billingInfoForm.controls['email'].value;
        newAddress.phone = this.applicantData.applicantForms.billingInfoForm.controls['phone'].value;
      }

      this.applicantData.addressList.push(newAddress);
    } else {
      if (this.editItem.address.streetAddress1 === this.address.address.streetAddress1 && this.editItem.address.city === this.address.address.city
        && this.editItem.address.state === this.address.address.state && this.editItem.address.zipCode === this.address.address.zipCode && this.editItem.addressType != 'Billing') {
          this.modalRef.hide();
          return;
      }

      if (exist) {
        NotifUtils.showError('Address already exist');
        return;
      }
      // Update addresses table
      this.editItem.address.streetAddress1 = this.address.address.streetAddress1;
      this.editItem.address.city = this.address.address.city;
      this.editItem.address.state = this.address.address.state;
      this.editItem.address.zipCode = this.address.address.zipCode;

      if (this.addressType.id == AddressType.Billing) {
        this.editItem.contactName = this.applicantData.applicantForms.billingInfoForm.controls['contactName'].value;
        this.editItem.email = this.applicantData.applicantForms.billingInfoForm.controls['email'].value;
        this.editItem.phone = this.applicantData.applicantForms.billingInfoForm.controls['phone'].value;
      }

      // Patch if Billing is edited
      if (this.addressType.id === AddressType.Business) {
        this.applicantData.isInitialLoad = true;
        this.applicantData.cityList = [];
        this.applicantData.applicantForms.nameAndAddressForm.controls['businessAddressInput'].patchValue(this.address.address.streetAddress1);
        this.applicantData.applicantForms.nameAndAddressForm.controls['businessZipCodeInput'].patchValue(this.address.address.zipCode.toString());
        this.applicantData.applicantForms.nameAndAddressForm.controls['businessStateInput'].patchValue(this.address.address.state);
        this.applicantData.applicantForms.nameAndAddressForm.controls['businessCityDropdown'].patchValue(this.address.address.city);
        businessUpdate = true;
      }
    }
    this.onClose.next(businessUpdate);
    this.applicantData.sortAddressList();
    this.applicantData.setPage(1, this.applicantData.addressList, !this.isEdit);
    this.modalRef.hide();
  }

  addNewAddress() {
    this.addressForm.controls['street'].patchValue(null);
    this.resetZipCode();
    this.showSubForm(true);
    this.disableCity = false;
  }

  setCityStateZipCode() {
    const zipCode = this.addressForm.controls['zipcode'].value;
    this.cityList = [];
    if (zipCode !== '') {
      // Set City dromdownlist values
      Utils.blockUI();
      this.zipcodeService.getZipCodes(zipCode).pipe(take(1)).subscribe(data => {
        // Map to dropdownList
        this.cityList = data.map(x => x.city);
        if (this.cityList.length > 0) {
          this.addressForm.controls['state'].patchValue(data[0].stateCode);
          if (this.cityList.length === 1) {
            this.disableCity = true;
            this.addressForm.controls['city'].patchValue(data[0].city);
          } else {
            this.addressForm.controls['city'].reset();
            this.disableCity = false;
          }
          Utils.unblockUI();
        } else {
          Utils.unblockUI();
          NotifUtils.showError('Zip code not found, contact Underwriting');
          this.resetZipCode();
        }
      });
    } else {
      this.resetZipCode();
    }
  }

  resetZipCode() {
    this.disableCity = false;
    this.cityList = [];
    this.addressForm.controls['city'].patchValue(null);
    this.addressForm.controls['state'].patchValue(null);
    this.addressForm.controls['zipcode'].patchValue(null);
    this.addressForm.markAsTouched();
  }

  saveNewAddress() {
    this.completeAddress = this.addressForm.controls['street'].value + ', ' + this.addressForm.controls['city'].value + ', ' +  this.addressForm.controls['state'].value + ', ' +  this.addressForm.controls['zipcode'].value;
    if (this.addressList.find(x => x.value === this.completeAddress)) {
      // NotifUtils.showError('Address already exist');
      this.showSubForm(false);
      return;
    }

    this.addressList.unshift({
      item: new EntityAddressDTO({
        addressTypeId: this.addressType.id,
        addressType: this.addressType.value,
        address: new AddressDTO({
          streetAddress1: this.addressForm.controls['street'].value,
          zipCode: this.addressForm.controls['zipcode'].value,
          state: this.addressForm.controls['state'].value,
          city: this.addressForm.controls['city'].value,
          isMainGarage: false })
        }),
      value: this.completeAddress
    });

    this.address = this.addressList[0].item;
    this.addressForm.controls['street'].reset();
    this.showSubForm(false);
  }

  cancelNewAddress() {
    this.addressForm.controls['street'].reset();
    this.resetZipCode();
    this.showSubForm(false);
  }

  showSubForm(value) {
    this.isDisable = value;
    this.isAddNewAddress = value;
  }
}
