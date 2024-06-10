import { Component, OnInit } from '@angular/core';
import { AgencyData } from '../../../../../../modules/management/data/agency.data';
import { ErrorMessageConstant } from '../../../../../../shared/constants/error-message.constants';
import { GenericAddressLabelConstants } from '../../../../../../shared/constants/generic-address.label.constants';
import { BsModalRef } from 'ngx-bootstrap';
import { ZipCodeData } from '../../../../../../modules/management/data/zipcode.data';
import { GenericAddressData } from '../../../../../../shared/components/generic-address/data/generic-address.data';
import { ZipCodeService } from '../../../../../../core/services/management/zip-code.service';import { takeUntil } from 'rxjs/operators';
import { BaseComponent } from '../../../../../../shared/base-component';
import { ZipcodesService } from '../../../../../../core/services/submission/zipcodes/zipcodes.service';
import { AgencyAddressDTO, AgencyAddressEntityAddressDTO } from '../../../../../../shared/models/management/agency-address.dto';
import { SubAgencyData } from '../../../../../../modules/management/data/sub-agency.data';
import { AgentData } from '../../../../../../modules/management/data/agent.data';
import { AgencyLabelConstants } from '../../../../../../shared/constants/agency.label.constants';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-agency-address-modal',
  templateUrl: './agency-address-modal.component.html',
  styleUrls: ['./agency-address-modal.component.scss']
})
export class AgencyAddressModalComponent extends BaseComponent implements OnInit {
  GenericAddressLabelConstants = GenericAddressLabelConstants;
  ErrorMessageConstant = ErrorMessageConstant;
  AgencyLabelConstants = AgencyLabelConstants;

  addressList: AgencyAddressEntityAddressDTO[];
  rowId: number;
  isEdit: boolean;
  modalTitle: string;
  buttonText: string;
  addressItem: AgencyAddressEntityAddressDTO;

  cityList = [];

  constructor(public agencyData: AgencyData
              , private bsModalRef: BsModalRef
              , private zipCodeData: ZipCodeData
              , public genericAddressData: GenericAddressData
              , private zipCodeService: ZipCodeService
              , private zipService: ZipcodesService
              , private subAgencyData: SubAgencyData
              , private agentData: AgentData
              , private toastr: ToastrService) { super(); }

  ngOnInit() {
    if (this.isEdit) {
      this.populateCityList(this.addressItem.address.zipCode);
      this.mapEditData(this.addressItem);
    } else {
      this.zipCodeData.cityList = [];
    }
  }

  get addressFormGroup(): FormGroup {
    return this.agencyData.addressFormGroup;
  }

  get isValid() {
    return this.addressFormGroup.valid && this.addressFormGroup.get('address').valid && this.addressFormGroup.touched && this.addressFormGroup.dirty;
  }

  get entityAddress(): AgencyAddressEntityAddressDTO {
    this.rowId = this.isEdit ? this.addressItem?.rowId : this.addressList.length;

    const addressData: AgencyAddressDTO = {
      streetAddress1: this.addressFormGroup.get('address').get('streetAddress1').value === null ? '' : this.removeWhiteSpace(this.addressFormGroup.get('address').get('streetAddress1').value),
      streetAddress2: this.addressFormGroup.get('address').get('streetAddress2').value === null ? '' : this.removeWhiteSpace(this.addressFormGroup.get('address').get('streetAddress2').value),
      city: this.addressFormGroup.get('address').get('city').value,
      stateCode: this.addressFormGroup.get('address').get('state').value,
      zipCode: this.addressFormGroup.get('address').get('zipCode').value,
      cityZipCodeID: this.addressFormGroup.get('address').get('zipCode')?.value
    };

    const entityAddress: AgencyAddressEntityAddressDTO = {
      addressTypeId: Number(this.addressFormGroup.get('addressType')?.value),
      rowId: this.addressItem?.rowId ? this.addressItem.rowId : this.rowId,
      address: addressData
    };

    return entityAddress;
  }

  save(): void {
    if (this.checkIsAddressExisting(this.entityAddress)){
      this.toastr.error('Address already exist.', 'Oops..');
      return;
    }

    if (!this.isEdit) {
      this.processAdd(this.entityAddress);
    } else {
      this.processEdit();
    }
    // this.agencyData.isAddressListEmpty = false;
    // this.subAgencyData.isAddressListEmpty = false;
    this.agencyData.showingOfSaveUpdateNext(this.isEdit);
    this.subAgencyData.showingOfSaveUpdateNext(this.isEdit);
    this.updateAddressTracker(this.agencyData.activeTab);
    this.addressFormGroup.reset();
    this.bsModalRef.hide();
  }

  processAdd(entityAddress: AgencyAddressEntityAddressDTO): void {
    this.addressList.push(entityAddress);
    const sortedAddressList = this.addressList.sort((a, b) => (a?.address?.city < b?.address?.city) ? -1 : 1) // sort by City
                                              .sort((a, b) => (a?.addressTypeId < b?.addressTypeId) ? -1 : 1); // sort by type
    this.addressList = sortedAddressList;
  }

  processEdit(): void {
    if (this.addressItem.id) { // use item.id => if existing in DB edit
      this.addressList = this.addressList.map(data => {
        if (data.id === this.addressItem.id) {
          this.setAddress(data);
        }
        return data;
      });
    } else {
      this.addressList = this.addressList.map(data => {
        if (data.rowId === this.addressItem.rowId) { // use item.rowId => if newly added edit
          this.setAddress(data);
        }
        return data;
      });
    }
  }

  mapEditData(entityAddress: AgencyAddressEntityAddressDTO): void {
    this.addressFormGroup.get('address').get('streetAddress1').setValue(this.removeWhiteSpace(entityAddress.address.streetAddress1));
    this.addressFormGroup.get('address').get('streetAddress2').setValue(this.removeWhiteSpace(entityAddress.address.streetAddress2));
    this.addressFormGroup.get('address').get('city').setValue(entityAddress.address.city);
    this.addressFormGroup.get('address').get('state').setValue(entityAddress.address.stateCode);
    this.addressFormGroup.get('address').get('zipCode').setValue(entityAddress.address.zipCode);
    this.addressFormGroup.get('addressType').setValue(entityAddress.addressTypeId);
    this.addressFormGroup.markAllAsTouched();
  }

  closeDialog(): void {
    this.addressFormGroup.reset();
    this.bsModalRef.hide();
  }

  setZipCode() {
    const zipCode = this.addressFormGroup.get('address').get('zipCode').value;
    const formControlNames = ['state', 'city', 'zipCode'];
    this.zipCodeData.cityList = [];
    if (zipCode !== '') {
      this.zipCodeData.getZipCode(zipCode, this.agencyData.addressFormGroup, formControlNames); // , PageSections.QuickQuoteApplicant
    } else {
      this.zipCodeData.resetForm(this.agencyData.addressFormGroup, formControlNames);
    }
    this.addressFormGroup.get('address').get('city').markAsTouched();
  }

  setAddress(data): void {
    data.address.streetAddress1 = this.removeWhiteSpace(this.addressFormGroup.get('address').get('streetAddress1').value);
    data.address.streetAddress2 = this.removeWhiteSpace(this.addressFormGroup.get('address').get('streetAddress2').value);
    data.address.city = this.addressFormGroup.get('address').get('city').value;
    data.address.stateCode = this.addressFormGroup.get('address').get('state').value;
    data.address.zipCode = this.addressFormGroup.get('address').get('zipCode').value;
    data.addressTypeId = Number(this.addressFormGroup.get('addressType').value);
  }

  populateCityList(zipCode: string) {
    if (zipCode && zipCode !== '') {
      this.zipService.getGenericZipCode(zipCode).pipe(takeUntil(this.stop$)).subscribe(res => {
        const zipCodeRes: any = res;
        this.zipCodeData.cityList = [];

        zipCodeRes.map(val => {
          const city = val.city;
          this.zipCodeData.cityList.push(city);
        });
      });
    }
  }

  updateAddressTracker(activeTab: string): void {
    switch (activeTab) {
      case AgencyLabelConstants.subAgencyTab:
        this.subAgencyData.isAddressesUpdated = true;
        break;
      case AgencyLabelConstants.agentTab:
        this.agentData.isAddressesUpdated = true;
        break;
      default:
        this.agencyData.isAddressesUpdated = true;
    }
  }

  checkIsAddressExisting(address: any): boolean {
    return this.addressList.some(add => address.rowId !== add.rowId && address.addressTypeId === add.addressTypeId &&
      address.address?.streetAddress1?.toLowerCase() === this.removeWhiteSpace(add.address?.streetAddress1).toLowerCase() &&
      address.address?.streetAddress2?.toLowerCase() === this.removeWhiteSpace(add.address?.streetAddress2).toLowerCase() &&
      address.address?.city === add.address?.city &&
      address.address?.stateCode === add.address?.stateCode &&
      address.address?.zipCode === add.address?.zipCode
    );
  }

  removeWhiteSpace(item: string): string {
    return item?.replace(/\s+/g, ' ').trim();
  }
}
