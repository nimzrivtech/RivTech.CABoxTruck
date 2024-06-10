import { Injectable } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators, FormArray } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { Subject } from 'rxjs';
import { Router } from '@angular/router';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { BaseClass } from '../../../shared/base-class';
import { BusinessTypeDTO } from '../../../shared/models/management/business-typeDto';
import { BusinessTypeService } from '../../../core/services/management/business-type.service';
import { NAICSCodeDTO } from '../../../shared/models/management/naicsCodeDto';
import { SaveAgencyDTO } from '../../../shared/models/management/agency-management/save-agency-Dto';
import { AddressTypeDTO } from '../../../shared/models/management/agency-management/address-typeDto';
import { AddressTypeService } from '../../../core/services/management/address-type.service';
import { EntityDTO } from '../../../shared/models/management/agency-management/entityDto';
import { ITableTr } from '../../../shared/models/dynamic/table.interface';
import { AgencyTableConstants } from '../../../shared/constants/agency.table.constants';
import { AgencyDTO } from '../../../shared/models/management/agency-management/agencyDto';
import { GenericAddressData } from '../../../shared/components/generic-address/data/generic-address.data';
import { AddressDTO } from '../../../shared/models/management/addressDto';
import { AgencyService } from '../../../core/services/management/agency-service';
import { distinctUntilChanged, takeUntil } from 'rxjs/operators';
import Utils from '../../../shared/utilities/utils';
import NotifUtils from '../../../shared/utilities/notif-utils';
import { environment } from '../../../../environments/environment';
import { CustomValidators } from '../../../shared/validators/custom.validator';
import { ProgramStateAgencyService } from '../../../core/services/management/programstate-agency-service';
import { LvAccountType, LvAgencyManagementSystem, LvAgencyNetwork, LvCommissionType } from '../../../shared/constants/agency.options.constants';
import { AgencyEntity, AgencyRequestDTO, CommissionFlatRate } from '../../../shared/models/management/agency-management/agency.request.dto';
import { AgencyDetailsResponseDTO } from '../../../shared/models/management/agency-management/agency-details.response.dto';
import { EntityAddressDTO } from '../../../shared/models/management/EntityAddressDto';
import { AgencyAddressEntityAddressDTO } from '../../../shared/models/management/agency-address.dto';
import { AgencyLabelConstants } from '../../../shared/constants/agency.label.constants';
import { AuthService } from 'app/core/services/auth.service';
import { UserService } from '../../../core/services/management/user.service';
import { AuditLogService } from '../../../core/services/management/auditLog.service';
import { RetailerService } from '@app/core/services/management/retailer.service';
import { SubAgencyDTO } from '@app/shared/models/management/agency-management/sub-agencyDto';
const defaultGUID = '00000000-0000-0000-0000-000000000000';
const Flat = 'F';
@Injectable({
  providedIn: 'root'
})
export class RetailerData extends BaseClass {

  AgencyTableConstants = AgencyTableConstants;

  agencyId: string;
  activeTab: string = AgencyLabelConstants.detailsTab;
  subAgencyActiveTab: string = AgencyLabelConstants.detailsTab;
  agentActiveTab: string = AgencyLabelConstants.detailsTab;
  isFlatCommisionsUpdated: boolean = false;
  isAddressesUpdated: boolean = false;

  // Form Groups
  agencyFormGroup: FormGroup;
  subagencyFormGroup: FormGroup;
  agencyFormAddressGroup: FormGroup;
  flatCommissionFormGroup: FormGroup;
  addressFormGroup: FormGroup;

  // Date Option
  effectiveDateOption: IAngularMyDpOptions;
  expirationDateOption: IAngularMyDpOptions;

  currentAgency: SaveAgencyDTO;
  agencies = new Subject<AgencyDTO[]>();
  tableRows: ITableTr[] = [];

  // Dropdown Values
  accountTypeList: any[];
  regionalSalesManagerList: any[] = [];
  networkList: any[];
  agencyManagementSystemList: any[];
  commissionTypeList: any[];
  commissionGroupList: any[] = [];
  addressTypes: AddressTypeDTO[];
  businessTypes: BusinessTypeDTO[];
  naicsCodes: NAICSCodeDTO[];
  addtionalNaicsCodes: NAICSCodeDTO[];

  agencyNotifier = new Subject<{ data: AgencyDTO; isComplete: boolean; hasError: boolean }>();

  // Loading

  isAgencyLoading: boolean = false;
  isFlatCommissionLoading: boolean = false;
  isScheduleLoading: boolean = false;
  isAddressLoading: boolean = false;
  isDetailsTab: boolean = false;

  // Headers
  agencyListHeader: any[] = [{
    name: this.AgencyTableConstants.agencyName,
    columnName: 'entity.companyName'
  },
  {
    name: this.AgencyTableConstants.agencyEmailAddress,
    columnName: 'entity.workEmailAddress'
  },
  {
    name: this.AgencyTableConstants.agencyWorkPhone,
    columnName: 'entity.workPhone'
  },
  {
    name: this.AgencyTableConstants.agencyStatus,
    columnName: 'entity.isActive'
  }];

  flatCommissionHeader: string[] = [
    this.AgencyTableConstants.newBusiness,
    this.AgencyTableConstants.renewalBusiness,
    this.AgencyTableConstants.effectiveDate,
    this.AgencyTableConstants.expirationDate
  ];

  scheduleHeader: string[] = [
    this.AgencyTableConstants.newBusiness,
    this.AgencyTableConstants.renewalBusiness,
    this.AgencyTableConstants.effectiveDate,
    this.AgencyTableConstants.expirationDate
  ];

  addressHeaders: string[] = [
    this.AgencyTableConstants.type,
    this.AgencyTableConstants.city,
    this.AgencyTableConstants.state,
    this.AgencyTableConstants.zipCode,
  ];

  // List
  agencyListData: AgencyDTO[] = [];

  retailerListData: AgencyDTO[] = [];

  retailerAgenciesList: SubAgencyDTO[] = [];

  addressList: AgencyAddressEntityAddressDTO[] = [];
  agencyEntityListData: EntityDTO[] = [];
  agencyFlatCommission: CommissionFlatRate[] = [];
  agencyScheduleList: any[] = [];
  underwriterList: any[] = [];
  // object
  agencyDetails: AgencyDetailsResponseDTO;

  // rowid
  agencyFlatCommissionRowId: number = 0;
  addressListRowId: number = 0;
  agencyScheduleRowId: number = 0;

  // others
  isSearched: boolean = false;
  showUpdate: boolean = false;
  showNext: boolean = false;
  showSave: boolean = false;
  isAddressListEmpty: boolean = false;

  constructor(
    private fb: FormBuilder,
    private addressTypeService: AddressTypeService,
    private businessTypeService: BusinessTypeService,
    public genericAddressData: GenericAddressData,
    private agencyService: AgencyService,
    private programStateAgencyService: ProgramStateAgencyService,
    private router: Router,
    private authService: AuthService,
    private userService: UserService,
    private auditLogService: AuditLogService,
    private retailerService: RetailerService) {
    super();
  }

  initializeForms() {
    this.agencyFormGroup = this.agencyFormSection();
    this.initDateOptions();
    this.initDropDownValues();
  }

  agencyFormSection(): FormGroup {
    return this.fb.group({
      id: new FormControl(0),
      companyName: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-ZñÑ0-9 !@#$%^&*‘’“”|()\\-`.+,/\_={}:;~"<>\']*$')]),
      federalIDNumber: new FormControl('', [CustomValidators.federalIdNumberValidator]),
      dba: new FormControl(''),
      contactName: new FormControl('',  ),
      workPhone: new FormControl('', [CustomValidators.phoneNumberValidator]),
      workFax: new FormControl('', [CustomValidators.spaceValidator, CustomValidators.phoneNumberValidator]),
      workEmailAddress: new FormControl('', [Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,10}$'), Validators.required, CustomValidators.spaceValidator]),
      businessTypeID: new FormControl('', ),

      isEftAvailable: new FormControl(false),
      eftAccountNumber: new FormControl('',  []),
      eftAccountType: new FormControl('',  []),
      eftRoutingNumber: new FormControl('',  []),
      eftCommissionEmail: new FormControl('', [Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,10}$'), CustomValidators.spaceValidator]),
      isCommissionPaidToAgency: new FormControl(true),
      hasBindingAuthority: new FormControl(false),
      is1099Required: new FormControl(false),
      regionalSalesManager: new FormControl('', []),
      agencyManagementSystem: new FormControl(''),
      network: new FormControl('', []),
      commissionType: new FormControl('F', []),
      commissionGroup: new FormControl('', []),
      underwriter: new FormControl('', []),

      // hasSubsidiaries: new FormControl(true),
      agencyCode: new FormControl('', []),
      isActive: new FormControl(true),

      // for sub agency tab additional fields setting this to not required so it won't affect the validation for agency details
      lexisNexisCode: new FormControl(''),
      licenseEffectiveDate: new FormControl(''),
      licenseExpirationDate: new FormControl(''),
      subAgencyCode: new FormControl(''),
    });
  }

  initDropDownValues(): void {

    this.addressTypeService.addressTypesAll().takeUntil(this.stop$).subscribe((res) => {
      this.addressTypes = res.filter(x => x.isActive);
    });

    this.businessTypeService.businessTypesAll().takeUntil(this.stop$).subscribe((res) => {
      this.businessTypes = res.filter(x => x.isActive);
    });

    this.regionalSalesManagerList = [];
    this.agencyService.getRegionalSalesManagerList().takeUntil(this.stop$).subscribe((res) => {
      res.map(result => {
        if (result?.entity?.isActive) {
          this.regionalSalesManagerList.push(
            {
              code: result?.id,
              description: result?.entity?.fullName
            }
          );
        }
      });
    }, err => {
    });

    this.commissionGroupList = [];
    // this.agencyService.getCommissionGroup().takeUntil(this.stop$).subscribe((res) => {
    //   res.map(result => {
    //     if (result?.isActive) {
    //       this.commissionGroupList.push(
    //         {
    //           code: result?.id,
    //           description: result?.commissionGroupName
    //         }
    //       );
    //     }
    //   });
    // }, err => {
    // });

    this.agencyFormAddressGroup = this.fb.group({
      addresses: this.fb.array([this.newAddress()])
    });

    this.accountTypeList = LvAccountType;
    this.networkList = LvAgencyNetwork;
    this.agencyManagementSystemList = LvAgencyManagementSystem;
    this.commissionTypeList = LvCommissionType;

    this.underwriterList = [];

    this.userService.getUnderwriterList().takeUntil(this.stop$).subscribe((res) => {
      res.map(result => {
        this.underwriterList.push(
          {
            code: result?.userId,
            description: `${result?.firstName} ${result?.lastName}`
          }
        );
      });
    }, err => {
      NotifUtils.showError(err);
    });
  }

  initDateOptions(): void {
    this.effectiveDateOption = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };

    this.expirationDateOption = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };
  }


  getAgencyDTOSave(isNew: boolean): SaveAgencyDTO {

    const request = new SaveAgencyDTO({
      programId: environment.ApplicationId,
      entity: new EntityDTO({
        isIndividual: false, //Default since Entity is Company not a Person
        companyName: this.agencyFormGroup.value.companyName,
        fullName: this.agencyFormGroup.value.companyName,
        mobilePhone: this.agencyFormGroup.value.mobilePhone,
        workPhone: this.agencyFormGroup.value.workPhone,
        workFax: this.agencyFormGroup.value.workFax,
        workEmailAddress: this.agencyFormGroup.value.workEmailAddress,
        businessTypeID: Number(this.agencyFormGroup.value.businessTypeID),
        entityAddresses: this.getAddressDTOToSave(true),
        dba: this.agencyFormGroup.value.dba,
        federalIDNumber: this.agencyFormGroup.value.federalIDNumber,
        naicsCodeID: Number(this.agencyFormGroup.value.naicsCodeID),
        additionalNAICSCodeID: Number(this.agencyFormGroup.value.additionalNAICSCodeID),
        iccmcDocketNumber: this.agencyFormGroup.value.iccmcDocketNumber,
        usdotNumber: this.agencyFormGroup.value.usdotNumber,
        pucNumber: this.agencyFormGroup.value.pucNumber,
        hasSubsidiaries: this.agencyFormGroup.value.hasSubsidiaries,
        isActive: this.agencyFormGroup.value.isActive,
      })
    });

    if (!isNew) {
      request.id = this.currentAgency.id;
      request.entity.id = this.currentAgency.entity.id;
    }

    return request;
  }

  mapAgencyDTOToEditinAgencyForm() {

    this.agencyFormGroup.patchValue(this.currentAgency.entity); // so the isActive will not be overwritten

    const addresses = this.currentAgency.entity.entityAddresses.filter(a => a.removeProcessDate == null); //Exclude InActive

    if (addresses.length > 1) {
      for (let i = 0; i < (addresses.length - 1); i++) {
        this.addAddress();
      }
    }
    const datePipe = new DatePipe('en-US');
    addresses.forEach(x => {
      x.effectiveDate = datePipe.transform(x.effectiveDate, 'yyyy-MM-dd');
      x.expirationDate = datePipe.transform(x.expirationDate, 'yyyy-MM-dd');
    });
    this.agencyFormAddressGroup.patchValue({ addresses: addresses });
  }

  getAddressDTOToSave(isNew: boolean): EntityAddressDTO[] {
    const result: Array<EntityAddressDTO> = [];

    this.genericAddressData.addressList.forEach((x: any) => {
      const address = new EntityAddressDTO({
        addressTypeId: x.addressTypeId,
        effectiveDate: this.authService.getCustomDate().toDateString(),
        expirationDate: this.authService.getCustomDate().toDateString(),
        addProcessDate: this.authService.getCustomDate(),
        address: new AddressDTO({
          streetAddress1: x.address[0].streetAddress1,
          streetAddress2: x.address[0].streetAddress2,
          cityZipCodeID: Number(x.address[0].cityZipCodeID),
          zipCode: x.address[0].zipCode,
          city: x.address[0].city,
          stateCode: x.address[0].stateCode,
          zipCodeExt: '',
          longitude: 0,
          latitude: 0,
          isGarageIndoor: x.address[0].isGarageIndoor,
          isGarageOutdoor: x.address[0].isGarageOutdoor,
          isGarageFenced: x.address[0].isGarageFenced,
          isGarageLighted: x.address[0].isGarageLighted,
          isGarageWithSecurityGuard: x.address[0].isGarageWithSecurityGuard
        })
      });

      if (x.id != null) {
        address.id = x.id;
        address.address.id = x.address[0].id;
      }

      result.push(address);
    });

    //Identity disabled/removed controls and mark isActive as 0
    if (!isNew && this.currentAgency.entity.entityAddresses.length) {
      this.currentAgency.entity.entityAddresses.forEach((a) => {
        const r = result.find((x) => x.id === a.id);

        if (r == null) {
          a.removeProcessDate = this.authService.getCustomDate();
          result.push(a);
        }
      });
    }

    return result;
  }

  newAddress(): FormGroup {
    return this.fb.group({
      id: new FormControl(null),
      isActive: new FormControl(true),
      addressTypeId: new FormControl(null, [Validators.required]),
      effectiveDate: new FormControl(null, [Validators.required]),
      expirationDate: new FormControl(null, [Validators.required]),
      address: this.fb.group({
        id: new FormControl(null),
        streetAddress1: new FormControl('', [Validators.required]),
        streetAddress2: new FormControl(''),
        cityZipCodeID: new FormControl(0),
        zipCodePlus: new FormControl(''),
        zipCode: new FormControl('', [Validators.required]),
        city: new FormControl(''),
        stateCode: new FormControl(''),
        longitude: new FormControl(0),
        latitude: new FormControl(0),
        isGarageIndoor: new FormControl(false),
        isGarageOutdoor: new FormControl(false),
        isGarageFenced: new FormControl(false),
        isGarageLighted: new FormControl(false),
        isGarageWithSecurityGuard: new FormControl(false),
        isActive: new FormControl(true)
      })
    });
  }

  addAddress() {
    (<FormArray>this.agencyFormAddressGroup.get('addresses')).push(this.newAddress());
  }

  resetAgencyList(): void {
    this.agencies.next([]);
    this.agencyListData = [];
    this.retailerAgenciesList = [];
    this.retailerListData = [];
  }

  /**
   * Retrieve agency and agent information for View Quote Proposal
   */
  retrieveAgencyVQP(id: string): void {
    Utils.blockUI();
    this.agencyService.find(id).pipe(takeUntil(this.stop$)).subscribe((result: AgencyDTO) => {
      this.agencyNotifier.next({ data: result, isComplete: true, hasError: false });
      Utils.unblockUI();
    }, error => {
      this.agencyNotifier.next({ data: null, isComplete: true, hasError: true });
      Utils.unblockUI();
      NotifUtils.showError(error.message);
    });
  }

  getAgencies(): void {
    this.agencyListData = [];
    this.retailerAgenciesList = [];
    this.agencyEntityListData = [];
    this.isAgencyLoading = true;
    Utils.blockUI();
    this.programStateAgencyService.getAllProgramStateAgency().pipe(distinctUntilChanged(), takeUntil(this.stop$)).subscribe(data => {
      this.agencyListData = data.map(x => {
        x.agency.contactName = x.agency.contactName === null ? '' : x.agency.contactName;
        return x.agency;
      });

      this.retailerAgenciesList = data.map(x => {
        x.agency.contactName = x.agency.contactName === null ? '' : x.agency.contactName;
        return x.agency;
      }).sort((a, b) => a?.entity?.companyName < b?.entity?.companyName ? -1 : 1);

      if (this.agencyDetails) {
        this.mapLinkedAgencies(this.agencyDetails);
      }

      this.agencyEntityListData = data.map(agencyItem => {
        return agencyItem.agency.entity;
      });
      this.agencies.next(this.agencyListData);
      this.isAgencyLoading = false;
      Utils.unblockUI();
    });
  }

  getRetailers(searchString: string = null): void {
    this.retailerListData = [];
    this.agencyEntityListData = [];
    this.isAgencyLoading = true;
    Utils.blockUI();

    this.retailerService.getAllRetailers().pipe(distinctUntilChanged(), takeUntil(this.stop$)).subscribe(data => {
      if (searchString != null) {
        const filteredData = data.filter(e => (e?.entity?.companyName +
                                               e?.entity?.workEmailAddress +
                                               e?.entity?.workPhone +
                                               (e?.entity?.isActive ? 'Active' : 'Inactive')
                                              ).toLowerCase().search(searchString.toLowerCase()) > -1);
        data = filteredData;
      }

      this.retailerListData = data.map(x => {
        return x;
      }).sort((a, b) => a?.entity?.companyName < b?.entity?.companyName ? -1 : 1);
      this.agencies.next(this.retailerListData);
      this.isAgencyLoading = false;
      Utils.unblockUI();
    });

  }

  saveAgency(): void {
    const payload: AgencyRequestDTO = this.mapAgencyPayload();
    this.agencyFlatCommission = [];
    Utils.blockUI();
    this.agencyService.postAgencyDetails(payload).subscribe(data => {
      Utils.unblockUI();
      NotifUtils.showSuccess(AgencyLabelConstants.agencyModalAddedSuccessfulyMessage, () => {
        this.agencyId = data;
        this.callAgencyAPIonLoad();
        this.router.navigate(['management', 'agency', 'edit', data]);
      });
    }, err => {
      Utils.unblockUI();
      this.agencyId = null;
    });
  }

  saveRetailer(): void {
    const payload: AgencyRequestDTO = this.mapAgencyPayload();
    this.agencyFlatCommission = [];
    payload.entity.driverLicenseExpiration = new Date(); //HAYDEN: Fix param error
    Utils.blockUI();
    this.retailerService.postRetailerDetails(payload).subscribe(data => {
      Utils.unblockUI();
      NotifUtils.showSuccess(AgencyLabelConstants.retailerModalAddedSuccessfulyMessage, () => {
        this.agencyId = data.id;
        this.callAgencyAPIonLoad();
        this.router.navigate(['management', 'retailer']);
      });
    }, err => {
      Utils.unblockUI();
      this.agencyId = null;
    });
  }

  updateAgency(agencyId: string): void {
    const payload: AgencyRequestDTO = this.mapAgencyPayload(true);
    this.agencyFlatCommission = [];
    Utils.blockUI();
    this.agencyService.putAgencyDetails(payload, agencyId).subscribe(data => {
      Utils.unblockUI();
      NotifUtils.showSuccess(AgencyLabelConstants.retailerModalUpdatedSuccessfulyMessage, () => {
        this.agencyId = data;
        this.getAgencyDetails(agencyId);
        this.callAgencyAPIonLoad();
        this.router.navigate(['management', 'retailer', 'edit', data]);
        this.agencyFormGroup.reset();
        this.resetAgencyUpdatesTrackers();
      });
    }, err => {
      Utils.unblockUI();
      this.agencyId = null;
    });
  }

  updateRetailer(retailerId: string): void {
    const payload: AgencyRequestDTO = this.mapAgencyPayload(true);
    this.agencyFlatCommission = [];
    payload.entity.driverLicenseExpiration = new Date();
    Utils.blockUI();
    this.retailerService.putRetailerDetails(payload, retailerId).subscribe(data => {
      Utils.unblockUI();
      NotifUtils.showSuccess(AgencyLabelConstants.retailerModalUpdatedSuccessfulyMessage, () => {
        this.agencyId = retailerId;
        this.getRetailerDetails(retailerId);
        this.callAgencyAPIonLoad();
        this.router.navigate(['management', 'retailer']);
        this.agencyFormGroup.reset();
        this.resetAgencyUpdatesTrackers();
      });
    }, err => {
      Utils.unblockUI();
      this.agencyId = null;
    });
  }

  mapAgencyPayload(isUpdate?: boolean): AgencyRequestDTO {
    //this.mapLinkedAgenciesForSaving();
    return {
      id: isUpdate ? this.agencyDetails.id : undefined,
      entityId: isUpdate ? this.agencyDetails.entityId : undefined,
      programId: environment.ApplicationId,
      // stateCode: null,
      createdBy: isUpdate ? this.agencyDetails.createdBy : undefined,
      entity: this.mapEntityAgencyPayload(isUpdate),
      agencyStatusHistoryId: null,
      contactName: this.agencyFormGroup.get('contactName').value,
      isEFTAvailable: this.agencyFormGroup.get('isEftAvailable').value,
      eftAccountNumber: this.agencyFormGroup.get('eftAccountNumber').value,
      eftAccountTypeId: this.agencyFormGroup.get('eftAccountType').value,
      eftRoutingNumber: this.agencyFormGroup.get('eftRoutingNumber').value,
      eftEmail: this.agencyFormGroup.get('eftCommissionEmail').value,
      isCommissionPaidToAgency: this.agencyFormGroup.get('isCommissionPaidToAgency').value,
      agencyNetworkId: this.agencyFormGroup.get('network').value,
      hasBindingAuthority: this.agencyFormGroup.get('hasBindingAuthority').value,
      licenseEffectiveDate: isUpdate ? this.agencyDetails.licenseEffectiveDate : undefined,
      licenseExpirationDate: isUpdate ? this.agencyDetails.licenseExpirationDate : undefined,
      commisionTypeId: this.agencyFormGroup.get('commissionType').value,
      // commissionGroupId: this.agencyFormGroup.get('commissionGroup').value !== '' ? this.agencyFormGroup.get('commissionGroup').value : defaultGUID,
      agencyManagementSystemId: this.agencyFormGroup.get('agencyManagementSystem').value,
      regionalSalesManagerId: this.agencyFormGroup.get('regionalSalesManager').value,
      commissionFlatRates: this.mapCommissionFlatRatesAgencyPayload(),
      isActive: this.agencyFormGroup.get('isActive').value,
      agencyCode: this.agencyFormGroup.get('agencyCode').value,
      is1099Required: this.agencyFormGroup.get('is1099Required').value,
      uwUserId: Number(this.agencyFormGroup.get('underwriter').value),
      linkedAgencies: this.linkedAgencies
    };
  }

  get linkedAgencies() {
    const selectedAgencyIds = this.retailerAgenciesList.filter(ra => ra.linked).map(ra => ra.id);      
    if (this.agencyDetails) {
      this.agencyDetails.linkedAgencies = this.agencyDetails.linkedAgencies.filter(la => selectedAgencyIds.includes(la.agencyId));

      selectedAgencyIds.forEach(sai => {
        if (!this.agencyDetails.linkedAgencies.find(la => la.agencyId === sai)){
          this.agencyDetails.linkedAgencies.push({
            retailerId: this.agencyDetails.id,
            agencyId: sai
          });
        }
      });
      return this.agencyDetails.linkedAgencies;
    }

    const newLinkedAgencies = [];
    selectedAgencyIds.forEach(sai => {
      newLinkedAgencies.push({
        retailerId: defaultGUID,
        agencyId: sai
      });
    });

    return newLinkedAgencies;
  }

  mapEntityAgencyPayload(isUpdate?: boolean): AgencyEntity {
    return {
      id: isUpdate ? this.agencyDetails.entity.id : undefined,
      isIndividual: false,
      firstName: '',
      middleName: '',
      lastName: '',
      companyName: this.agencyFormGroup.get('companyName').value,
      fullName: this.agencyFormGroup.get('companyName').value,
      homePhone: '',
      mobilePhone: this.agencyFormGroup.get('workPhone').value,
      workPhone: this.agencyFormGroup.get('workPhone').value,
      workPhone2: this.agencyFormGroup.get('workPhone').value,
      workPhoneExtension: '',
      workFax: this.agencyFormGroup.get('workFax').value,
      socialSecurityNumber: '',
      personalEmailAddress: '',
      workEmailAddress: this.agencyFormGroup.get('workEmailAddress').value,
      // birthDate: null,
      age: 0,
      driverLicenseNumber: '',
      driverLicenseState: '',
      driverLicenseExpiration: null,
      genderID: 0,
      maritalStatusID: 0,
      businessTypeID: Number(this.agencyFormGroup.get('businessTypeID').value),
      programId: environment.ApplicationId,
      dba: this.agencyFormGroup.get('dba').value,
      federalIDNumber: this.agencyFormGroup.get('federalIDNumber').value,
      fein: this.agencyFormGroup.get('federalIDNumber').value,
      naicsCodeID: 0, // this.agencyFormGroup.get('naicsCodeID').value,
      additionalNAICSCodeID: 0, // this.agencyFormGroup.get('additionalNAICSCodeID').value,
      iccmcDocketNumber: '', //this.agencyFormGroup.get('iccmcDocketNumber').value,
      usdotNumber: '', // this.agencyFormGroup.get('usdotNumber').value,
      pucNumber: '', // this.agencyFormGroup.get('pucNumber').value,
      isRegionalSalesManager: isUpdate ? this.agencyDetails.entity.isRegionalSalesManager : undefined,
      isInternalUser: isUpdate ? this.agencyDetails.entity.isInternalUser : undefined,
      isAgent: isUpdate ? this.agencyDetails.entity.isAgent : undefined,
      hasSubsidiaries: isUpdate ? this.agencyDetails.entity.hasSubsidiaries : undefined, //this.agencyFormGroup.get('hasSubsidiaries').value,
      isActive: this.agencyFormGroup.get('isActive').value,
      createdBy: isUpdate ? this.agencyDetails.entity.createdBy : undefined,
      entityAddresses: this.addressList,
      deleteAddresses: []
    };
  }

  mapCommissionFlatRatesAgencyPayload(): CommissionFlatRate[] {
    this.agencyFlatCommission.forEach(item => {
      if (typeof (item.effectiveDate) !== 'string') {
        item.effectiveDate = this.getAdjustedDateISOFormat('effectiveDate', false, new Date(item.effectiveDate).toLocaleDateString('en-US'));
      }
      if (typeof (item.expirationDate) !== 'string') {
        item.expirationDate = this.getAdjustedDateISOFormat('expirationDate', false, new Date(item.expirationDate).toLocaleDateString('en-US'));
      }
    });
    return this.agencyFlatCommission;
  }

  getAgencyDetails(agencyId?): void {
    this.agencyService.getAgencyDetails(agencyId).subscribe(data => {
      this.agencyDetails = data;
      this.mapAgencyDetails(data);
      this.showingOfSaveUpdateNext(true);
    });
  }

  getRetailerDetails(agencyId?): void {
    this.retailerService.getRetailerDetails(agencyId).subscribe(data => {
      this.agencyDetails = data;
      this.mapLinkedAgencies(data);
      this.mapAgencyDetails(data);
      this.showingOfSaveUpdateNext(true);
    });
  }

  // HAYDEN
  mapLinkedAgencies(data) {
    const linkedAgencyIds = data.linkedAgencies.map(la => la.agencyId);
    this.retailerAgenciesList.forEach(ra => {
      ra.linked = linkedAgencyIds.includes(ra.id);
    });
  }

  showingOfSaveUpdateNext(isUpdate?: boolean) {
    if (isUpdate) {
      this.showUpdate = true;
      this.showSave = false;
    } else {
      this.showSave = true;
      this.showUpdate = false;
    }
    this.showNext = false;
  }

  mapAgencyDetails(data?: AgencyDetailsResponseDTO): void {
    const entity = data?.entity;
    this.agencyFormGroup.get('companyName').setValue(entity?.companyName);
    this.agencyFormGroup.get('federalIDNumber').setValue(entity?.fein);
    this.agencyFormGroup.get('dba').setValue(entity?.dba);
    this.agencyFormGroup.get('contactName').setValue(data?.contactName);
    this.agencyFormGroup.get('workPhone').setValue(entity?.workPhone);
    this.agencyFormGroup.get('workFax').setValue(entity?.workFax);
    this.agencyFormGroup.get('workEmailAddress').setValue(entity?.workEmailAddress);
    this.agencyFormGroup.get('businessTypeID').setValue(entity?.businessTypeID);
    this.agencyFormGroup.get('isEftAvailable').setValue(data?.isEFTAvailable);
    this.agencyFormGroup.get('eftAccountNumber').setValue(data?.eftAccountNumber);
    this.agencyFormGroup.get('eftAccountType').setValue(data?.eftAccountTypeId);
    this.agencyFormGroup.get('eftRoutingNumber').setValue(data?.eftRoutingNumber);
    this.agencyFormGroup.get('eftCommissionEmail').setValue(data?.eftEmail);
    this.agencyFormGroup.get('isCommissionPaidToAgency').setValue(data?.isCommissionPaidToAgency);
    this.agencyFormGroup.get('hasBindingAuthority').setValue(data?.hasBindingAuthority);
    this.agencyFormGroup.get('regionalSalesManager').setValue(data?.regionalSalesManagerId);
    this.agencyFormGroup.get('agencyManagementSystem').setValue(data?.agencyManagementSystemId);
    this.agencyFormGroup.get('network').setValue(data?.agencyNetworkId);
    this.agencyFormGroup.get('commissionType').setValue(data?.commisionTypeId);
    // this.agencyFormGroup.get('commissionGroup').setValue(data?.commissionGroupId !== defaultGUID ? data?.commissionGroupId : '');
    //this.commissionTypeLogic(this.agencyFormGroup);
    this.agencyFormGroup.get('isActive').setValue(data?.isActive);
    this.agencyFormGroup.get('agencyCode').setValue(data?.agencyCode);
    this.agencyFormGroup.get('is1099Required').setValue(data?.is1099Required);
    this.agencyFormGroup.get('underwriter').setValue(data?.uwUserId);
    this.agencyFormGroup.markAllAsTouched();
  }

  callAgencyAPIonLoad(): void {
    this.getRetailers();
    this.getAgencies();
    this.genericAddressData.getAddressType();
  }

  getAdjustedDateISOFormat(field: string, isAgencyDetailsDatePicker: boolean = true, flatComDate?: string): string {
    const date = isAgencyDetailsDatePicker ?
      new Date(this.agencyFormGroup.get(field).value?.singleDate?.jsDate?.toLocaleDateString('en-US')).toISOString() :
      new Date(flatComDate).toISOString();
    const correctDate = new Date(date);
    correctDate.setDate(correctDate.getDate() + 1);
    return correctDate.toISOString();
  }

  resetAgencyUpdatesTrackers(): void {
    this.isFlatCommisionsUpdated = false;
    this.isAddressesUpdated = false;
  }

  isCurrentActiveTab(currentTab: string): boolean {
    return currentTab === this.activeTab;
  }

  isCurrentSubAgencyModalActiveTab(currentTab: string): boolean {
    return currentTab === this.subAgencyActiveTab;
  }

  isCurrentAgentModalActiveTab(currentTab: string): boolean {
    return currentTab === this.agentActiveTab;
  }
}