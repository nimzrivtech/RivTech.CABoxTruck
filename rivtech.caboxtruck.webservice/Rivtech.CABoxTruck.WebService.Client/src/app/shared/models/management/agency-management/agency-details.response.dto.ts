import { AgencyAddressEntityAddressDTO } from '../agency-address.dto';

export class AgencyDetailsResponseDTO implements IAgencyRequestDTO {
  id?: string | undefined;
  entityId?: string | undefined;
  createdBy?: number | undefined;
  createdDate?: string | undefined;
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
  isActive?: boolean | undefined;
  commissionFlatRates?: CommissionFlatRate[] | undefined;
  lexisNexisCode?: string | undefined;
  agencyCode?: string | undefined;
  is1099Required?: boolean | undefined;
  subAgencyCode?: string | undefined;
  uwUserId?: number | undefined;

  linkedAgencies?: RetailerAgency[] | undefined;

    constructor(data?: AgencyDetailsResponseDTO) {
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
    agencyCode?: string | undefined;
    is1099Required?: boolean | undefined;
    subAgencyCode?: string | undefined;
    uwUserId?: number | undefined;
    linkedAgencies?: RetailerAgency[] | undefined;
  }

  export interface AgencyEntity {
    id?: string;
    isIndividual?: boolean;
    userName?: string;
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
    agencyCode?: string;
    isRegionalSalesManager?: boolean;
    isInternalUser?: boolean;
    isAgent?: boolean;
    hasSubsidiaries?: boolean;
    isActive?: boolean;
    createdBy?: number;
    entityAddresses?: AgencyAddressEntityAddressDTO[];
    deleteAddresses?: string[];
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

  export interface RetailerAgency {
    id?: string;
    retailerId?: string;
    agencyId?: string;
  }