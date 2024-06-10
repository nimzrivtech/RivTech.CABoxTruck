import { Component, OnInit, EventEmitter } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { IAngularMyDpOptions } from 'angular-mydatepicker';

import { GenericAddressData } from './data/generic-address.data';
import { GenericAddressLabelConstants } from '../../../shared/constants/generic-address.label.constants';
import { ZipCodeData } from '../../../modules/management/data/zipcode.data';
import { AddressDTO, EntityAddressDTO } from '../../../shared/models/management/generic-address.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-generic-address',
  templateUrl: './generic-address.component.html',
  styleUrls: ['./generic-address.component.scss']
})

export class GenericAddressComponent implements OnInit {
  GenericAddressLabelConstants = GenericAddressLabelConstants;
  effectiveDateOption: IAngularMyDpOptions;
  expirationDateOption: IAngularMyDpOptions;

  public event: EventEmitter<any> = new EventEmitter<any>();

  modalTitle: string;
  buttonText: string;
  isAdd: boolean = true;
  entityAddressModel: EntityAddressDTO;

  constructor(
    private modalRef: BsModalRef,
    public genericAddressData: GenericAddressData,
    public zipCodeData: ZipCodeData,
    private toastr: ToastrService,) { }

  ngOnInit(): void {
    if (this.isAdd) {
      this.genericAddressData.initializeForms();
      this.zipCodeData.cityList = [];
    } else {
      this.genericAddressData.fillForms(this.entityAddressModel);
      this.setZipCode(this.isAdd);
    }

    this.effectiveDateOption = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };
    this.expirationDateOption = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };
  }

  get GenericFormAddress() {
    return this.genericAddressData.genericFormAddress;
  }

  setZipCode(isAdd?: boolean) {
    const zipCode = this.GenericFormAddress.get('address').get('zipCode').value;
    const formControlNames = ['state', 'city', 'zipCode'];
    this.zipCodeData.cityList = [];
    if (zipCode !== '') {
      this.zipCodeData.getZipCode(zipCode, this.GenericFormAddress, formControlNames, isAdd); // , PageSections.QuickQuoteApplicant
    } else {
      this.zipCodeData.resetForm(this.GenericFormAddress, formControlNames);
    }
    this.GenericFormAddress.get('address').get('city').markAsTouched();
  }

  saveAddress(): void {
    const addressList: AddressDTO[] = [];
    const address: AddressDTO = {
      id: this.isAdd ? null : this.entityAddressModel.address[0].id,
      streetAddress1: this.GenericFormAddress.get('address').get('streetAddress1')?.value,
      streetAddress2: this.GenericFormAddress.get('address').get('streetAddress2')?.value,
      cityZipCodeID: this.GenericFormAddress.get('address').get('zipCode')?.value,
      zipCode: this.GenericFormAddress.get('address').get('zipCode')?.value,
      city: this.GenericFormAddress.get('address').get('city')?.value,
      stateCode: this.GenericFormAddress.get('address').get('state')?.value,
      isGarageIndoor: this.GenericFormAddress.get('address').get('isGarageIndoor')?.value,
      isGarageOutdoor: this.GenericFormAddress.get('address').get('isGarageOutdoor')?.value,
      isGarageFenced: this.GenericFormAddress.get('address').get('isGarageFenced')?.value,
      isGarageLighted: this.GenericFormAddress.get('address').get('isGarageLighted')?.value,
      isGarageWithSecurityGuard: this.GenericFormAddress.get('address').get('isGarageWithSecurityGuard')?.value
    };
    addressList.push(address);

    const entityAddress: EntityAddressDTO = {
      id: this.isAdd ? null : this.entityAddressModel.id,
      addressTypeId: this.GenericFormAddress.get('addressType')?.value,
      effectiveDate: this.GenericFormAddress.get('effectiveDate')?.value,
      expirationDate: this.GenericFormAddress.get('expirationDate')?.value,
      address: addressList
    };

    if (this.checkIsAddressExisting(entityAddress)){
        this.toastr.error('Address already exist.', 'Oops..');
        return;
    }

    if (this.isAdd) {
      this.genericAddressData.addressList.push(entityAddress);
    }

    this.closeDialog();
    this.event.emit({ data: entityAddress, res: 200 });
  }

  closeDialog(): void {
    this.modalRef.hide();
  }

  checkIsAddressExisting(address: any): boolean {
    return this.genericAddressData.addressList.some(add =>
      address.addressTypeId === add.addressTypeId &&
        address.address[0].streetAddress1 === add.address[0]?.streetAddress1 &&
        address.address[0].streetAddress2 === add.address[0]?.streetAddress2  &&
        address.address[0].city === add.address[0]?.city &&
        address.address[0].stateCode === add.address[0]?.stateCode &&
        address.address[0].zipCode === add.address[0]?.zipCode
    );
  }

  // HAYDEN: Revisit for removed functions
}