import { EntityDTO } from '../agency-management/entityDto';

export class AgentDTO implements IAgentDTO {
    id?: string;
    entityId?: string;
    programId?: number;
    createdBy?: number;
    createdDate?: Date;
    agencyId?: string;
    subAgencyId?: string;
    entity?: EntityDTO;
    subAgencyIds?: string[];
    agentLicenses?: Array<any> | undefined;
    isActive?: boolean = false;

    constructor(init?: Partial<AgentDTO>) {
        Object.assign(this, init);
      }
}

export interface IAgentDTO {
    id?: string;
    entityId?: string;
    programId?: number;
    createdBy?: number;
    createdDate?: Date;
    agencyId?: string | undefined;
    subAgencyId?: string | undefined;
    entity?: EntityDTO;
    subAgencyIds?: string[];
    agentLicenses?: Array<any> | undefined;
    isActive?: boolean;
}
