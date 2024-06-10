import { BusinessTypeDTO } from '../business-typeDto';
import { EntityAddressDTO } from '../EntityAddressDto';

export class EntityDTO implements IEntityDTO {
    id?: string;
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
    birthDate?: Date;
    age?: number;
    driverLicenseNumber?: string | undefined;
    driverLicenseState?: string | undefined;
    driverLicenseExpiration?: Date;
    genderID?: number;
    maritalStatusID?: number;
    businessTypeID?: number;
    dba?: string | undefined;
    yearEstablished?: number;
    federalIDNumber?: string | undefined;
    naicsCodeID?: number | undefined;
    additionalNAICSCodeID?: number | undefined;
    iccmcDocketNumber?: string | undefined;
    usdotNumber?: string | undefined;
    pucNumber?: string | undefined;
    hasSubsidiaries?: boolean;
    createdBy?: string;
    createdDate?: Date;
    businessType?: BusinessTypeDTO;
    entityAddresses?: EntityAddressDTO[] | undefined;

    constructor(data?: IEntityDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
            this.isIndividual = _data["isIndividual"];
            this.firstName = _data["firstName"];
            this.middleName = _data["middleName"];
            this.lastName = _data["lastName"];
            this.companyName = _data["companyName"];
            this.fullName = _data["fullName"];
            this.homePhone = _data["homePhone"];
            this.mobilePhone = _data["mobilePhone"];
            this.workPhone = _data["workPhone"];
            this.workPhoneExtension = _data["workPhoneExtension"];
            this.workFax = _data["workFax"];
            this.personalEmailAddress = _data["personalEmailAddress"];
            this.workEmailAddress = _data["workEmailAddress"];
            this.birthDate = _data["birthDate"] ? new Date(_data["birthDate"].toString()) : <any>undefined;
            this.age = _data["age"];
            this.driverLicenseNumber = _data["driverLicenseNumber"];
            this.driverLicenseState = _data["driverLicenseState"];
            this.driverLicenseExpiration = _data["driverLicenseExpiration"] ? new Date(_data["driverLicenseExpiration"].toString()) : <any>undefined;
            this.genderID = _data["genderID"];
            this.maritalStatusID = _data["maritalStatusID"];
            this.businessTypeID = _data["businessTypeID"];
            this.dba = _data["dba"];
            this.yearEstablished = _data["yearEstablished"];
            this.federalIDNumber = _data["federalIDNumber"];
            this.naicsCodeID = _data["naicsCodeID"];
            this.additionalNAICSCodeID = _data["additionalNAICSCodeID"];
            this.iccmcDocketNumber = _data["iccmcDocketNumber"];
            this.usdotNumber = _data["usdotNumber"];
            this.pucNumber = _data["pucNumber"];
            this.hasSubsidiaries = _data["hasSubsidiaries"];
            this.createdBy = _data["createdBy"];
            this.createdDate = _data["createdDate"] ? new Date(_data["createdDate"].toString()) : <any>undefined;
            this.businessType = _data["businessType"] ? BusinessTypeDTO.fromJS(_data["businessType"]) : <any>undefined;
            if (Array.isArray(_data["entityAddresses"])) {
                this.entityAddresses = [] as any;
                for (let item of _data["entityAddresses"])
                    this.entityAddresses!.push(EntityAddressDTO.fromJS(item));
            }
        }
    }

    static fromJS(data: any): EntityDTO {
        data = typeof data === 'object' ? data : {};
        let result = new EntityDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["isIndividual"] = this.isIndividual;
        data["firstName"] = this.firstName;
        data["middleName"] = this.middleName;
        data["lastName"] = this.lastName;
        data["companyName"] = this.companyName;
        data["fullName"] = this.fullName;
        data["homePhone"] = this.homePhone;
        data["mobilePhone"] = this.mobilePhone;
        data["workPhone"] = this.workPhone;
        data["workPhoneExtension"] = this.workPhoneExtension;
        data["workFax"] = this.workFax;
        data["personalEmailAddress"] = this.personalEmailAddress;
        data["workEmailAddress"] = this.workEmailAddress;
        data["birthDate"] = this.birthDate ? this.birthDate.toISOString() : <any>undefined;
        data["age"] = this.age;
        data["driverLicenseNumber"] = this.driverLicenseNumber;
        data["driverLicenseState"] = this.driverLicenseState;
        data["driverLicenseExpiration"] = this.driverLicenseExpiration ? this.driverLicenseExpiration.toISOString() : <any>undefined;
        data["genderID"] = this.genderID;
        data["maritalStatusID"] = this.maritalStatusID;
        data["businessTypeID"] = this.businessTypeID;
        data["dba"] = this.dba;
        data["yearEstablished"] = this.yearEstablished;
        data["federalIDNumber"] = this.federalIDNumber;
        data["naicsCodeID"] = this.naicsCodeID;
        data["additionalNAICSCodeID"] = this.additionalNAICSCodeID;
        data["iccmcDocketNumber"] = this.iccmcDocketNumber;
        data["usdotNumber"] = this.usdotNumber;
        data["pucNumber"] = this.pucNumber;
        data["hasSubsidiaries"] = this.hasSubsidiaries;
        data["createdBy"] = this.createdBy;
        data["createdDate"] = this.createdDate ? this.createdDate.toISOString() : <any>undefined;
        data["businessType"] = this.businessType ? this.businessType.toJSON() : <any>undefined;
        if (Array.isArray(this.entityAddresses)) {
            data["entityAddresses"] = [];
            for (let item of this.entityAddresses)
                data["entityAddresses"].push(item.toJSON());
        }
        return data; 
    }
}

export interface IEntityDTO {
    id?: string;
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
    birthDate?: Date;
    age?: number;
    driverLicenseNumber?: string | undefined;
    driverLicenseState?: string | undefined;
    driverLicenseExpiration?: Date;
    genderID?: number;
    maritalStatusID?: number;
    businessTypeID?: number;
    dba?: string | undefined;
    yearEstablished?: number;
    federalIDNumber?: string | undefined;
    naicsCodeID?: number | undefined;
    additionalNAICSCodeID?: number | undefined;
    iccmcDocketNumber?: string | undefined;
    usdotNumber?: string | undefined;
    pucNumber?: string | undefined;
    hasSubsidiaries?: boolean;
    createdBy?: string;
    createdDate?: Date;
    businessType?: BusinessTypeDTO;
    entityAddresses?: EntityAddressDTO[] | undefined;
}
