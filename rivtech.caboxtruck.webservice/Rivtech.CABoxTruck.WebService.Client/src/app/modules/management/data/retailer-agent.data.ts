import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import * as _ from 'lodash';
import { BaseClass } from '../../../shared/base-class';
import { take, takeUntil } from 'rxjs/operators';
import NotifUtils from '../../../shared/utilities/notif-utils';
import { AgentService } from '../../../core/services/management/agent-service';
import { ProgramStateAgentDTO } from '../../../shared/models/management/agent-management/programStateAgentDto';
import Utils from '../../../shared/utilities/utils';
import { ProgramStateAgentService } from '../../../core/services/management/programstate-agent-service';
import { AgentLabelConstants } from '../../../shared/constants/agent.label.constants';
import { AgencyTableConstants } from '../../../shared/constants/agency.table.constants';
import { AgentUserResponseDTO } from '../../../shared/models/data/dto/agent/agentuser-response.dto';
import { SubAgencyData } from './sub-agency.data';
import { SubAgencyDTO } from '../../../shared/models/management/agency-management/sub-agencyDto';
import { AgentLicenses, SaveAgentDTO } from '../../../shared/models/management/agent-management/save-agentDto';
import { AgencyEntity } from '../../../shared/models/management/agency-management/agency.request.dto';
import { environment } from '../../../../environments/environment';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { UserService } from '../../../core/services/management/user.service';
import { UserViewModel } from '../../../shared/models/management/user-view-model';
import { CustomValidators } from '../../../shared/validators/custom.validator';
import { ProgramStateData } from './program-state.data';
import { Subject } from 'rxjs';
import { AgencyData } from './agency.data';
import { AgencyLabelConstants } from '../../../shared/constants/agency.label.constants';
import { AuditLogService } from '../../../core/services/management/auditLog.service';
import { AuthService } from '../../../core/services/auth.service';
import * as moment from 'moment';
import { UserNameValidator } from './validators/usernameValidator';
import { RetailerAgentService } from '@app/core/services/management/retailer-agent.service';
import { RetailerData } from './retailer.data';
const Business = 2;
@Injectable({
    providedIn: 'root'
})
export class RetailerAgentData extends BaseClass {
    AgentLabelConstants = AgentLabelConstants;
    AgencyTableConstants = AgencyTableConstants;
    AgencyLabelConstants = AgencyLabelConstants;

    agentId: string;

    agentFormGroup: FormGroup;
    agentLicenseFormGroup: FormGroup;

    effectiveDateOption: IAngularMyDpOptions;

    agentListHeaders: any[] = [{
      name: this.AgentLabelConstants.name,
      columnName: 'entity.fullName'
    },
    {
      name: this.AgentLabelConstants.email,
      columnName: 'entity.workEmailAddress'
    },
    {
      name: this.AgentLabelConstants.workphone,
      columnName: 'entity.workPhone'
    },
    {
      name: this.AgentLabelConstants.status,
      columnName: 'entity.isActive'
    }];

    agentSubAgenciesListHeaders: any[] = [{
    name: this.AgentLabelConstants.linked,
    columnName: 'linked'
    },
    {
      name: this.AgentLabelConstants.nameCity,
      columnName: 'entity.companyName'
    },
    {
      name: this.AgentLabelConstants.officeAddress,
      columnName: 'fullAddress'
    },
    {
      name: this.AgentLabelConstants.primary,
      columnName: 'isPrimary'
    }];

    retailerAgencyListHeaders: any[] = [{
      name: this.AgentLabelConstants.linked,
      columnName: 'linked'
      },
      {
        name: this.AgentLabelConstants.name,
        columnName: 'entity.companyName'
      },
      {
        name: this.AgentLabelConstants.contact,
        columnName: 'entity.contactName'
      },
    ];

    agentLicenseListHeaders: any[] = [{
    name: this.AgentLabelConstants.state,
    columnName: 'stateCode'
    },
    {
      name: this.AgentLabelConstants.effectiveDate,
      columnName: 'licenseEffectiveDate'
    },
    {
      name: this.AgentLabelConstants.expirationDate,
      columnName: 'licenseExpirationDate'
    }];

    // object
    agentDetails: AgentUserResponseDTO;

    agentList: ProgramStateAgentDTO[] = [];
    agentSubAgenciesList: SubAgencyDTO[] = [];
    agentLicenseList: any[] = [];

    isAgentLoading: boolean = false;
    isAgentSubAgenciesLoading: boolean = false;
    isAgentLicenseLoading: boolean = false;
    isAgentLicensesUpdated: boolean = false;
    isAddressesUpdated: boolean = false;
    isSubAgenciesUpdated: boolean = false;
    isProgramStatesUpdated: boolean = false;

    agentRowId: number = 0;
    agentSubAgenciesRowId: number = 0;
    agentLicenseRowId: number = 0;

    isUserExist: Subject<boolean> = new Subject<boolean>();
    userId: number = null;
    tempUsername: string = '';

    constructor(
        public agentService: AgentService,
        public retailerAgentService: RetailerAgentService,
        private programStateAgentService: ProgramStateAgentService,
        public subAgencyData: SubAgencyData,
        public agencyData: AgencyData,
        public retailerData: RetailerData,
        private userService: UserService,
        private programStateData: ProgramStateData,
        private auditLogService: AuditLogService,
        private authService: AuthService,
        private userNameValidator: UserNameValidator) {
        super();
    }

  //#region  =======> Form Groups

  agentSection(): FormGroup {
    return new FormGroup({
      username: new FormControl('', []),
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      isSystemUser: new FormControl(false, []),
      isActive: new FormControl(false, []),
      emailAddress: new FormControl('', [Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,10}$'), Validators.required, CustomValidators.spaceValidator,Validators.maxLength(100)]),
      workPhone: new FormControl('', [CustomValidators.spaceValidator, CustomValidators.phoneNumberValidator]),
      workFax: new FormControl('', [CustomValidators.spaceValidator, CustomValidators.phoneNumberValidator]),
      oldUserName: new FormControl(''),
      torrentUsername: new FormControl('', [CustomValidators.spaceValidator])
    });
  }

  agentLicenseSection(): FormGroup {
    return new FormGroup({
      stateCode: new FormControl('', [Validators.required]),
      licenseNumber: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z0-9]+$')]),
      licenseEffectiveDate: new FormControl('', [Validators.required]),
      licenseExpirationDate: new FormControl('', [Validators.required])
    });
  }
  //#endregion

  //#region  =======> Initializations

  initializeForms() {
    this.agentFormGroup = this.agentSection();
    this.agentLicenseFormGroup = this.agentLicenseSection();

    this.effectiveDateOption = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };
  }

  resetAgent(): void {
    this.agentLicenseList = [];
    this.agentSubAgenciesList = [];
    this.agentLicenseRowId = 0;
    this.agentSubAgenciesRowId = 0;
  }

  callAgentAPIonLoad(): void {
    this.getAgentList(this.retailerData.agencyId);
  }

  populateAgent(data: AgentUserResponseDTO) {
    this.agentFormGroup.get('username').clearAsyncValidators();
    this.agentFormGroup.get('username').updateValueAndValidity();
    this.agentFormGroup.get('username').setValue(data?.entity?.userName ?? '');
    this.agentFormGroup.get('firstName').setValue(data?.entity?.firstName ?? '');
    this.agentFormGroup.get('lastName').setValue(data?.entity?.lastName ?? '');
    this.agentFormGroup.get('firstName').setValue(data?.entity?.firstName ?? '');
    this.agentFormGroup.get('emailAddress').setValue(data?.entity?.workEmailAddress ?? '');
    this.agentFormGroup.get('isSystemUser').setValue(data?.isSystemUser);
    this.agentFormGroup.get('isActive').setValue(data?.entity?.isActive);
    this.agentFormGroup.get('workPhone').setValue(data?.entity?.workPhone);
    this.agentFormGroup.get('workFax').setValue(data?.entity?.workFax);
    this.agentFormGroup.get('oldUserName').setValue(data?.entity?.userName ?? '');
    this.userNameValidator.oldUserName =  data?.entity?.userName ?? '';
    this.agentFormGroup.markAllAsTouched();
    this.agentFormGroup.get('username').setAsyncValidators(this.userNameValidator.usernameValidator());
    this.systemUserLogic(true);

    this.mapSubAgency(data);

    // Map Row Id License
    this.agentLicenseRowId = data?.agentLicenses?.length - 1;
    let licCtr = 0;
    const licenseItem = data.agentLicenses;
    licenseItem.forEach(license => {
      license.rowId = licCtr;
      licCtr++;
    });

    this.agentLicenseList = licenseItem;

    // Map Program State
    const programStateIds = data.programStateIds;
    this.programStateData.programStateList.map(programState => {
      if (programStateIds.includes(programState.programStateId)) {
        programState.isSelected = true;
      } else {
        programState.isSelected = false;
      }
    });
  }

  //#region =====> Methods on Calling API

  getAgent(agentId): void {
    this.agentId = agentId;
    this.resetAgent();
    this.isAgentSubAgenciesLoading = true;
    this.retailerAgentService.getAgentInformation(agentId).pipe(takeUntil(this.stop$)).subscribe((data: AgentUserResponseDTO) => {
        this.agentDetails = data;
        this.populateAgent(data);
        this.isAgentSubAgenciesLoading = false;
        this.setUserId();
    }, err => {
      NotifUtils.showError(JSON.stringify(err));
      this.isAgentSubAgenciesLoading = false;
    });
  }

  getAgentList(agencyId?: string, subagencyId?: string, searchString: string = null): void {
    this.agentList = [];
    this.isAgentLoading = true;
    Utils.blockUI();
    this.retailerAgentService.getAllAgents(agencyId).pipe(takeUntil(this.stop$)).subscribe((data) => {
        if (data.length > 0) {
          this.agentList = data.sort((a, b) => (a?.entity?.fullName.localeCompare(b?.entity?.fullName)) ? -1 : 1);

          if (searchString !== null) {
            this.agentList = this.searchRecords(data, searchString.toLowerCase()).sort((a, b) => (a?.entity?.fullName.localeCompare(b?.entity?.fullName)) ? -1 : 1);
          }
        }
        this.isAgentLoading = false;
        Utils.unblockUI();
    }, err => {
        Utils.unblockUI();
        NotifUtils.showError(JSON.stringify(err));
        this.isAgentLoading = false;
    });
  }

  searchRecords(data, searchString) {
    const filteredData = data.filter(e => (e?.entity?.fullName +
                                      e?.entity?.workEmailAddress +
                                      e?.entity?.workPhone +
                                      (e?.entity?.isActive ? 'Active' : 'Inactive')
                                    ).toLowerCase().search(searchString.toLowerCase()) > -1);

     return filteredData;
  }

  saveUserAndAgent(retailerId): void {
    const agentPayload = this.getAgentPayload();
    Utils.blockUI();
    agentPayload.retailerId = retailerId;
    this.saveAgent(agentPayload);
  }

  saveAgent(agentPayload?: SaveAgentDTO): void {
    this.retailerAgentService.postAgent(agentPayload).subscribe(res => {
      Utils.unblockUI();
      NotifUtils.showSuccess(AgentLabelConstants.agentModalAddedSuccessfullyMessage, () => {
        this.isUserExist.next(false);
        this.callAgentAPIonLoad();
      });
    }, err => {
      Utils.unblockUI();
    });
  }

  updateAgent(agentPayload?: SaveAgentDTO, isSystemUser?: boolean, isFromPost?: boolean, userId?: string): void {
    this.retailerAgentService.putAgent(agentPayload, agentPayload.id).subscribe(res => {
      Utils.unblockUI();
      NotifUtils.showSuccess(AgentLabelConstants.agentModalUpdatedSuccessfullyMessage, () => {
        this.getAgent(this.agentId);
        this.callAgentAPIonLoad();
        this.resetAgentUpdatesTrackers();
        this.isUserExist.next(false);
      });
    }, err => {
      Utils.unblockUI();
    });
  }

  updateUserAndAgent(): void {
    const agentPayload = this.getAgentPayload(true);
    const isSystemUser = this.agentFormGroup.get('isSystemUser').value;
    Utils.blockUI();
    this.updateAgent(agentPayload, isSystemUser, false);
  }

  getAgentPayload(isUpdate?: boolean): SaveAgentDTO {
    const subAgencyIds: Array<string> = [];
    const programStateIds: Array<number> = [];

    this.programStateData.programStateList.forEach(x => {
      if (x.isSelected) {
        programStateIds.push(x.programStateId);
      }
    });

    this.agentSubAgenciesList.forEach(val => {
      if (val.linked) {
        subAgencyIds.push(val.id);
      }
    });

    const saveAgentPayload: SaveAgentDTO = {
      id: isUpdate ? this.agentDetails?.id : undefined,
      entityId: isUpdate ? this.agentDetails?.entityId : undefined,
      programId: environment.ApplicationId,
      // stateCode: '',
      isActive: this.agentFormGroup.get('isActive').value,
      createdBy: this.agentDetails?.createdBy,
      agencyId: this.subAgencyData.agencyData.agencyId,
      subAgencyId: this.agentSubAgenciesList?.find(x => x.isPrimary)?.id,
      isSystemUser: this.agentFormGroup.get('isSystemUser').value,
      subAgencyIds: subAgencyIds,
      programStateIds: programStateIds,
      agentLicenses: this.getAdjustedDateFormat(this.agentLicenseList),
      createdDate: this.agentDetails?.createdDate,
      entity: this.mapEntityPayload(isUpdate)
    };

    return saveAgentPayload;
  }

  mapEntityPayload(isUpdate?: boolean): AgencyEntity {
    const entity = this.agentDetails?.entity;
    return {
      id: isUpdate ? entity?.id : undefined,
      isIndividual: this.agentDetails?.entity?.isIndividual,
      userName: this.agentFormGroup.get('username').value,
      firstName: this.agentFormGroup.get('firstName').value,
      middleName: entity?.middleName,
      lastName: this.agentFormGroup.get('lastName').value,
      companyName: entity?.companyName,
      homePhone: entity?.homePhone,
      mobilePhone: entity?.mobilePhone,
      workPhone: this.agentFormGroup.get('workPhone').value,
      workPhone2: entity?.workPhone,
      workPhoneExtension: entity?.workPhoneExtension,
      workFax: this.agentFormGroup.get('workFax').value,
      socialSecurityNumber: entity?.socialSecurityNumber,
      personalEmailAddress: entity?.personalEmailAddress,
      workEmailAddress: this.agentFormGroup.get('emailAddress').value,
      birthDate: entity?.birthDate,
      age: entity?.age,
      driverLicenseNumber: entity?.driverLicenseNumber,
      driverLicenseState: entity?.driverLicenseState,
      driverLicenseExpiration: entity?.driverLicenseExpiration,
      genderID: entity?.genderID,
      maritalStatusID: entity?.maritalStatusID,
      businessTypeID: entity?.businessTypeID,
      programId: environment.ApplicationId,
      dba: entity?.dba,
      yearEstablished: entity?.yearEstablished,
      federalIDNumber: entity?.federalIDNumber,
      fein: entity?.fein,
      naicsCodeID: entity?.naicsCodeID,
      additionalNAICSCodeID: entity?.additionalNAICSCodeID,
      iccmcDocketNumber: entity?.iccmcDocketNumber,
      usdotNumber: entity?.usdotNumber,
      pucNumber: entity?.pucNumber,
      isRegionalSalesManager: entity?.isRegionalSalesManager,
      isInternalUser: entity?.isInternalUser,
      isAgent: entity?.isAgent,
      hasSubsidiaries: entity?.hasSubsidiaries,
      isActive: this.agentFormGroup.get('isActive').value,
      createdBy: entity?.createdBy,
      deleteAddresses: entity?.deleteAddresses,
      torrentUsername: ''
    };
  }

  mapSubAgency(data?): void {
    const subAgencyList = this.subAgencyData.subAgencyListData;
    let subAgenciesCtr = 0;
    // HAYDEN SUBAGENCY
    this.agentSubAgenciesList = subAgencyList.map(subAgency => {
      if (data?.subAgencyId === subAgency.id) {
        subAgency.isPrimary = true;
      } else {
        subAgency.isPrimary = false;
      }

      if (data?.subAgencyIds.includes(subAgency.id)) {
        subAgency.linked = true;
      } else {
        subAgency.linked = false;
      }

      subAgency.entity?.entityAddresses?.map(address => {
        if (address?.addressTypeId === Business) {
          subAgency.fullAddress = `${address?.address?.streetAddress1} ${address?.address?.streetAddress2} ${address?.address?.city} ${address?.address?.zipCode}`;
          subAgency.subAgencyCity = `(${address?.address?.city})`;
        }
      });

      subAgency.rowId = subAgenciesCtr;
      subAgenciesCtr++;

      return subAgency;
    }).filter(x => x.entity.isActive);
  }

  mapUserInformation(isNew: boolean): UserViewModel {
    const request: UserViewModel = {
      userName: this.agentFormGroup.get('username').value,
      firstName: this.agentFormGroup.get('firstName').value,
      lastName: this.agentFormGroup.get('lastName').value,
      fullName: `${this.agentFormGroup.get('firstName').value} ${this.agentFormGroup.get('lastName').value}`,
      appCode: environment.ApplicationId,
      carrierCode: null,
      agencyId: this.subAgencyData.agencyData.agencyId,
      subAgencyId: this.agentSubAgenciesList?.find(x => x.isPrimary)?.id,
      emailAddress: this.agentFormGroup.get('emailAddress').value,
      isAgent: this.agentDetails?.entity?.isAgent ?? true,
      isSystemUser: this.agentFormGroup.get('isSystemUser').value,
      isAgencyAdmin: false,
      isSubAgencyAdmin: false,
      isInternal: false,
      isActive: this.agentFormGroup.get('isSystemUser').value ? this.agentFormGroup.get('isActive').value : false,
      roleId: 4, // Hardcode for test member
      userAccessRights: null,
      torrentUserName: this.agentFormGroup.get('torrentUsername').value
    };

    return request;
  }

  getAdjustedDateFormat(licenseList: AgentLicenses[]): AgentLicenses[]  {

    if (licenseList.length > 0){
      return licenseList.map(item => {
        item.licenseEffectiveDate = moment(item.licenseEffectiveDate).format('YYYY-MM-DD');
        item.licenseExpirationDate =  moment(item.licenseExpirationDate).format('YYYY-MM-DD');
        return item;
      });
    }
  }
  //#endregion

  //#region =====> Logics
    systemUserLogic(isEdit?: boolean): void {
      const systemUser = this.agentFormGroup.get('isSystemUser');
      const username = this.agentFormGroup.get('username');
      if (systemUser.value || username.value !== '' && isEdit) {
        username.setValidators(Validators.required);

      } else {
        username.setValue('');
        username.clearValidators();
        username.enable();

      }
      username.updateValueAndValidity();
    }
  //#endregion

  resetAgentUpdatesTrackers(): void {
    this.isAgentLicensesUpdated = false;
    this.isAddressesUpdated = false;
    this.isSubAgenciesUpdated = false;
    this.isProgramStatesUpdated = false;
  }

  agentAuditLog(action: string, entityId: string): void {
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
          payload.description = AgentLabelConstants.agentModalAddedSuccessfullyMessage;
          payload.method = AgentLabelConstants.methodName.add;
        break;
      case AgencyLabelConstants.auditLog.edit:
          payload.auditType = AgencyLabelConstants.auditLog.edit;
          payload.description = AgentLabelConstants.agentModalUpdatedSuccessfullyMessage;
          payload.method = AgentLabelConstants.methodName.edit;
        break;
      case AgencyLabelConstants.auditLog.delete:
          payload.auditType = AgencyLabelConstants.auditLog.delete;
          payload.description = AgentLabelConstants.agentDeleteMessage;
          payload.method = AgentLabelConstants.methodName.delete;
        break;
    }

    this.auditLogService.insertToAuditLog(payload).subscribe();
  }

  saveUserInfo(agentPayload: SaveAgentDTO, userId: string, agentId: string, isLoggedIn?: boolean): void {
    const payload = {
      userInfo: {
        userId: userId,
        fullName: `${agentPayload.entity?.firstName} ${agentPayload.entity?.lastName}`,
        agentId: agentId,
        agencyId: agentPayload.agencyId,
        subAgencyId: agentPayload.subAgencyId,
        isLoggedIn: isLoggedIn ? true : false
      }
    };

    this.authService.saveUserInfo(payload).pipe(take(1)).subscribe();
  }

  getUserInfo(agentPayload: SaveAgentDTO, userId: string, agentId: string): void {
      this.userService.getUserInfoById(userId).pipe(takeUntil(this.stop$)).subscribe(result => {
        Utils.unblockUI();
        this.saveUserInfo(agentPayload, userId, agentId, result.isLoggedIn);
      }, (error) => {
        Utils.unblockUI();
      });
    }

  setUserId(): void {
    if(this.agentDetails.entity.userName){
      Utils.blockUI();
      this.userService.getSpecificUser(environment.ApplicationId, this.agentDetails.entity.userName).subscribe(a => {
        Utils.unblockUI();
        this.userId = a.userId;
        this.tempUsername = this.agentDetails.entity.userName;
        this.agentFormGroup.get('torrentUsername').setValue(a.torrentUserName);
      }, err => {
        NotifUtils.showError(err.message);
        Utils.unblockUI();
      });
    }
  }
}
