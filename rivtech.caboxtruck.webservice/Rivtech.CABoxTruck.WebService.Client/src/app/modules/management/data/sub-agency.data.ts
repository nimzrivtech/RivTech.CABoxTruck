import { Injectable } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators, FormArray } from '@angular/forms';
import { Subject } from 'rxjs';
import { BaseClass } from '../../../shared/base-class';
import { BusinessTypeService } from '../../../core/services/management/business-type.service';
import { SubAgencyTableConstants } from '../../../shared/constants/sub-agency.table.constants';
import { GenericAddressData } from '../../../shared/components/generic-address/data/generic-address.data';
import { BusinessTypeDTO } from '../../../shared/models/management/business-typeDto';
import { NAICSCodeDTO } from '../../../shared/models/management/naicsCodeDto';
import { SaveSubAgencyDTO } from '../../../shared/models/management/agency-management/save-sub-agencyDto';
import { AddressTypeDTO } from '../../../shared/models/management/agency-management/address-typeDto';
import { EntityDTO } from '../../../shared/models/management/agency-management/entityDto';
import { GenderDTO } from '../../../shared/models/management/marital-gender/genderDto';
import { MaritalStatusDTO } from '../../../shared/models/management/marital-gender/maritalDto';
import { AgencySubAgencyDTO } from '../../../shared/models/management/agency-management/agencySubAgencyDto';
import { SubAgencyDTO } from '../../../shared/models/management/agency-management/sub-agencyDto';
import Utils from '../../../shared/utilities/utils';
import { ProgramStateSubAgencyService } from '../../../core/services/management/programstate-subagency.service';
import { distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { AgencySubAgencyService } from '../../../core/services/management/agency-sub-agency.service';
import { CustomValidators } from '../../../shared/validators/custom.validator';
import { environment } from '../../../../environments/environment';
import { AgencyEntity, AgencyRequestDTO, CommissionFlatRate } from '../../../shared/models/management/agency-management/agency.request.dto';
import { AgencyDetailsResponseDTO } from '../../../shared/models/management/agency-management/agency-details.response.dto';
import { AgencyData } from './agency.data';
import NotifUtils from '../../../shared/utilities/notif-utils';
import { AgencyAddressEntityAddressDTO } from '../../../shared/models/management/agency-address.dto';
import { AgencyLabelConstants } from '../../../shared/constants/agency.label.constants';
import { AgencyTableConstants } from '../../../shared/constants/agency.table.constants';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { AuthService } from '../../../core/services/auth.service';
import { AuditLogService } from '../../../core/services/management/auditLog.service';
import { AgencySubAgencyLabelConstants } from '../../../shared/constants/agency-sub-agency.label.constants';
import * as moment from 'moment';
const defaultGUID = '00000000-0000-0000-0000-000000000000';

@Injectable({
    providedIn: 'root'
})
export class SubAgencyData extends BaseClass {

    subAgencyId: string;

    SubAgencyTableConstants = SubAgencyTableConstants;
    AgencyLabelConstants = AgencyLabelConstants;
    AgencyTableConstants = AgencyTableConstants;

    subAgencyFormGroup: FormGroup;
    subAgencyFormAddressGroup: FormGroup;
    agencySubAgencyFormGroup: FormGroup;
    flatCommissionFormGroup: FormGroup;
    addressFormGroup: FormGroup;

    addressTypes: AddressTypeDTO[];
    businessTypes: BusinessTypeDTO[];
    genders: GenderDTO[];
    maritalStatuses: MaritalStatusDTO[];
    currentAgencySubAgency: AgencySubAgencyDTO;
    currentSubAgency: SaveSubAgencyDTO;
    agencySubAgencies = new Subject<AgencySubAgencyDTO[]>();
    subAgencies = new Subject<SubAgencyDTO[]>();
    naicsCodes: NAICSCodeDTO[];
    addtionalNaicsCodes: NAICSCodeDTO[];
    subAgencyListData: SubAgencyDTO[] = [];
    addressList: AgencyAddressEntityAddressDTO[] = [];
    subAgencyEntityListData: EntityDTO[] = [];
    subAgencyFlatCommission: CommissionFlatRate[] = [];
    subAgencyScheduleList: any[] = [];
    subAgencyDetails: AgencyDetailsResponseDTO;

    subAgencyListHeader: any[] = [{
        name: this.AgencyTableConstants.agencyName,
        columnName: 'entity.companyName'
    },
    {
        name: this.AgencyTableConstants.agencyContactPerson,
        columnName: 'contactName'
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
        this.SubAgencyTableConstants.newBusiness,
        this.SubAgencyTableConstants.renewalBusiness,
        this.SubAgencyTableConstants.effectiveDate,
        this.SubAgencyTableConstants.expirationDate,
        //   this.AgencyTableConstants.status
    ];

    scheduleHeader: string[] = [
        this.SubAgencyTableConstants.newBusiness,
        this.SubAgencyTableConstants.renewalBusiness,
        this.SubAgencyTableConstants.effectiveDate,
        this.SubAgencyTableConstants.expirationDate
    ];

    addressHeaders: any[] = [
        {
          name: this.AgencyTableConstants.type,
          columnName: 'addressTypeDescription'
        },
        {
          name: this.AgencyTableConstants.city,
          columnName: 'address.city'
        },
        {
          name: this.AgencyTableConstants.state,
          columnName: 'address.stateCode'
        },
        {
          name: this.AgencyTableConstants.zipCode,
          columnName: 'address.zipCode'
        }
    ];

    isSubAgencyTab: boolean = false;
    public isSubAgencyLoading: boolean = false;
    isFlatCommissionLoading: boolean = false;
    isScheduleLoading: boolean = false;
    isAddressLoading: boolean = false;
    isFlatCommisionsUpdated: boolean = false;
    isAddressesUpdated: boolean = false;
    isAddressListEmpty: boolean = false;
    showUpdate: boolean = false;
    showNext: boolean = false;
    showSave: boolean = false;

    subAgencyFlatCommissionRowId: number = 0;
    subAgencyScheduleRowId: number = 0;
    addressListRowId: number = 0;

    licenseEffectiveDateOption: IAngularMyDpOptions;
    licenseExpirationDateOption: IAngularMyDpOptions;


    constructor(private fb: FormBuilder,
        private businessTypeService: BusinessTypeService,
        public genericAddressData: GenericAddressData,
        private programStateSubAgencyService: ProgramStateSubAgencyService,
        private agencySubAgencyService: AgencySubAgencyService,
        public agencyData: AgencyData,
        private auditLogService: AuditLogService,
        private authService: AuthService
    ) {
        super();
    }

    initializeForms() {

        this.businessTypeService.businessTypesAll().takeUntil(this.stop$).subscribe((res) => {
            this.businessTypes = res;
        });

        this.subAgencyFormGroup = this.subAgencyFormSection();
        this.addressFormGroup = this.addressSection();
        this.initDateOptions();
    }

    addressSection(): FormGroup {
        const regex = /[^0-9]*/g;
        return this.fb.group({
            // id: [null, []],
            addressType: new FormControl(null, [Validators.required]),
            effectiveDate: new FormControl(null),
            expirationDate: new FormControl(null),
            address: this.fb.group({
                streetAddress1: new FormControl(''),
                streetAddress2: new FormControl(''),
                zipCode: new FormControl('', [Validators.required, Validators.pattern(regex), CustomValidators.hasNoValue]),
                state: new FormControl({ value: '', disabled: true }, [Validators.required]),
                city: new FormControl('', [Validators.required]),
                isGarageIndoor: new FormControl(false), //Remove all related to garage once API is done
                isGarageOutdoor: new FormControl(false),
                isGarageFenced: new FormControl(false),
                isGarageLighted: new FormControl(false),
                isGarageWithSecurityGuard: new FormControl(false)
            })
        });
    }

    subAgencyFormSection(): FormGroup {
        return this.fb.group({
            id: new FormControl(0),
            companyName: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-ZñÑ0-9 !@#$%^&*‘’“”|()\\-`.+,/\_={}:;~"<>\']*$')]),
            federalIDNumber: new FormControl('', [CustomValidators.federalIdNumberValidator]),
            dba: new FormControl(''),
            contactName: new FormControl('', [Validators.required]),
            workPhone: new FormControl('', [CustomValidators.spaceValidator, CustomValidators.phoneNumberValidator]),
            workFax: new FormControl('', [CustomValidators.spaceValidator, CustomValidators.phoneNumberValidator]),
            workEmailAddress: new FormControl('', [Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,10}$'), Validators.required, CustomValidators.spaceValidator]),

            businessTypeID: new FormControl('', [Validators.required]),

            // naicsCodeID: new FormControl(''),
            // additionalNAICSCodeID: new FormControl(''),
            // iccmcDocketNumber: new FormControl(''),
            // usdotNumber: new FormControl(''),
            // pucNumber: new FormControl(''),

            isEftAvailable: new FormControl(true),
            eftAccountNumber: new FormControl('', []),
            eftAccountType: new FormControl('', []),
            eftRoutingNumber: new FormControl('', []),
            eftCommissionEmail: new FormControl('', []),
            hasBindingAuthority: new FormControl(this.agencyData.agencyFormGroup.get('hasBindingAuthority').value),
            is1099Required: new FormControl(true),
            underwriter: new FormControl(this.agencyData.agencyFormGroup.get('underwriter').value, []),
            regionalSalesManager: new FormControl(this.agencyData.agencyFormGroup.get('regionalSalesManager').value, []),
            agencyManagementSystem: new FormControl(this.agencyData.agencyFormGroup.get('agencyManagementSystem').value),
            commissionType: new FormControl(this.agencyData.agencyFormGroup.get('commissionType').value, []),
            // commissionGroup: new FormControl(this.agencyData.agencyFormGroup.get('commissionGroup').value, []),

            // hasSubsidiaries: new FormControl(true),
            subAgencyCode: new FormControl(''),
            isActive: new FormControl(true),
            lexisNexisCode: new FormControl(''),
            licenseEffectiveDate: new FormControl('', [Validators.required]),
            licenseExpirationDate: new FormControl('', [Validators.required])
        });
    }

    initDateOptions(): void {
        this.licenseEffectiveDateOption = {
            dateRange: false,
            dateFormat: 'mm/dd/yyyy'
        };

        this.licenseExpirationDateOption = {
            dateRange: false,
            dateFormat: 'mm/dd/yyyy'
        };
    }

    getSubAgencies() {
        this.subAgencyListData = [];
        this.subAgencyEntityListData = [];
        this.isSubAgencyLoading = true;
        const agencyId = localStorage.getItem('agencyId');

        this.programStateSubAgencyService.getProgramSubStateAgencyWithAgencyId(this.agencyData.agencyId).pipe(distinctUntilChanged(), takeUntil(this.stop$)).subscribe(data => {

            this.subAgencyListData = data.map(x => x.subAgency).sort((a, b) => a?.entity?.fullName < b?.entity?.fullName ? -1 : 1);
            // console.log(this.subAgencyListData);
            this.subAgencyEntityListData = data.map(agencyItem => {
                return agencyItem.subAgency.entity;
            });
            this.subAgencies.next(this.subAgencyListData);
            this.isSubAgencyLoading = false;
        });
    }

    saveSubAgency(): void {
        const payload: AgencyRequestDTO = this.mapSubAgencyPayload();
        Utils.blockUI();
        this.agencySubAgencyService.postSubAgencyDetails(payload).subscribe(data => {
            Utils.unblockUI();
            this.getSubAgencies();
            this.subAgencyAuditLog(AgencyLabelConstants.auditLog.add, data);
            NotifUtils.showSuccess(AgencyLabelConstants.subAgencyModalAddedSuccessfullyMessage, () => {
                this.subAgencyId = data;
            });
        }, err => {
            Utils.unblockUI();
            this.subAgencyId = null;
        });
    }


    updateSubAgency(subAgencyId: string): void {
        const payload: AgencyRequestDTO = this.mapSubAgencyPayload(true);
        Utils.blockUI();
        this.agencySubAgencyService.putSubAgencyDetails(payload, subAgencyId).subscribe(data => {
            Utils.unblockUI();
            this.getSubAgencies();
            this.subAgencyAuditLog(AgencyLabelConstants.auditLog.edit, data);
            NotifUtils.showSuccess(AgencyLabelConstants.subAgencyModalUpdatedSuccessfullyMessage, () => {
                this.subAgencyId = data;
                this.subAgencyFormGroup.reset();
                this.resetSubAgencyUpdatesTrackers();
            });
        }, err => {
            Utils.unblockUI();
            this.subAgencyId = null;
        });
    }

    deleteSubAgency(id: string): void {
        Utils.blockUI();
        NotifUtils.showConfirmMessage(AgencyLabelConstants.deleteConfirmationMessageModal, () => {
            this.agencySubAgencyService.deleteSubAgencyDetails(id).subscribe(data => {
                Utils.unblockUI();
                const indexAgency = this.subAgencyListData.findIndex(x => x.id === id);
                this.subAgencyListData.splice(indexAgency, 1);
                this.getSubAgencies();
                this.subAgencyAuditLog(AgencyLabelConstants.auditLog.delete, data);
            }, err => {
                Utils.unblockUI();
            });
        });
    }

    mapSubAgencyPayload(isUpdate?: boolean): AgencyRequestDTO {
        return {
            id: isUpdate ? this.subAgencyDetails.id : undefined,
            entityId: isUpdate ? this.subAgencyDetails.entityId : undefined,
            programId: environment.ApplicationId,
            agencyId: this.agencyData.agencyId,
            // stateCode: null,
            createdBy: isUpdate ? this.subAgencyDetails.createdBy : undefined,
            entity: this.mapEntitySubAgencyPayload(isUpdate),
            agencyStatusHistoryId: null,
            contactName: this.subAgencyFormGroup.get('contactName').value,
            isEFTAvailable: this.subAgencyFormGroup.get('isEftAvailable').value,
            eftAccountNumber: this.subAgencyFormGroup.get('eftAccountNumber').value,
            eftAccountTypeId: this.subAgencyFormGroup.get('eftAccountType').value,
            eftRoutingNumber: this.subAgencyFormGroup.get('eftRoutingNumber').value,
            eftEmail: this.subAgencyFormGroup.get('eftCommissionEmail').value,
            hasBindingAuthority: this.subAgencyFormGroup.get('hasBindingAuthority').value,
            licenseEffectiveDate: this.getAdjustedDateISOFormat('licenseEffectiveDate'),
            licenseExpirationDate: this.getAdjustedDateISOFormat('licenseExpirationDate'),
            commisionTypeId: this.subAgencyFormGroup.get('commissionType').value,
            // commissionGroupId: this.subAgencyFormGroup.get('commissionGroup').value !== '' ? this.subAgencyFormGroup.get('commissionGroup').value : defaultGUID,
            agencyManagementSystemId: this.subAgencyFormGroup.get('agencyManagementSystem').value,
            regionalSalesManagerId: this.subAgencyFormGroup.get('regionalSalesManager').value,
            commissionFlatRates: this.mapCommissionFlatRatesAgencyPayload(),
            lexisNexisCode: this.subAgencyFormGroup.get('lexisNexisCode').value,
            subAgencyCode: this.subAgencyFormGroup.get('subAgencyCode').value,
            is1099Required: this.subAgencyFormGroup.get('is1099Required').value,
            isActive: this.subAgencyFormGroup.get('isActive').value,
            uwUserId: Number(this.subAgencyFormGroup.get('underwriter').value)
        };
    }

    mapEntitySubAgencyPayload(isUpdate?: boolean): AgencyEntity {
        return {
            id: isUpdate ? this.subAgencyDetails.entity.id : undefined,
            isIndividual: false,
            firstName: '',
            middleName: '',
            lastName: '',
            companyName: this.subAgencyFormGroup.get('companyName').value,
            fullName: this.subAgencyFormGroup.get('companyName').value,
            homePhone: '',
            mobilePhone: this.subAgencyFormGroup.get('workPhone').value,
            workPhone: this.subAgencyFormGroup.get('workPhone').value,
            workPhone2: this.subAgencyFormGroup.get('workPhone').value,
            workPhoneExtension: '',
            workFax: this.subAgencyFormGroup.get('workFax').value,
            socialSecurityNumber: '',
            personalEmailAddress: '',
            workEmailAddress: this.subAgencyFormGroup.get('workEmailAddress').value,
            // birthDate: null,
            age: 0,
            driverLicenseNumber: '',
            driverLicenseState: '',
            driverLicenseExpiration: null,
            genderID: 0,
            maritalStatusID: 0,
            businessTypeID: Number(this.subAgencyFormGroup.get('businessTypeID').value),
            programId: environment.ApplicationId,
            dba: this.subAgencyFormGroup.get('dba').value,
            federalIDNumber: this.subAgencyFormGroup.get('federalIDNumber').value,
            fein: this.subAgencyFormGroup.get('federalIDNumber').value,
            naicsCodeID: 0, // this.agencyFormGroup.get('naicsCodeID').value,
            additionalNAICSCodeID: 0, // this.agencyFormGroup.get('additionalNAICSCodeID').value,
            iccmcDocketNumber: '', //this.agencyFormGroup.get('iccmcDocketNumber').value,
            usdotNumber: '', // this.agencyFormGroup.get('usdotNumber').value,
            pucNumber: '', // this.agencyFormGroup.get('pucNumber').value,
            isRegionalSalesManager: isUpdate ? this.subAgencyDetails.entity.isRegionalSalesManager : undefined,
            isInternalUser: isUpdate ? this.subAgencyDetails.entity.isInternalUser : undefined,
            isAgent: isUpdate ? this.subAgencyDetails.entity.isAgent : undefined,
            hasSubsidiaries: isUpdate ? this.subAgencyDetails.entity.hasSubsidiaries : undefined, //this.agencyFormGroup.get('hasSubsidiaries').value,
            isActive: this.subAgencyFormGroup.get('isActive').value,
            createdBy: isUpdate ? this.subAgencyDetails.entity.createdBy : undefined,
            entityAddresses: this.addressList,
            deleteAddresses: []
        };
    }

    mapCommissionFlatRatesAgencyPayload(): CommissionFlatRate[] {
        this.subAgencyFlatCommission.forEach(item => {
            if (typeof (item.effectiveDate) !== 'string') {
                item.effectiveDate = this.getAdjustedDateISOFormat('effectiveDate', false, new Date(item.effectiveDate).toLocaleDateString('en-US'));
            }
            if (typeof (item.expirationDate) !== 'string') {
                item.expirationDate = this.getAdjustedDateISOFormat('expirationDate', false, new Date(item.expirationDate).toLocaleDateString('en-US'));
            }
        });
        return this.subAgencyFlatCommission;
    }

    getAgencyDetails(subAgencyId?): void {
        this.agencySubAgencyService.getSubAgencyDetails(subAgencyId).subscribe(data => {
            this.subAgencyDetails = data;
            this.mapSubAgencyDetails(data);
            this.showingOfSaveUpdateNext(true);
        });
    }

    showingOfSaveUpdateNext(isUpdate?: boolean) {
        if (this.checkAddresListComplete(this.addressList) == true) {
            if (this.agencyData.subAgencyActiveTab === AgencyLabelConstants.detailsTab || this.agencyData.subAgencyActiveTab === AgencyLabelConstants.addressTab) {
                if (isUpdate) {
                    this.showUpdate = true;
                    this.showSave = false;
                } else {
                    this.showSave = true;
                    this.showUpdate = false;
                }
                this.showNext = false;
                this.isAddressListEmpty = false;
            }
        } else {
            if (this.agencyData.subAgencyActiveTab === AgencyLabelConstants.detailsTab) {
                this.showUpdate = false;
                this.showSave = false;
                this.showNext = true;

            } else if (this.agencyData.subAgencyActiveTab === AgencyLabelConstants.addressTab) {
                if (isUpdate) {
                    this.showUpdate = true;
                    this.showSave = false;
                } else {
                    this.showSave = true;
                    this.showUpdate = false;
                }
                this.showNext = false;
                this.isAddressListEmpty = true;
            }
        }
    }

    checkAddresListComplete(addList) {
        // Filter address for deletion
        const addressList = addList.filter(x => x?.removedProcessDate == null || x?.removedProcessDate == undefined);
        if (addressList?.length > 0) {
          const hasBusiness = addressList.some(x => x.addressTypeId == 1);
          const hasBilling = addressList.some(x => x.addressTypeId == 2);
          return hasBusiness && hasBilling;
        }
        
        return false;
      }

    mapSubAgencyDetails(data?: AgencyDetailsResponseDTO): void {
        const entity = data?.entity;
        const licenseEffectiveDateValue = new Date(data?.licenseEffectiveDate).toLocaleDateString('en-US');
        const licenseExpirationDateValue = new Date(data?.licenseExpirationDate).toLocaleDateString('en-US');

        this.subAgencyFormGroup.get('companyName').setValue(entity?.companyName);
        this.subAgencyFormGroup.get('federalIDNumber').setValue(entity?.fein);
        this.subAgencyFormGroup.get('dba').setValue(entity?.dba);
        this.subAgencyFormGroup.get('contactName').setValue(data?.contactName);
        this.subAgencyFormGroup.get('workPhone').setValue(entity?.workPhone);
        this.subAgencyFormGroup.get('workFax').setValue(entity?.workFax);
        this.subAgencyFormGroup.get('workEmailAddress').setValue(entity?.workEmailAddress);
        this.subAgencyFormGroup.get('businessTypeID').setValue(entity?.businessTypeID);
        this.subAgencyFormGroup.get('isEftAvailable').setValue(data?.isEFTAvailable);
        this.subAgencyFormGroup.get('eftAccountNumber').setValue(data?.eftAccountNumber);
        this.subAgencyFormGroup.get('eftAccountType').setValue(data?.eftAccountTypeId);
        this.subAgencyFormGroup.get('eftRoutingNumber').setValue(data?.eftRoutingNumber);
        this.subAgencyFormGroup.get('eftCommissionEmail').setValue(data?.eftEmail);
        this.subAgencyFormGroup.get('hasBindingAuthority').setValue(data?.hasBindingAuthority);
        this.subAgencyFormGroup.get('is1099Required').setValue(data?.is1099Required);
        this.subAgencyFormGroup.get('regionalSalesManager').setValue(data?.regionalSalesManagerId);
        this.subAgencyFormGroup.get('agencyManagementSystem').setValue(data?.agencyManagementSystemId);
        this.subAgencyFormGroup.get('commissionType').setValue(data?.commisionTypeId);
        // this.subAgencyFormGroup.get('commissionGroup').setValue(data?.commissionGroupId !== defaultGUID ? data?.commissionGroupId : '');
        // this.agencyData.commissionTypeLogic(this.subAgencyFormGroup);
        this.subAgencyFormGroup.get('isActive').setValue(data?.isActive);
        this.subAgencyFormGroup.get('lexisNexisCode').setValue(data?.lexisNexisCode);
        this.subAgencyFormGroup.get('subAgencyCode').setValue(data?.subAgencyCode);
        this.subAgencyFormGroup.get('licenseEffectiveDate').setValue({ isRange: false, singleDate: { jsDate: new Date(licenseEffectiveDateValue) } });
        this.subAgencyFormGroup.get('licenseExpirationDate').setValue({ isRange: false, singleDate: { jsDate: new Date(licenseExpirationDateValue) } });
        this.subAgencyFormGroup.get('underwriter').setValue(data?.uwUserId);
        this.subAgencyFlatCommissionRowId = data.commissionFlatRates.length - 1;
        let ctr = 0;
        // const commissionFlatRates = data.commissionFlatRates;
        // commissionFlatRates.forEach(cfr => {
        //     cfr.rowId = ctr;
        //     ctr++;
        // });
        // this.subAgencyFlatCommission = commissionFlatRates;

        let addrCtr = 0;
        const addressItems = data.entity.entityAddresses;
        addressItems.forEach(addr => {
            addr.rowId = addrCtr;
            addrCtr++;
        });
        this.addressList = addressItems;
        this.subAgencyFormGroup.markAllAsTouched();
    }

    getAdjustedDateISOFormat(field: string, isSubAgencyDetailsDatePicker: boolean = true, flatComDate?: string): string {
        const date = isSubAgencyDetailsDatePicker ?
            new Date(this.subAgencyFormGroup.get(field).value?.singleDate?.jsDate) :
            new Date(flatComDate);
        const correctDate = new Date(date);
        const formattedDate = moment(new Date(correctDate)).format('YYYY-MM-DD');
        return formattedDate;
    }
    resetSubAgencyUpdatesTrackers(): void {
        this.isFlatCommisionsUpdated = false;
        this.isAddressesUpdated = false;
    }

    subAgencyAuditLog(action: string, entityId: string): void {
        const payload = {
          userId: this.authService.getUserId(),
          keyId: entityId,
          auditType: '',
          description: '',
          method: ''
        };

        switch (action) {
          case AgencyLabelConstants.auditLog.add:
              payload.auditType = AgencyLabelConstants.auditLog.add;
              payload.description = AgencyLabelConstants.subAgencyModalAddedSuccessfullyMessage;
              payload.method = AgencySubAgencyLabelConstants.methodName.add;
            break;
          case AgencyLabelConstants.auditLog.edit:
              payload.auditType = AgencyLabelConstants.auditLog.edit;
              payload.description = AgencyLabelConstants.subAgencyModalUpdatedSuccessfullyMessage;
              payload.method = AgencySubAgencyLabelConstants.methodName.edit;
            break;
          case AgencyLabelConstants.auditLog.delete:
              payload.auditType = AgencyLabelConstants.auditLog.delete;
              payload.description = AgencySubAgencyLabelConstants.subAgencyDeleteMessage;
              payload.method = AgencySubAgencyLabelConstants.methodName.delete;
            break;
        }

        this.auditLogService.insertToAuditLog(payload).subscribe();
    }

}
