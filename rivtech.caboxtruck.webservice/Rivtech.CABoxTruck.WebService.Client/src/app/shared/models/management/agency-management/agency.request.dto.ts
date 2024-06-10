import { AgencyAddressEntityAddressDTO } from '../agency-address.dto';

export class AgencyRequestDTO implements IAgencyRequestDTO {
    id?: string | undefined;
    entityId?: string | undefined;
    programId?: number | undefined;
    agencyId?: string | undefined;
    stateCode?: string | undefined;
    createdBy?: number | undefined;
    entity?: AgencyEntity | undefined;
    agencyStatusHistoryId?: string | undefined;
    contactName?: string | undefined;
    isEFTAvailable?: boolean | undefined;
    eftAccountNumber?: string | undefined;
    eftAccountTypeId?: number | undefined;
    eftRoutingNumber?: string | undefined;
    eftEmail?: string | undefined;
    isCommissionPaidToAgency?: boolean | undefined;
    agencyNetworkId?: number | undefined;
    hasBindingAuthority?: boolean | undefined;
    licenseEffectiveDate?: string | undefined;
    licenseExpirationDate?: string | undefined;
    commisionTypeId?: string | undefined;
    commissionGroupId?: string | undefined;
    agencyManagementSystemId?: number | undefined;
    regionalSalesManagerId?: string | undefined;
    commissionFlatRates?: CommissionFlatRate[] | undefined;
    lexisNexisCode?: string | undefined;
    isActive?: boolean;
    agencyCode?: string;
    subAgencyCode?: string;
    is1099Required?: boolean;
    uwUserId?: number | undefined;
    linkedAgencies?: any | undefined;

    constructor(data?: AgencyRequestDTO) {
      if (data) {
        for (const property in data) {
          if (data.hasOwnProperty(property)) {
            (<any>this)[property] = (<any>data)[property];
          }
        }
      }
    }
  }

  export interface IAgencyRequestDTO {
    id?: string | undefined;
    entityId?: string | undefined;
    programId?: number | undefined;
    agencyId?: string | undefined;
    stateCode?: string | undefined;
    createdBy?: number | undefined;
    entity?: AgencyEntity | undefined;
    agencyStatusHistoryId?: string | undefined;
    contactName?: string | undefined;
    isEFTAvailable?: boolean | undefined;
    eftAccountNumber?: string | undefined;
    eftAccountTypeId?: number | undefined;
    eftRoutingNumber?: string | undefined;
    eftEmail?: string | undefined;
    isCommissionPaidToAgency?: boolean | undefined;
    agencyNetworkId?: number | undefined;
    hasBindingAuthority?: boolean | undefined;
    licenseEffectiveDate?: string | undefined;
    licenseExpirationDate?: string | undefined;
    commisionTypeId?: string | undefined;
    commissionGroupId?: string | undefined;
    agencyManagementSystemId?: number | undefined;
    regionalSalesManagerId?: string | undefined;
    commissionFlatRates?: CommissionFlatRate[] | undefined;
    lexisNexisCode?: string | undefined;
    isActive?: boolean;
    agencyCode?: string;
    subAgencyCode?: string;
    is1099Required?: boolean;
    uwUserId?: number | undefined;
    linkedAgencies?: any | undefined;
  }

  export interface AgencyEntity {
    id?: string;
    isIndividual?: boolean;
    firstName?: string;
    middleName?: string;
    lastName?: string;
    companyName?: string;
    fullName?: string;
    homePhone?: string;
    mobilePhone?: string;
    workPhone?: string;
    workPhone2?: string;
    workPhoneExtension?: string;
    workFax?: string;
    socialSecurityNumber?: string;
    personalEmailAddress?: string;
    workEmailAddress?: string;
    birthDate?: Date;
    age?: number;
    driverLicenseNumber?: string;
    driverLicenseState?: string;
    driverLicenseExpiration?: Date;
    genderID?: number;
    maritalStatusID?: number;
    businessTypeID?: number;
    programId?: number;
    dba?: string;
    yearEstablished?: number;
    federalIDNumber?: string;
    fein?: string;
    naicsCodeID?: number;
    additionalNAICSCodeID?: number;
    iccmcDocketNumber?: string;
    usdotNumber?: string;
    pucNumber?: string;
    isRegionalSalesManager?: boolean;
    isInternalUser?: boolean;
    isAgent?: boolean;
    hasSubsidiaries?: boolean;
    isActive?: boolean;
    createdBy?: number;
    entityAddresses?: AgencyAddressEntityAddressDTO[];
    deleteAddresses?: string[];
    userName?: string;
    torrentUsername?: string | undefined;
  }

  export interface Address {
    id?: string;
    streetAddress1?: string;
    streetAddress2?: string;
    zipCode?: string;
    zipCodeExt?: string;
    city?: string;
    stateCode?: string;
    county?: string;
    countryCode?: string;
    longitude?: number;
    latitude?: number;
    isGarageIndoor?: boolean;
    isGarageOutdoor?: boolean;
    isGarageFenced?: boolean;
    isGarageLighted?: boolean;
    isGarageWithSecurityGuard?: boolean;
  }

  export interface CommissionFlatRate {
    id?: string;
    commmissionGroupId?: string;
    agencyId?: string;
    subAgencyId?: string;
    nbCommissionPercent?: number;
    rnCommissionPercent?: number;
    createdDate?: string;
    effectiveDate?: string;
    expirationDate?: string;
    removedProcessDate?: Date;
    rowId?: number;
  }