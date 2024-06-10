import { AgencyEntity } from '../agency-management/agency-details.response.dto';

export class SaveAgentDTO implements ISaveAgentDTO {
  id?: string;
  entityId?: string;
  programId?: number;
  stateCode?: string;
  isActive?: boolean;
  createdBy?: number;
  agencyId?: string;
  subAgencyId?: string;
  isSystemUser?: boolean;
  subAgencyIds?: Array<string>;
  programStateIds?: Array<number>;
  agentLicenses?: Array<AgentLicenses> | undefined;
  createdDate?: Date;
  entity?: AgencyEntity;
  userId?: number;
  retailerId?: string;

  constructor(data?: SaveAgentDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface ISaveAgentDTO {
    id?: string;
    entityId?: string;
    programId?: number;
    stateCode?: string;
    isActive?: boolean;
    createdBy?: number;
    agencyId?: string;
    subAgencyId?: string;
    isSystemUser?: boolean;
    subAgencyIds?: Array<string>;
    programStateIds?: Array<number>;
    agentLicenses?: Array<AgentLicenses> | undefined;
    createdDate?: Date;
    entity?: AgencyEntity;
    userId?: number;
    retailerId?: string;
}

export interface AgentLicenses {
  id?: string;
  agentId?: string;
  stateCode?: string;
  licenseNumber?: string;
  isSurplusLines?: boolean;
  licenseEffectiveDate?: string;
  licenseExpirationDate?: string;
  removedProcessDate?: string;
}