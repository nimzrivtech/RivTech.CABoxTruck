import { AgencyAddressEntityAddressDTO } from '../../../../../shared/models/management/agency-address.dto';

export class AgentUserResponseDTO implements IAgentUserResponseDTO {
  id?: string | undefined;
  entityId?: string | undefined;
  agencyId?: string | undefined;
  subAgencyId?: string | undefined;
  createdBy?: number | undefined;
  createdDate?: Date | undefined;
  entity?: AgentUserEntity | undefined;
  agentLicenses?: Array<AgentLicenseDTO> | undefined;
  isSystemUser?: boolean | undefined;
  subAgencyIds?: Array<string>;
  programStateIds?: Array<string>;
  constructor(data?: AgentUserResponseDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IAgentUserResponseDTO {
  id?: string | undefined;
  entityId?: string | undefined;
  agencyId?: string | undefined;
  subAgencyId?: string | undefined;
  createdBy?: number | undefined;
  createdDate?: Date | undefined;
  entity?: AgentUserEntity | undefined;
  agentLicenses?: Array<any> | undefined;
  isSystemUser?: boolean | undefined;
  subAgencyIds?: Array<string>;
  programStateIds?: Array<string>;
}

export interface AgentUserEntity {
  id?: string | undefined;
  isIndividual?: boolean | undefined;
  userName?: string | undefined;
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
  entityAddresses?: AgencyAddressEntityAddressDTO[] | undefined;
  deleteAddresses?: string[] | undefined;
  socialSecurityNumber?: string;
  fein?: string;
  isRegionalSalesManager?: boolean;
  isInternalUser?: boolean;
  isAgent?: boolean;
}

export interface AgentLicenseDTO {
  id?: string;
  agentId?: string;
  stateCode?: string;
  licenseNumber?: string;
  isSurplusLines?: boolean;
  licenseEffectiveDate?: Date;
  licenseExpirationDate?: Date;
  rowId?: number;
  removedProcessDate?: Date;
}