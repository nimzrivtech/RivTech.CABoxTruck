import { SubAgenciesEntity } from './subagencies-response.dto';

export class AgenciesSubAgenciesResponseDTO implements IAgenciesSubAgenciesResponseDTO {
  id?: string;
  agencyId?: string;
  subAgencyId?: string;
  effectiveDate?: Date;
  expirationDate?: Date;
  addProcessDate?: Date;
  removeProcessDate?: Date;
  agency?: Agencies;
  subAgency?: Array<SubAgencies>;

  constructor(data?: AgenciesSubAgenciesResponseDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface Agencies {
  id?: string;
  entityId?: string;
  createdBy?: number;
  createdDate?: string;
  entity?: AgenciesSubAgenciesEntity;
  isActive?: boolean;
}

export interface IAgenciesSubAgenciesResponseDTO {
  id?: string;
  agencyId?: string;
  subAgencyId?: string;
  effectiveDate?: Date;
  expirationDate?: Date;
  addProcessDate?: Date;
  removeProcessDate?: Date;
  agency?: Agencies;
  subAgency?: Array<SubAgencies>;
}

export interface AgenciesSubAgenciesEntity {
  id?: string | undefined;
  isIndividual?: boolean | undefined;
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
  birthDate?: Date | undefined;
  age?: number | undefined;
  driverLicenseNumber?: string | undefined;
  driverLicenseState?: string | undefined;
  driverLicenseExpiration?: Date | undefined;
  genderID?: number | undefined;
  maritalStatusID?: number | undefined;
  businessTypeID?: number | undefined;
  programId?: number | undefined;
  dba?: string | undefined;
  yearEstablished?: number | undefined;
  federalIDNumber?: string | undefined;
  naicsCodeID?: number | undefined;
  additionalNAICSCodeID?: number | undefined;
  iccmcDocketNumber?: string | undefined;
  usdotNumber?: string | undefined;
  pucNumber?: string | undefined;
  agencyCode?: string | undefined;
  hasSubsidiaries?: boolean | undefined;
  isActive?: boolean | undefined;
  createdBy?: number | undefined;
  entityAddresses?: AgenciesSubAgenciesEntityAddress[] | undefined;
  deleteAddresses?: string[] | undefined;
}

export interface AgenciesSubAgenciesEntityAddress {
  id?: string | undefined;
  entityId?: string | undefined;
  addressId?: string | undefined;
  addressTypeId?: number | undefined;
  effectiveDate?: Date | undefined;
  expirationDate?: Date | undefined;
  addProcessDate?: Date | undefined;
  addedBy?: number | undefined;
  removedProcessDate?: Date | undefined;
  removedBy?: number | undefined;
  address?: AgenciesSubAgenciesAddress | undefined;
}

export interface AgenciesSubAgenciesAddress {
  id?: string | undefined;
  streetAddress1?: string | undefined;
  streetAddress2?: string | undefined;
  cityZipCodeID?: number | undefined;
  zipCodeExt?: string | undefined;
  longitude?: number | undefined;
  latitude?: number | undefined;
  isGarageIndoor?: boolean | undefined;
  isGarageOutdoor?: boolean | undefined;
  isGarageFenced?: boolean | undefined;
  isGarageLighted?: boolean | undefined;
  isGarageWithSecurityGuard?: boolean | undefined;
  city?: string | undefined;
  stateCode?: string | undefined;
  zipCode?: string | undefined;
}

export interface SubAgencies {
  id?: string | undefined;
  entityId?: string | undefined;
  agencyId?: string;
  createdBy?: number | undefined;
  stateCode?: string;
  createdDate?: Date | undefined;
  entity?: SubAgenciesEntity | undefined;
  subAgencyStatusHistoryId?: string | null;
  contactName?: string;
  isEFTAvailable?: boolean;
  EFTAccountNumber?: string;
  EFTAccountTypeId?: string;
  EFTRoutingNumber?: string;
  EFTEmail?: string;
  lexisNexisCode?: string | undefined;
  hasBindingAuthority?: boolean;
  licenseEffectiveDate?: Date | string | null;
  licenseExpirationDate?: Date | string | null;
  commisionTypeId?: string;
  commissionGroupId?: string | null;
  agencyManagementSystemId?: string;
  regionalSalesManagerId?: string | null;
  is1099Required?: boolean;
  subAgencyCode?: string;
  isActive?: boolean;
  commissionFlatRates?: any[] | null;
}
