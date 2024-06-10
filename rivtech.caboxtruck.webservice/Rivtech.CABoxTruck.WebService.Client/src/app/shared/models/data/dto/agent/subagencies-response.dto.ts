export class SubAgenciesResponseDTO implements ISubAgenciesResponseDTO {
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
  CommissionFlatRates?: any[] | null;
  uwUserId?: number | undefined;

  constructor(data?: SubAgenciesResponseDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface ISubAgenciesResponseDTO {
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
  CommissionFlatRates?: any[] | null;
  uwUserId?: number | undefined;
}

export interface SubAgenciesEntity {
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
  entityAddresses?: SubAgenciesEntityAddress[] | undefined;
  deleteAddresses?: string[] | undefined;
}

export interface SubAgenciesEntityAddress {
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
  address?: SubAgenciesAddress | undefined;
}

export interface SubAgenciesAddress {
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
}