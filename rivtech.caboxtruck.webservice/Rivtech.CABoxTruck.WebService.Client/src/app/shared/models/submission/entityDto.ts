import { BusinessTypeDTO } from './businesstypeDto';
import { EntityAddressDTO } from './entityAddressDto';

export class EntityDTO implements IEntityDTO {
    id?: string;
    isIndividual?: boolean;
    firstName?: string | undefined;
    middleName?: string | undefined;
    lastName?: string | undefined;
    companyName?: string | undefined;
    readonly fullName?: string | undefined;
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
    genderID?: number | undefined;
    maritalStatusID?: number | undefined;
    businessTypeID?: number | undefined;
    dba?: string | undefined;
    yearEstablished?: number;
    federalIDNumber?: string | undefined;
    naicsCodeID?: number | undefined;
    additionalNAICSCodeID?: number | undefined;
    iccmcDocketNumber?: string | undefined;
    usdotNumber?: string | undefined;
    pucNumber?: string | undefined;
    hasSubsidiaries?: boolean;
    createdBy?: number;
    createdDate?: Date;
    businessType?: BusinessTypeDTO;
    entityAddresses?: EntityAddressDTO[] | undefined;

    constructor(init?: Partial<EntityDTO>) {
        Object.assign(this, init);
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
    genderID?: number | undefined;
    maritalStatusID?: number | undefined;
    businessTypeID?: number | undefined;
    dba?: string | undefined;
    yearEstablished?: number;
    federalIDNumber?: string | undefined;
    naicsCodeID?: number | undefined;
    additionalNAICSCodeID?: number | undefined;
    iccmcDocketNumber?: string | undefined;
    usdotNumber?: string | undefined;
    pucNumber?: string | undefined;
    hasSubsidiaries?: boolean;
    createdBy?: number;
    createdDate?: Date;
    businessType?: BusinessTypeDTO;
    entityAddresses?: EntityAddressDTO[] | undefined;
}