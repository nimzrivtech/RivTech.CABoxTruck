/* eslint-disable radix */
/* eslint-disable no-shadow */
import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { take, takeUntil } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { Subject } from 'rxjs';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import * as _ from 'lodash';
import { ToastrService } from 'ngx-toastr';
import { ApplicationService } from '../../../core/services/management/application.service';
import { RoleGroup } from '../../../shared/models/management/role-group';
import NotifUtils from '../../../shared/utilities/notif-utils';
import { BaseClass } from '../../../shared/base-class';
import { UserAccessRight } from '../../../shared/models/management/user';
import { UserViewModel } from '../../../shared/models/management/user-view-model';
import { UserRoleInfo } from '../../../shared/models/management/user-role-info';
import { AddressType, City, Gender, MaritalStatus, State, TableFormcontrols } from '../../../shared/constants/user.options.constants';
import { ITableFormControl, ITableTd, ITableTh, ITableTr } from '../../../shared/models/dynamic/table.interface';
import { UsersLabelConstants, UserTableLabelConstants, UserTypeLabelConstants } from '../../../shared/constants/user.label.constants';
import { UserService } from '../../../core/services/management/user.service';
import { RoleService } from '../../../core/services/management/role.service';
import { Role } from '../../../shared/models/management/role';
import { CommonService } from '../../../core/services/common.service';
import { UserType } from '../../../shared/models/management/user-type';
import { GenericLabel } from '../../../shared/constants/generic.labels.constants';
import { LineOfBusinessDTO } from '../../../shared/models/management/line-of-business-dto';
import Utils from '../../../shared/utilities/utils';
import { EntityDTO } from '../../../shared/models/management/agency-management/entityDto';
import { GenericAddressData } from '../../../shared/components/generic-address/data/generic-address.data';
import { AddressDTO } from '../../../shared/models/management/addressDto';
import { EntityAddressDTO } from '../../../shared/models/management/EntityAddressDto';
import { EntityService } from '../../../core/services/management/entity.service';
import * as moment from 'moment';
import { CustomValidators } from '../../../shared/validators/custom.validator';
import { AuthService } from 'app/core/services/auth.service';
import { AuditLogService } from '../../../core/services/management/auditLog.service';
import { UserNameValidator } from './validators/usernameValidator';
import { BoxtruckAdminRole, UserRole } from '@app/shared/constants/user-roles.constants';

@Injectable({
  providedIn: 'root'
})
export class UserData extends BaseClass {

  UserInfoFormGroup: FormGroup;
  userTypeFormGroup: FormGroup;

  currentUserRole: UserRoleInfo;
  currUserAccessRights: UserAccessRight[];

  // Date Options
  birthdateOption: IAngularMyDpOptions;

  // Dropdown Values
  maritalStatusList: any[] = [];
  genderList: any[] = [];
  addressTypeList: any[] = [];
  stateList: any[] = [];
  cityList: any[] = [];
  roleList: any[] = [];

  tableFormControls: ITableFormControl[] = TableFormcontrols;

  userTableHeaders: any[] = [
    { name: UserTableLabelConstants.userName, columnName: 'userName' },
    { name: UserTableLabelConstants.name, columnName: 'fullName' },
    { name: UserTableLabelConstants.email, columnName: 'emailAddress' },
    { name: UserTableLabelConstants.status, columnName: 'isActive' }
  ];

  userTypeTableHeaders: ITableTh[] = [
    { value: UserTableLabelConstants.accessCategory },
    { value: UserTableLabelConstants.userType },
    { value: UserTableLabelConstants.status }
  ];

  userTypeTableRows: ITableTr[] = [];
  userTypeList: UserType[] = [];
  userCategories: RoleGroup[] = [];

  editId: any;
  deleteAddresses: any[];

  isSystemUserSuccess: boolean = false;
  isTypeNameNotFound = new Subject<boolean>();
  seniorRoleManagerCode: number;
  userTypeLabelConstants = UserTypeLabelConstants;
  userListData: UserViewModel[] = [];

  isUsersLoading: boolean = false;

  isExisting: Subject<boolean> = new Subject<boolean>();

  userId: number = null;
  isAccountLocked: boolean = false;

  constructor(
    private applicationService: ApplicationService,
    private userService: UserService,
    private role: RoleService,
    private common: CommonService,
    private genericAddressData: GenericAddressData,
    private entityService: EntityService,
    private toastr: ToastrService,
    private authService: AuthService,
    private auditLogService: AuditLogService,
    private userNameValidator: UserNameValidator) {
    super();
  }

  initializeForms(): void {
    this.UserInfoFormGroup = this.userInformation();
    this.userTypeFormGroup = this.userType();
    this.dropdownValues();
    this.dateFormatOptions();

  }

  userInformation(): FormGroup {
    return new FormGroup({
      firstName: new FormControl('', [Validators.required, CustomValidators.spaceValidator]),
      middleName: new FormControl('', []),
      lastName: new FormControl('', [Validators.required, CustomValidators.spaceValidator]),
      homePhone: new FormControl('', [CustomValidators.spaceValidator, CustomValidators.phoneNumberValidator]),
      mobilePhone: new FormControl('', [CustomValidators.spaceValidator, CustomValidators.phoneNumberValidator]),
      workPhone: new FormControl('', [CustomValidators.spaceValidator, CustomValidators.phoneNumberValidator]),
      workPhoneExtension: new FormControl('', []),
      personalEmailAddress: new FormControl('', [Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,10}$')]),
      workEmailAddress: new FormControl('', [Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,10}$'), Validators.required, CustomValidators.spaceValidator]),
      birthdate: new FormControl('', []),
      age: new FormControl({ value: '', disabled: true }, []),
      gender: new FormControl('', []),
      maritalStatus: new FormControl('', []),
      federalIdNumber: new FormControl('', []),
      isActive: new FormControl(false, [Validators.required]),
      userName: new FormControl('',  {  validators: [Validators.required, Validators.maxLength(100)] , asyncValidators: [] , updateOn:'blur' }),
      role: new FormControl(null, [Validators.required]),
      oldUserName: new FormControl(''),
      torrentUsername: new FormControl('', [CustomValidators.spaceValidator]),
      isLocked: new FormControl(false),
    });
  }

  private userType(): FormGroup {
    return new FormGroup({
      accessCategory: new FormControl('', [Validators.required]),
      typeName: new FormControl('', [Validators.required]),
      isInternal: new FormControl(false),
      isActive: new FormControl(false),
    });
  }

  roleGroup(): FormGroup {
    return new FormGroup({
      roleGroupId: new FormControl('', [Validators.required]),
      roleId: new FormControl('', [Validators.required])
    });
  }

  populateUserInformation(data) {
    this.UserInfoFormGroup.get('userName').patchValue(data.userName);
    this.UserInfoFormGroup.get('personalEmailAddress').patchValue(data.emailAddress);
    this.UserInfoFormGroup.get('role').patchValue(data.roleId);
    this.UserInfoFormGroup.get('lastName').patchValue(data.lastName);
    this.UserInfoFormGroup.get('firstName').patchValue(data.firstName);
    this.UserInfoFormGroup.get('isActive').patchValue(data.isActive);
    this.UserInfoFormGroup.get('oldUserName').setValue(data.userName);
    this.UserInfoFormGroup.get('isLocked').setValue(data.isLocked);
    this.userNameValidator.oldUserName = data?.userName;

    this.isAccountLocked = data.isLocked;
    this.UserInfoFormGroup.get('isLocked').disable();
    let user_role = localStorage.getItem(environment.CurrentUserRole);
    if (user_role == BoxtruckAdminRole.BoxtruckAdmin ||
      user_role == UserRole.Underwriter || user_role == UserRole.Admin) {
      if (this.UserInfoFormGroup.get('isLocked').value == true) {
        this.UserInfoFormGroup.get('isLocked').enable();
      }
    }
  }

  populateUserInformationEntity(data) {
    this.editId = data.id;
    this.UserInfoFormGroup.get('workEmailAddress').patchValue(data.workEmailAddress);
    this.UserInfoFormGroup.get('middleName').patchValue(data.middleName);
    this.UserInfoFormGroup.get('age').patchValue(data.age);
    this.UserInfoFormGroup.get('birthdate').patchValue(data.birthDate ? { isRange: false, singleDate: { jsDate: new Date(data.birthDate) } } : null);
    this.UserInfoFormGroup.get('gender').patchValue(data.genderID);
    this.UserInfoFormGroup.get('maritalStatus').patchValue(data.maritalStatusID);
    this.UserInfoFormGroup.get('workPhone').patchValue(data.workPhone);
    this.UserInfoFormGroup.get('homePhone').patchValue(data.homePhone);
    this.UserInfoFormGroup.get('workPhoneExtension').patchValue(data.workPhoneExtension);
    this.UserInfoFormGroup.get('mobilePhone').patchValue(data.mobilePhone);
    this.UserInfoFormGroup.get('federalIdNumber').patchValue(data.federalIDNumber);

    data.entityAddresses.forEach(x => {
      if (x.removedProcessDate == null) {
        const address = [];
        const oldAddress = {
          id: x.address.id,
          streetAddress1: x.address.streetAddress1,
          streetAddress2: x.address.streetAddress2,
          cityZipCodeID: x.address.zipCode,
          zipCode: x.address.zipCode,
          city: x.address.city,
          stateCode: x.address.stateCode,
          isGarageIndoor: x.address.isGarageIndoor,
          isGarageOutdoor: x.address.isGarageOutdoor,
          isGarageFenced: x.address.isGarageFenced,
          isGarageLighted: x.address.isGarageLighted,
          isGarageWithSecurityGuard: x.address.isGarageWithSecurityGuard
        };
        address.push(oldAddress);

        const entityAddress = {
          id: x.id,
          addressTypeId: x.addressTypeId.toString(),
          effectiveDate: x.effectiveDate,
          expirationDate: x.expirationDate,
          address: address
        };

        this.genericAddressData.addressList.push(entityAddress);
        this.genericAddressData.addTableItem(entityAddress);
      }
    });
  }

  dropdownValues(): void {
    this.maritalStatusList = MaritalStatus;
    this.genderList = Gender;
    this.addressTypeList = AddressType;
    this.stateList = State;
    this.cityList = City;
  }

  dateFormatOptions(): void {
    this.birthdateOption = {
      dateFormat: 'mm/dd/yyyy',
      disableSince: {
        year: this.authService.getCustomDate().getFullYear(),
        month: this.authService.getCustomDate().getMonth() + 1,
        day: this.authService.getCustomDate().getDate() + 1,
      },
    };
  }

  markAllControlsAsDirty(form: FormGroup) {
    Object.keys(form.controls).forEach(key => {
      form.get(key).markAsDirty();
    });
  }


  //#region SYSTEM USER
  postUserInformation(): void {
    this.userService.postUser(this.mapUserInformation(true)).subscribe((res) => { //UPDATE MODEL
      Utils.unblockUI();
      // this.userAuditLog(UsersLabelConstants.auditLog.add, res);
      // this.saveUserInfo(res);
      NotifUtils.showSuccess(UsersLabelConstants.userModalAddedSuccessfullyMessage, () => {
        this.isSystemUserSuccess = true;
        this.isExisting.next(false);
        this.saveEntityInfo(false); // Save to Agency Services Entity
        this.getUserList();
      });
    }, error => {
      if (error.error.includes('exists')) {
        this.isExisting.next(true);
      }
      Utils.unblockUI();
      NotifUtils.showError(error.error);
    });
  }

  mapUserInformation(isNew: boolean): UserViewModel {
    const request: UserViewModel = {
      userId: 0,
      userName: this.UserInfoFormGroup.get('userName').value,
      firstName: this.UserInfoFormGroup.get('firstName').value,
      lastName: this.UserInfoFormGroup.get('lastName').value,
      fullName: `${this.UserInfoFormGroup.get('firstName').value} ${this.UserInfoFormGroup.get('lastName').value}`,
      appCode: environment.ApplicationId,
      carrierCode: null,
      agencyId: null,
      subAgencyId: null,
      emailAddress: this.UserInfoFormGroup.get('workEmailAddress').value,
      isAgent: false,  //True?
      isSystemUser: true,
      isAgencyAdmin: false,
      isSubAgencyAdmin: false,
      isInternal: true, //UPDATE
      isActive: this.UserInfoFormGroup.get('isActive').value,
      roleId: this.UserInfoFormGroup.get('role').value,
      userAccessRights: null,
      oldUserName: this.UserInfoFormGroup.get('userName').value,
      torrentUserName: this.UserInfoFormGroup.get('torrentUsername').value
    };

    if(!isNew){
      request.userId = this.userId;
      request.oldUserName = this.UserInfoFormGroup.get('oldUserName').value;
    }

    return request;
  }

  putUserInformation(): void {
    this.userService.putUser(this.mapUserInformation(false)).subscribe((res) => {
      Utils.unblockUI();
      // this.userAuditLog(UsersLabelConstants.auditLog.edit, JSON.parse(res).userId);
      // this.getUserInfo(JSON.parse(res).userId);
      NotifUtils.showSuccess(UsersLabelConstants.userModalUpdatedSuccessfullyMessage, () => {
        this.isSystemUserSuccess = true;
        this.isExisting.next(false);
        this.saveEntityInfo(true);
        this.getUserList();
      });
    }, error => {
      const errMessage = JSON.parse(error.error).error;
      if (errMessage.includes('exists')) {
        this.isExisting.next(true);
      } else {
        this.isExisting.next(false);
      }
      Utils.unblockUI();
      NotifUtils.showError(errMessage);
    });
  }

  deleteUser(userId): void {
    this.userService.deleteUser(userId).subscribe(() => {
      this.getUserList();
    });
  }

  saveEntityInfo(isEdit: boolean): void {
    const entityData = new EntityDTO({
      appCode: environment.ApplicationId,
      isIndividual: true, //Default since Entity is Company not a Person
      companyName: null,
      firstName: this.UserInfoFormGroup.get('firstName').value,
      lastName: this.UserInfoFormGroup.get('lastName').value,
      middleName: this.UserInfoFormGroup.get('middleName').value,
      fullName: `${this.UserInfoFormGroup.get('firstName').value} ${this.UserInfoFormGroup.get('lastName').value}`,
      mobilePhone: this.UserInfoFormGroup.get('mobilePhone').value,
      homePhone: this.UserInfoFormGroup.get('homePhone').value,
      workPhone: this.UserInfoFormGroup.get('workPhone').value,
      workPhoneExtension: this.UserInfoFormGroup.get('workPhoneExtension').value,
      workFax: '',
      personalEmailAddress: this.UserInfoFormGroup.get('personalEmailAddress').value,
      workEmailAddress: this.UserInfoFormGroup.get('workEmailAddress').value,
      businessTypeID: null,
      dba: '',
      yearEstablished: 0,
      naicsCodeID: 0,
      additionalNAICSCodeID: 0,
      createdBy: 0,
      birthDate: moment(new Date(this.UserInfoFormGroup.get('birthdate').value?.singleDate.jsDate)).add(1, 'days').toISOString(),
      age: Number(this.UserInfoFormGroup.get('age').value),
      genderID: Number(this.UserInfoFormGroup.get('gender').value),
      maritalStatusID: Number(this.UserInfoFormGroup.get('maritalStatus').value),
      federalIDNumber: this.UserInfoFormGroup.get('federalIdNumber').value,
      isActive: this.UserInfoFormGroup.get('isActive').value,
      entityAddresses: this.getAddressDTOToSave(),
      deleteAddresses: this.deleteAddresses,
      isRegionalSalesManager: Number(this.UserInfoFormGroup.get('role').value) === this.seniorRoleManagerCode ? true : false,
      userName: this.UserInfoFormGroup.get('userName').value
    });

    if (isEdit) {
      entityData.id = this.editId;
      this.entityService.putEntity(entityData).subscribe((res) => {
      }, error => {
      });
    } else {
      this.entityService.postEntity(entityData).subscribe((res) => {
      }, error => {
      });
    }
  }

  getAddressDTOToSave(): EntityAddressDTO[] {
    const result: Array<EntityAddressDTO> = [];

    this.genericAddressData.addressList.forEach((x: any) => {
      const address = new EntityAddressDTO({
        addressTypeId: Number(x.addressTypeId),
        effectiveDate: this.authService.getCustomDate().toDateString(),
        expirationDate: this.authService.getCustomDate().toDateString(),
        addProcessDate: this.authService.getCustomDate(),
        address: new AddressDTO({
          streetAddress1: x.address[0].streetAddress1,
          streetAddress2: x.address[0].streetAddress2,
          zipCode: x.address[0].cityZipCodeID,
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

    return result;
  }

  getUserEntityInfo(username): void {
    this.entityService.getEntityByUsername(environment.ApplicationId, username).pipe(takeUntil(this.stop$)).subscribe((res) => {
      this.populateUserInformationEntity(res);
    }, err => {
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  getUserList(callbackFn?: Function): void {
    this.resetList();
    this.isUsersLoading = true;
    Utils.blockUI();
    this.userService.getUsers(environment.ApplicationId).pipe(takeUntil(this.stop$)).subscribe((data: UserViewModel[]) => {
      if (data.length > 0) {
        this.populateUsersList(data);
      }
      this.isUsersLoading = false;
      Utils.unblockUI();
      if (callbackFn) { callbackFn(); }
    }, err => {
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  private populateUsersList(result: UserViewModel[]): void {
    this.resetList();
    _.map(result, (item) => {
      const user: UserViewModel = {
        userId: item.userId,
        userName: item.userName,
        appCode: item.appCode,
        roleId: item.roleId,
        carrierCode: item.carrierCode,
        agencyId: item.agencyId,
        subAgencyId: item.subAgencyId,
        emailAddress: item.emailAddress,
        createdDate: item.createdDate,
        isActive: item.isActive,
        companyName: item.companyName,
        firstName: item.firstName,
        lastName: item.lastName,
        userAccessRights: item.userAccessRights,
        userCredentials: item.userCredentials,
        isSystemUser: item.isSystemUser,
        fullName: `${item.firstName}${' '}${item.lastName}`,
        isLocked: item.isLocked
      };
      this.userListData.push(user);

    });

    this.userListData.sort((a, b) => a.userName.localeCompare(b.userName));
  }

  getAllRoles(): void {
    Utils.blockUI();
    this.roleList = [];
    this.role.getUserTypeList([environment.ApplicationId]).pipe(takeUntil(this.stop$)).subscribe((data: UserType[]) => {
      if (data.length > 0) {
        _.map(data, (item: UserType) => {
          const types: UserType = {
            roleId: item.roleId,
            programId: item.programId,
            roleGroupId: item.roleGroupId,
            roleName: item.roleName,
            createdDate: item.createdDate,
            isInternal: item.isInternal,
            isActive: item.isActive,
            roleGroup: item.roleGroup
          };
          if (types.roleName.toLowerCase() === this.userTypeLabelConstants.regionalSalesManager) {
            this.seniorRoleManagerCode = types.roleId;
          }

          this.roleList.push(types);
        });
      }
      Utils.unblockUI();
    }, err => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(err));
    });
  }
  //#endregion


  //#region USER TYPE
  initUserTypeSection(): void {
    this.getProgramList();
    this.getUserTypeList();
    this.getAccessCategories();
  }

  private getProgramList(): void {
    Utils.blockUI();
    this.applicationService.getUserApplications().pipe(takeUntil(this.stop$)).subscribe((app: LineOfBusinessDTO[]) => {
      if (app.length > 0) {
        this.common.programIdList = [];
        app.forEach(value => {
          value.programs.forEach(prog => {
            this.common.programList.push(prog);
            this.common.programIdList.push(prog.id);
          });
        });
      }
    }, err => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  private getUserTypeList(): void {
    this.userTypeList = [];
    if (this.common.programIdList.length === 0) {
      this.applicationService.getUserApplications().pipe(takeUntil(this.stop$)).subscribe((app: LineOfBusinessDTO[]) => {
        if (app.length > 0) {
          this.common.programList = [];
          this.common.programIdList = [];

          app.forEach(value => {
            value.programs.forEach(prog => {
              this.common.programList.push(prog);
              this.common.programIdList.push(prog.id);
            });
          });
          this.getAllRole();
        }
      }, err => {
        Utils.unblockUI();
        NotifUtils.showError(JSON.stringify(err));
      });
    } else {
      this.common.programIdList.push(environment.ApplicationId);
      this.getAllRole();
    }
  }

  private getAccessCategories(): void {
    this.userCategories = [];
    this.role.getAccessCategoriesList().pipe(takeUntil(this.stop$)).subscribe((data: RoleGroup[]) => {
      if (data.length > 0) {
        data.forEach(item => {
          this.userCategories.push(item);
        });
      }
      Utils.unblockUI();
    }, err => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  private getAllRole() {
    Utils.blockUI();
    this.role.getUserTypeList(this.common.programIdList).pipe(takeUntil(this.stop$)).subscribe((data: UserType[]) => {
      if (data.length > 0) {
        this.populateUserTypesData(data);
      }
      Utils.unblockUI();
    }, err => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  private populateUserTypesData(result: UserType[]): void {
    _.map(result, (item: UserType) => {
      const types: UserType = {
        roleId: item.roleId,
        programId: item.programId,
        roleGroupId: item.roleGroupId,
        roleName: item.roleName,
        createdDate: item.createdDate,
        isInternal: item.isInternal,
        isActive: item.isActive,
        roleGroup: item.roleGroup
      };
      this.userTypeList.push(types);
      this.addUserTypeTableItem(types);
    });
  }

  addNewUserType(role: Role): void {
    this.role.postUserType(role).pipe(takeUntil(this.stop$)).subscribe(data => {
      if (data) {
        this.resetUserTypeList();
        this.getAllRole();
      }
    }, err => {
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  editUserType(id: number, role: Role): void {
    this.role.putUserType(id, role).pipe(takeUntil(this.stop$)).subscribe(data => {
      if (data) {
        this.resetUserTypeList();
        this.getAllRole();
      }
    }, err => {
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  deleteUserType(id: number): void {
    this.role.deleteUserType(id).pipe(takeUntil(this.stop$)).subscribe(data => {
      if (data) {
        this.resetUserTypeList();
        this.getAllRole();
      }
    }, err => {
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  public addUserTypeTableItem(newItem: any, isAdd: boolean = false): void {
    const tr: ITableTd[] = [];
    const fields: string[] = [
      'roleGroup',
      'roleName',
      'isActive'
    ];

    fields.forEach((item, index) => {
      let display: any;

      switch (item) {
        case 'roleGroup':
          if (isAdd) {
            display = Boolean(newItem['roleName']) ? newItem['roleName'] : '';
          } else {
            display = Boolean(newItem[item]) ? newItem[item].description : '';
          }
          break;
        case 'isActive':
          display = newItem[item] ? 'Active' : 'In-Active';
          break;
        default:
          display = Boolean(newItem[item]) ? String(newItem[item]) : '';
          break;
      }

      tr.push({
        id: index + 1,
        value: Boolean(newItem[item]) ? newItem[item] : '',
        display: display,
      });
    });

    this.userTypeTableRows.push({
      id: this.userTypeTableRows.length > 0 ? this.userTypeTableRows.length + 1 : 1,
      tr: tr
    });
  }

  checkTypeNameUniqueness(term: string, programId: number): void {
    this.role.validateNameExistence(term, programId).pipe(takeUntil(this.stop$)).subscribe((res: any) => {
      if (res.status === 200) {
        this.isTypeNameNotFound.next(false);
      } else {
        this.isTypeNameNotFound.next(true);
      }
    }, err => {
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  resetList(): void {
    this.resetUserTypeList();
    this.userListData = [];
  }

  resetUserTypeList(): void {
    this.userTypeList = [];
    this.userTypeTableRows = [];
  }
  //#endregion

  searchSystemUsers(input: string): void {
    Utils.blockUI();
    this.userService.searchSystemUser({ term: input, appCode: environment.ApplicationId })
      .subscribe((result: UserViewModel[]) => {
        if (result.length > 0) {
          this.populateUsersList(result);
        }
        Utils.unblockUI();
      }, error => {
        Utils.unblockUI();
        NotifUtils.showError(error.message);
      });
  }

  searchUserType(input: string): void {
    Utils.blockUI();
    this.resetUserTypeList();
    this.role.searchUserType({ term: input, appCode: environment.ApplicationId })
      .subscribe((result: UserType[]) => {
        if (result.length > 0) {
          this.populateUserTypesData(result);
        }
        Utils.unblockUI();
      }, error => {
        Utils.unblockUI();
        NotifUtils.showError(error.message);
      });
  }

  userAuditLog(action: string, entityId: string): void {
    const payload = {
      userId: this.authService.getUserId(),
      keyId: entityId,
      auditType: '',
      description: '',
      method: ''
    };

    switch (action) {
      case UsersLabelConstants.auditLog.add:
          payload.auditType = UsersLabelConstants.auditLog.add;
          payload.description = UsersLabelConstants.userModalAddedSuccessfullyMessage;
          payload.method = UsersLabelConstants.methodName.add;
        break;
      case UsersLabelConstants.auditLog.edit:
          payload.auditType = UsersLabelConstants.auditLog.edit;
          payload.description = UsersLabelConstants.userModalUpdatedSuccessfullyMessage;
          payload.method = UsersLabelConstants.methodName.edit;
        break;
      case UsersLabelConstants.auditLog.delete:
          payload.auditType = UsersLabelConstants.auditLog.delete;
          payload.description = UsersLabelConstants.userModalDeletedSuccessfullyMessage;
          payload.method = UsersLabelConstants.methodName.delete;
        break;
    }

    this.auditLogService.insertToAuditLog(payload).subscribe();
  }

  saveUserInfo(userId: string, isLoggedIn: boolean = false): void {
    const payload = {
      userInfo: {
        userId: userId,
        fullName: `${this.UserInfoFormGroup.get('firstName').value} ${this.UserInfoFormGroup.get('lastName').value}`,
        isLoggedIn: isLoggedIn ? true : false
      }
    };

    this.authService.saveUserInfo(payload).pipe(take(1)).subscribe();
  }

  getUserInfo(userId: string): void {
      this.userService.getUserInfoById(userId).pipe(takeUntil(this.stop$)).subscribe(result => {
        Utils.unblockUI();
        this.saveUserInfo(userId, result.isLoggedIn);
      }, (error) => {
        Utils.unblockUI();
      });
    }

  setUserId(userName: string): void {
    if (userName){
      Utils.blockUI();
      this.userService.getSpecificUser(environment.ApplicationId, userName).subscribe(a => {
        Utils.unblockUI();
        this.userId = a.userId;
        this.UserInfoFormGroup.get('torrentUsername').setValue(a.torrentUserName);
      }, err => {
        NotifUtils.showError(err.message);
        Utils.unblockUI();
      });
    }
  }

  unlockAccount(userName: string, callbackFn?: Function): void {
    Utils.blockUI();
    this.authService.unlockAccount({ username:userName, appCode: environment.ApplicationId }).pipe(takeUntil(this.stop$)).subscribe(data => {
      Utils.unblockUI();
      this.userListData.findIndex(x => x.userName == userName);
      const fieldIndex = this.userListData?.findIndex(x => x.userName === userName);
      if (fieldIndex > -1) {
        this.userListData[fieldIndex].isLocked = false;
      }
      NotifUtils.showSuccess('Account has been unlocked');
      if (callbackFn) { callbackFn(); }
    }, err => {
      NotifUtils.showError(JSON.stringify(err));
    });
  }
}