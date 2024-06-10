/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { EntityAddressDTO } from '../EntityAddressDto';

export class EntityDTO implements IEntityDTO {
    id?: string;
    appCode?: number;
    isIndividual?: boolean;
    firstName?: string | undefined;
    middleName?: string | undefined;
    lastName?: string | undefined;
    companyName?: string | undefined;
    fullName?: string | undefined;
    homePhone?: string | undefined;
    mobilePhone?: string | undefined;
    workPhone?: string | undefined;
    workPhoneExtension?: string | undefined;
    workFax?: string | undefined;
    personalEmailAddress?: string | undefined;
    workEmailAddress?: string | undefined;
    birthDate?: Date | string;
    age?: number;
    driverLicenseNumber?: string | undefined;
    driverLicenseState?: string | undefined;
    driverLicenseExpiration?: Date | undefined;
    genderID?: number | undefined;
    maritalStatusID?: number | undefined;
    businessTypeID?: number | undefined;
    dba?: string | undefined;
    yearEstablished?: number | undefined;
    federalIDNumber?: string | undefined;
    naicsCodeID?: number | undefined;
    additionalNAICSCodeID?: number | undefined;
    iccmcDocketNumber?: string | undefined;
    usdotNumber?: string | undefined;
    pucNumber?: string | undefined;
    hasSubsidiaries?: boolean;
    isInternalUser?: boolean;
    isActive?: boolean;
    createdBy?: number;
    entityAddresses?: EntityAddressDTO[] | undefined;
    deleteAddresses?: string[] | undefined;
    isRegionalSalesManager?: boolean;
    userName?: string | undefined;

    constructor(data?: IEntityDTO) {
        if (data) {
            for (const property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data['id'];
            this.appCode = _data['appCode'];
            this.isIndividual = _data['isIndividual'];
            this.firstName = _data['firstName'];
            this.middleName = _data['middleName'];
            this.lastName = _data['lastName'];
            this.companyName = _data['companyName'];
            this.fullName = _data['fullName'];
            this.homePhone = _data['homePhone'];
            this.mobilePhone = _data['mobilePhone'];
            this.workPhone = _data['workPhone'];
            this.workPhoneExtension = _data['workPhoneExtension'];
            this.workFax = _data['workFax'];
            this.personalEmailAddress = _data['personalEmailAddress'];
            this.workEmailAddress = _data['workEmailAddress'];
            this.birthDate = _data['birthDate'] ? new Date(_data['birthDate'].toString()) : <any>undefined;
            this.age = _data['age'];
            this.driverLicenseNumber = _data['driverLicenseNumber'];
            this.driverLicenseState = _data['driverLicenseState'];
            this.driverLicenseExpiration = _data['driverLicenseExpiration'] ? new Date(_data['driverLicenseExpiration'].toString()) : <any>undefined;
            this.genderID = _data['genderID'];
            this.maritalStatusID = _data['maritalStatusID'];
            this.businessTypeID = _data['businessTypeID'];
            this.dba = _data['dba'];
            this.yearEstablished = _data['yearEstablished'];
            this.federalIDNumber = _data['federalIDNumber'];
            this.naicsCodeID = _data['naicsCodeID'];
            this.additionalNAICSCodeID = _data['additionalNAICSCodeID'];
            this.iccmcDocketNumber = _data['iccmcDocketNumber'];
            this.usdotNumber = _data['usdotNumber'];
            this.pucNumber = _data['pucNumber'];
            this.hasSubsidiaries = _data['hasSubsidiaries'];
            this.isInternalUser = _data['isInternalUser'];
            this.isActive = _data['isActive'];
            this.createdBy = _data['createdBy'];
            this.deleteAddresses = _data['deleteAddresses'];
            this.isRegionalSalesManager = _data['isRegionalSalesManager'];
            this.userName = _data['userName'];
            if (Array.isArray(_data['entityAddresses'])) {
                this.entityAddresses = [] as any;
                for (const item of _data['entityAddresses'])
                    this.entityAddresses!.push(EntityAddressDTO.fromJS(item));
            }
        }
    }

    static fromJS(data: any): EntityDTO {
        data = typeof data === 'object' ? data : {};
        const result = new EntityDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['id'] = this.id;
        data['appCode'] = this.appCode;
        data['isIndividual'] = this.isIndividual;
        data['firstName'] = this.firstName;
        data['middleName'] = this.middleName;
        data['lastName'] = this.lastName;
        data['companyName'] = this.companyName;
        data['fullName'] = this.fullName;
        data['homePhone'] = this.homePhone;
        data['mobilePhone'] = this.mobilePhone;
        data['workPhone'] = this.workPhone;
        data['workPhoneExtension'] = this.workPhoneExtension;
        data['workFax'] = this.workFax;
        data['personalEmailAddress'] = this.personalEmailAddress;
        data['workEmailAddress'] = this.workEmailAddress;
        data['birthDate'] = this.birthDate ? this.birthDate : <any>undefined;
        data['age'] = this.age;
        data['driverLicenseNumber'] = this.driverLicenseNumber;
        data['driverLicenseState'] = this.driverLicenseState;
        data['driverLicenseExpiration'] = this.driverLicenseExpiration ? new Date(this.driverLicenseExpiration).toISOString() : <any>undefined;
        data['genderID'] = this.genderID;
        data['maritalStatusID'] = this.maritalStatusID;
        data['businessTypeID'] = this.businessTypeID;
        data['dba'] = this.dba;
        data['yearEstablished'] = this.yearEstablished;
        data['federalIDNumber'] = this.federalIDNumber;
        data['naicsCodeID'] = this.naicsCodeID;
        data['additionalNAICSCodeID'] = this.additionalNAICSCodeID;
        data['iccmcDocketNumber'] = this.iccmcDocketNumber;
        data['usdotNumber'] = this.usdotNumber;
        data['pucNumber'] = this.pucNumber;
        data['hasSubsidiaries'] = this.hasSubsidiaries;
        data['isInternalUser'] = this.isInternalUser;
        data['isActive'] = this.isActive;
        data['createdBy'] = this.createdBy;
        data['deleteAddresses'] = this.deleteAddresses;
        data['isRegionalSalesManager'] = this.isRegionalSalesManager;
        data['userName'] = this.userName;
        if (Array.isArray(this.entityAddresses)) {
            data['entityAddresses'] = [];
            for (const item of this.entityAddresses)
                data['entityAddresses'].push(item.toJSON());
        }
        return data;
    }
}

export interface IEntityDTO {
    id?: string;
    appCode?: number;
    isIndividual?: boolean;
    firstName?: string | undefined;
    middleName?: string | undefined;
    lastName?: string | undefined;
    companyName?: string | undefined;
    fullName?: string | undefined;
    homePhone?: string | undefined;
    mobilePhone?: string | undefined;
    workPhone?: string | undefined;
    workPhoneExtension?: string | undefined;
    workFax?: string | undefined;
    personalEmailAddress?: string | undefined;
    workEmailAddress?: string | undefined;
    birthDate?: Date | string;
    age?: number;
    driverLicenseNumber?: string | undefined;
    driverLicenseState?: string | undefined;
    driverLicenseExpiration?: Date | undefined;
    genderID?: number | undefined;
    maritalStatusID?: number | undefined;
    businessTypeID?: number | undefined;
    dba?: string | undefined;
    yearEstablished?: number | undefined;
    federalIDNumber?: string | undefined;
    naicsCodeID?: number | undefined;
    additionalNAICSCodeID?: number | undefined;
    iccmcDocketNumber?: string | undefined;
    usdotNumber?: string | undefined;
    pucNumber?: string | undefined;
    hasSubsidiaries?: boolean;
    isInternalUser?: boolean;
    isActive?: boolean;
    createdBy?: number;
    entityAddresses?: EntityAddressDTO[] | undefined;
    deleteAddresses?: string[] | undefined;
    isRegionalSalesManager?: boolean;
    userName?: string | undefined;
}