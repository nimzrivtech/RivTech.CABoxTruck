import { AgencyDTO } from '../agency-management/agencyDto';

export class ProgramStateAgencyDTO implements IProgramStateAgencyDTO {
    id?: string;
    programId?: number;
    agencyId?: string;
    agency: AgencyDTO;

    constructor(init?: Partial<ProgramStateAgencyDTO>) {
        Object.assign(this, init);
    }
}

export interface IProgramStateAgencyDTO {
    id?: string;
    programId?: number;
    agencyId?: string;
    agency: AgencyDTO;
}
export interface IProgramStateDTO {
    id?: number;
    mainProgram: string;
    programName: string;
    formType: string;
    state: string;
    status?: string;
    isSelected?: boolean;
}
export interface IProgramStateRequestDTO {
    id?: string;
    programStateIds?: number[];
    agencyId?: string;
    subAgencyId?: string;
    isActive?: boolean;
}