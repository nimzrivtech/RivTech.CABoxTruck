import { EventEmitter, ViewChild } from '@angular/core';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ApplicantData } from '../../../../../../../../modules/submission/data/applicant/applicant.data';
import { DataTableDirective } from 'angular-datatables';
import { of, Subject } from 'rxjs';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import NotifUtils from '../../../../../../../../shared/utilities/notif-utils';
import { ZipcodeService } from '../../../../../../../../core/services/generics/zipcode.service';
import { catchError, debounceTime, distinctUntilChanged, filter, map, switchMap, take, takeUntil, tap } from 'rxjs/operators';
import { EntityAddressDTO } from '../../../../../../../../shared/models/submission/entityAddressDto';
import { AddressType, LvAddressTypeList } from '../../../../../../../../shared/constants/name-and-address.options.constants';
import { AddressDTO } from '../../../../../../../../shared/models/submission/addressDto';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { AddressDialogComponent } from './address-dialog/address-dialog.component';
import Utils from '../../../../../../../../shared/utilities/utils';
import { ApplicantNameAddressService } from '../../../../../../../../modules/submission/services/applicant-name-address.service';
import { BaseClass } from '../../../../../../../../shared/base-class';
import { TableConstants } from '../../../../../../../../shared/constants/table.constants';
import { PolicySummaryData } from '../../../../../../../../modules/policy/data/policy-summary.data';
import { DatatableInitModule } from '@app/views/tables/datatable/datatable.module';

@Component({
  selector: 'app-name-and-address-section',
  templateUrl: './name-and-address-section.component.html',
  styleUrls: ['./name-and-address-section.component.scss']
})
export class NameAndAddressSectionComponent extends BaseClass implements OnDestroy, OnInit {
  // Table Settings
  @ViewChild(DataTableDirective)
  dtElement: DataTableDirective;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  isDtInitialized: boolean = false;
  TableConstants = TableConstants;

  // Dropdown Settings
  dropdownSettings: IDropdownSettings = {
    singleSelection: true,
    idField: 'value',
    textField: 'label',
    selectAllText: 'Select All',
    unSelectAllText: 'UnSelect All',
    itemsShowLimit: 3,
    allowSearchFilter: true,
    allowRemoteDataSearch: true
  };

  hideMe: boolean = false;
  disableCity: boolean = false;
  disableCityOperations: boolean = false;
  displayAddresses: boolean = false;
  modalRef: BsModalRef;

  // NgSelect Dropdown PrimaryCityOfOperations
  cityStateZipList: any[] = [];
  cityStateZipLoading = false;
  cityStateZipInput = new EventEmitter<string>();
  selectedCityStateZip: any;
  minLengthTerm = 3;

  constructor(public applicantData: ApplicantData,
    public submissionData: SubmissionData,
    private zipcodeService: ZipcodeService,
    private modalService: BsModalService,
    private applicantNameAddressService: ApplicantNameAddressService,
    public policySummaryData: PolicySummaryData
  ) { super(); }

  ngOnInit() {
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true,
      processing: true,
      destroy: true
    };

    this.applicantData.cityList = [];
    this.applicantData.cityListOperations = [];

    this.initiateCityStateList();
    if (!this.submissionData.isNew) {
      this.getAddresses();
    }
    this.renderTable();

    this.applicantData.applicantForms.nameAndAddressForm.get('renewal').disable();
    this.applicantData.applicantForms.nameAndAddressForm.get('previousPolicyNumber').disable();
  }

  ngOnDestroy(): void {
    // Do not forget to unsubscribe the event
    this.dtTrigger.unsubscribe();
  }

  renderTable() {
    if (this.isDtInitialized) {
      this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
        // Destroy the table first
        dtInstance.destroy();
        // Call the dtTrigger to rerender again
        this.dtTrigger.next();
      });
    } else {
      this.dtTrigger.next();
      this.isDtInitialized = true;
    }
  }

  initiateCityStateList() {
    // Initiate Primary city typeahead observable
    this.cityStateZipInput
      .pipe(
        takeUntil(this.stop$),
        filter(res => {
          return res !== null && res.length >= this.minLengthTerm;
        }),
        distinctUntilChanged(),
        debounceTime(800),
        tap(() => this.cityStateZipLoading = true),
        switchMap(term => {
          return this.zipcodeService.searchZipCodes(term).pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.cityStateZipLoading = false),
          );
        })
      ).subscribe(item => {
        this.cityStateZipList = item.map((x) => ({
          id: x.id,
          citystatezip: x.city + ', ' + x.stateCode + ', ' + x.zipCode
        }));
      });
  }

  setCityStateZipCode(initialSet) {
    const zipCode = this.applicantData.applicantForms.nameAndAddressForm.controls['businessZipCodeInput'].value;
    this.applicantData.cityList = [];
    if (zipCode !== '') {
      // Set City dromdownlist values
      Utils.blockUI();
      this.zipcodeService.getZipCodes(zipCode).pipe(take(1)).subscribe(data => {
        this.applicantData.cityList = data.map(x => x.city);
        if (this.applicantData.cityList.length > 0) {
          this.applicantData.applicantForms.nameAndAddressForm.controls['businessStateInput'].patchValue(data[0].stateCode);
          if (this.applicantData.cityList.length === 1) {
            this.applicantData.applicantForms.nameAndAddressForm.controls['businessCityDropdown'].patchValue(data[0].city);
            this.applicantData.applicantForms.nameAndAddressForm.controls['businessCityDropdown'].disable();
            this.disableCity = true;
          } else {
            if (initialSet == null) {
              this.applicantData.applicantForms.nameAndAddressForm.controls['businessCityDropdown'].patchValue(null);
            }
            this.applicantData.applicantForms.nameAndAddressForm.controls['businessCityDropdown'].disable();
            this.disableCity = true;
          }
          this.setAddressAndCity();
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

  setCityStateZipCodeOperations(initialSet) {
    const zipCode = this.applicantData.applicantForms.nameAndAddressForm.controls['zipCodeOperations'].value;
    this.applicantData.cityListOperations = [];
    if (zipCode !== '') {
      // Set City dromdownlist values
      Utils.blockUI();
      this.zipcodeService.getZipCodes(zipCode).pipe(take(1)).subscribe(data => {
        this.applicantData.cityListOperations = data.map(x => x.city);
        if (this.applicantData.cityListOperations.length > 0) {
          this.applicantData.applicantForms.nameAndAddressForm.controls['stateOperations'].patchValue(data[0].stateCode);
          if (this.applicantData.cityListOperations.length === 1) {
            this.applicantData.applicantForms.nameAndAddressForm.controls['cityOperations'].patchValue(data[0].city);
            this.disableCityOperations = true;
          } else {
            if (initialSet == null) {
              this.applicantData.applicantForms.nameAndAddressForm.controls['cityOperations'].patchValue(null);
            }
            this.applicantData.applicantForms.nameAndAddressForm.controls['cityOperations'].disable();
            this.disableCityOperations = true;
          }
          Utils.unblockUI();
        } else {
          Utils.unblockUI();
          NotifUtils.showError('Zip code not found, contact Underwriting');
          this.resetZipCodeOperations();
        }
      });
    } else {
      this.resetZipCodeOperations();
    }
  }

  checkZip(zipValue) {
    if (zipValue == null) {
      this.resetZipCode();
      return;
    }

    this.setCityStateZipCode(null);
  }

  checkZipOperations(zipValue) {
    if (zipValue == null) {
      this.resetZipCodeOperations();
      return;
    }

    this.setCityStateZipCodeOperations(null);
  }

  trackByFn(item: any) {
    return item.id;
  }

  getValues() {
    if (this.selectedCityStateZip) {
      this.applicantData.applicantForms.nameAndAddressForm.controls['cityOfOperationsDropdown'].setValue(this.selectedCityStateZip.id);
    } else {
      this.applicantData.applicantForms.nameAndAddressForm.controls['cityOfOperationsDropdown'].setValue('');
    }
  }

  getAddresses() {
    Utils.blockUI();
    this.applicantNameAddressService.getNameAndAddress(this.submissionData.riskDetail.id).pipe(take(1)).subscribe(data => {
      Utils.unblockUI();
      this.applicantData.addressList = data.addresses;
      this.applicantData.applicantForms.nameAndAddressForm.controls['businessNameInput'].patchValue(this.submissionData.riskDetail.nameAndAddress.businessName);
      this.applicantData.applicantForms.nameAndAddressForm.controls['insuredDbaInput'].patchValue(this.submissionData.riskDetail.nameAndAddress.insuredDBA);
      this.applicantData.applicantForms.nameAndAddressForm.controls['businessPrincipalInput'].patchValue(this.submissionData.riskDetail.nameAndAddress.businessPrincipal);
      this.applicantData.applicantForms.nameAndAddressForm.controls['emailInput'].patchValue(this.submissionData.riskDetail.nameAndAddress.email);
      this.applicantData.applicantForms.nameAndAddressForm.controls['phoneExtInput'].patchValue(this.submissionData.riskDetail.nameAndAddress.phoneExt);

      this.applicantData.applicantForms.nameAndAddressForm.controls['renewal'].patchValue(this.submissionData.riskDetail.isRenewal);
      this.applicantData.applicantForms.nameAndAddressForm.controls['previousPolicyNumber'].patchValue(this.submissionData.riskDetail.previousPolicyNumber);

      this.applicantData.applicantForms.nameAndAddressForm.controls['businessAddressInput'].patchValue(data.businessAddress);
      if (data.zipCode) {
        this.applicantData.applicantForms.nameAndAddressForm.controls['businessZipCodeInput'].patchValue(data.zipCode?.toString());
        this.applicantData.applicantForms.nameAndAddressForm.controls['businessStateInput'].patchValue(data.state);
        this.applicantData.applicantForms.nameAndAddressForm.controls['businessCityDropdown'].patchValue(data.city);
        this.setCityStateZipCode(data.city);
      }
      if (data.zipCodeOperations) {
        this.applicantData.applicantForms.nameAndAddressForm.controls['zipCodeOperations'].patchValue(data.zipCodeOperations?.toString());
        this.applicantData.applicantForms.nameAndAddressForm.controls['stateOperations'].patchValue(data.stateOperations);
        this.applicantData.applicantForms.nameAndAddressForm.controls['cityOperations'].patchValue(data.cityOperations);
        this.setCityStateZipCodeOperations(data.city);
      }

      if (this.applicantData.addressList.length > 0) {
        this.applicantData.addressList.forEach(data => {
          data.addressType = this.applicantData.getAddressTypeName(data.addressTypeId);
        });
        this.displayAddresses = true;
        this.applicantData.sortAddressList();
        this.applicantData.setPage(1, this.applicantData.addressList);
      }
    }, (error) => {
      // Utils.unblockUI();
      NotifUtils.showError('There was an error on Saving. Please try again.');
    });
  }

  resetZipCode() {
    this.applicantData.cityList = [];

    this.applicantData.applicantForms.nameAndAddressForm.controls['businessZipCodeInput'].setValue('');
    this.applicantData.applicantForms.nameAndAddressForm.controls['businessStateInput'].setValue('');
    this.applicantData.applicantForms.nameAndAddressForm.controls['businessCityDropdown'].setValue(null);
    this.applicantData.applicantForms.nameAndAddressForm.controls['businessCityDropdown'].enable();
    this.disableCity = true;
  }

  resetZipCodeOperations() {
    this.applicantData.cityListOperations = [];

    this.applicantData.applicantForms.nameAndAddressForm.controls['zipCodeOperations'].setValue('');
    this.applicantData.applicantForms.nameAndAddressForm.controls['stateOperations'].setValue('');
    this.applicantData.applicantForms.nameAndAddressForm.controls['cityOperations'].setValue(null);
    this.applicantData.applicantForms.nameAndAddressForm.controls['cityOperations'].enable();
    this.disableCityOperations = true;
  }

  clearBusinessAddressAndSame() {
    const newAddressList = [];
    if (this.applicantData.addressList.length > 0) {
      const businessData = this.applicantData.addressList.find(x => x.addressTypeId === AddressType.Business);
      if (businessData) {
        this.applicantData.addressList.forEach((x, index) => {
          if (parseInt(x.address.zipCode) !== parseInt(businessData.address.zipCode) || x.address.streetAddress1 !== businessData.address.streetAddress1
            || x.address.state !== businessData.address.state || x.address.city !== businessData.address.city) {
            newAddressList.push(x);
          }
        });

        this.applicantData.addressList = newAddressList;
      }
    }
  }

  setAddressAndCity() {
    if (!this.isAddressComplete()) {
      console.log('incomplete');
      // this.clearBusinessAddressAndSame();
      return;
    }

    this.patchAddressList();
    if (this.applicantData.addressList.length > 0) {
      const businessData = this.applicantData.addressList.find(x => x.addressTypeId === AddressType.Business).address;
      this.applicantData.addressList.forEach(x => {
        if (parseInt(x.address.zipCode) === parseInt(businessData.zipCode) && x.address.streetAddress1 === businessData.streetAddress1
          && x.address.state === businessData.state && x.address.city === businessData.city && x.addressTypeId !== AddressType.Business) {
          x.address.streetAddress1 = this.applicantData.applicantForms.nameAndAddressForm.controls['businessAddressInput'].value;
          x.address.state = this.applicantData.applicantForms.nameAndAddressForm.controls['businessStateInput'].value;
          x.address.zipCode = this.applicantData.applicantForms.nameAndAddressForm.controls['businessZipCodeInput'].value;
          x.address.city = this.applicantData.applicantForms.nameAndAddressForm.controls['businessCityDropdown'].value;
        }
      });

      businessData.streetAddress1 = this.applicantData.applicantForms.nameAndAddressForm.controls['businessAddressInput'].value;
      businessData.state = this.applicantData.applicantForms.nameAndAddressForm.controls['businessStateInput'].value;
      businessData.zipCode = this.applicantData.applicantForms.nameAndAddressForm.controls['businessZipCodeInput'].value;
      businessData.city = this.applicantData.applicantForms.nameAndAddressForm.controls['businessCityDropdown'].value;
    }

    if (this.applicantData.addressList.length > 0) {
      this.displayAddresses = true;
    }

    this.applicantData.sortAddressList();
    this.applicantData.setPage(1, this.applicantData.addressList);
  }

  isAddressComplete() {
    if (this.applicantData.applicantForms.nameAndAddressForm.get('businessCityDropdown').value === null) {
      return false;
    }
    return this.applicantData.applicantForms.nameAndAddressForm.controls['businessAddressInput'].value &&
      this.applicantData.applicantForms.nameAndAddressForm.controls['businessZipCodeInput'].value &&
      this.applicantData.applicantForms.nameAndAddressForm.controls['businessStateInput'].value &&
      this.applicantData.applicantForms.nameAndAddressForm.controls['businessCityDropdown'].value;
  }

  patchAddressList() {
    LvAddressTypeList.forEach(x => {
      const address = this.applicantData.addressList.find(xx => xx.addressTypeId === x.id);
      if (address) {
        return;
      }
      // Add New Address
      const newAddress = new EntityAddressDTO({
        addressTypeId: x.id,
        addressType: x.value,
        address: new AddressDTO({
          streetAddress1: this.applicantData.applicantForms.nameAndAddressForm.controls['businessAddressInput'].value,
          zipCode: this.applicantData.applicantForms.nameAndAddressForm.controls['businessZipCodeInput'].value,
          state: this.applicantData.applicantForms.nameAndAddressForm.controls['businessStateInput'].value,
          city: this.applicantData.applicantForms.nameAndAddressForm.controls['businessCityDropdown'].value,
          isMainGarage: x.id === AddressType.Garaging ? true : false
        })
      });
      this.applicantData.addressList.push(newAddress);
    });

    const GaragingAddress = this.applicantData.addressList.find(xx => xx.addressTypeId === AddressType.Garaging && xx.address.isMainGarage === true);
    if (!GaragingAddress) {
      this.applicantData.addressList.push(new EntityAddressDTO({
        addressTypeId: AddressType.Garaging,
        addressType: this.applicantData.getAddressTypeName(AddressType.Garaging),
        address: new AddressDTO({
          streetAddress1: this.applicantData.applicantForms.nameAndAddressForm.controls['businessAddressInput'].value,
          zipCode: this.applicantData.applicantForms.nameAndAddressForm.controls['businessZipCodeInput'].value,
          state: this.applicantData.applicantForms.nameAndAddressForm.controls['businessStateInput'].value,
          city: this.applicantData.applicantForms.nameAndAddressForm.controls['businessCityDropdown'].value,
          isMainGarage: true
        })
      }));
    }
  }

  deleteAddress(item) {
    if (!this.policySummaryData.canEditPolicy) { return; }
    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${item?.address?.streetAddress1}'?`, () => {
      if (item.address.id !== undefined) {
        this.applicantNameAddressService.removeAddressMidterm(item.address.id).pipe(take(1)).subscribe(() => {
          const index: number = this.applicantData.addressList.indexOf(item);
          if (index !== -1) {
            this.applicantData.addressList.splice(index, 1);
            this.applicantData.setPage(1, this.applicantData.addressList);
          }
        });
      } else {
        const index: number = this.applicantData.addressList.indexOf(item);
        if (index !== -1) {
          this.applicantData.addressList.splice(index, 1);
          this.applicantData.setPage(1, this.applicantData.addressList);
        }
      }
    });
  }

  changeMainGarage(entityAddressData) {
    if (entityAddressData.address.isMainGarage) {
      return false;
    }
    this.applicantData.addressList.forEach(x => x.address.isMainGarage = false);
    entityAddressData.address.isMainGarage = true;
    entityAddressData.isMainGaragingAddress = true;
  }

  openAddAddressModal() {
    this.modalRef = this.modalService.show(AddressDialogComponent, {
      initialState: {
        isEdit: false
      },
      backdrop: 'static',
      keyboard: false,
      class: 'modal-md'
    });
  }

  openEditAddressModal(item) {
    if (!this.policySummaryData.canEditPolicy) { return; }
    this.modalRef = this.modalService.show(AddressDialogComponent, {
      initialState: {
        isEdit: true,
        modalTitle: 'Edit Entity Address',
        modalButton: 'Save',
        editItem: item
      },
      backdrop: 'static',
      keyboard: false,
      class: 'modal-md'
    });
    this.modalRef.content.onClose.subscribe(result => {
      if (result) {
        this.setCityStateZipCode(result);
      }
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.applicantData.applicantForms.nameAndAddressForm.controls; }
  get fg() { return this.applicantData.applicantForms.nameAndAddressForm; }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }
}
