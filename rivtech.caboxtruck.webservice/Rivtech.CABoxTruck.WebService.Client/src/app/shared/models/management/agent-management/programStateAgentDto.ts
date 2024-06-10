import { AgencyDTO } from '../agency-management/agencyDto';
import { SubAgencyDTO } from '../agency-management/sub-agencyDto';
import { AgentDTO } from './agentDto';

export class ProgramStateAgentDTO implements IProgramStateAgentDTO {
    id?: string;
    programId?: number;
    stateID?: number;
    agentId?: string;
    agencyId?: string;
    subAgencyId?: string;
    agent: AgentDTO;
    agency: AgencyDTO;
    subAgency: SubAgencyDTO;
    subAgencyIds?: any[];

    constructor(init?: Partial<ProgramStateAgentDTO>) {
        Object.assign(this, init);
      }
}

export interface IProgramStateAgentDTO {
    id?: string;
    programId?: number;
    stateID?: number;
    agentId?: string;
    agencyId?: string;
    subAgencyId?: string;
    agent: AgentDTO;
    agency: AgencyDTO;
    subAgency: SubAgencyDTO;
    subAgencyIds?: any[];
}
