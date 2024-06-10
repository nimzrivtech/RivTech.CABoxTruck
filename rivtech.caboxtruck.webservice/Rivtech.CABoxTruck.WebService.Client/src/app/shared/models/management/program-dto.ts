export class ProgramDTO {
    id: number;
    lobId: number;
    insuranceCompanyId: string;
    programName: string;
    riskTypeId: number;
    url: string;
    effectiveDate?: Date;
    expirationDate?: Date;
    addProcessDate: Date;
    addedBy: number;
    removeProcessDate?: Date;
    removedBy?: string;
    isSurplusLine?: boolean;
    insuranceCompany?: any;
    programStateUsers?: any;
    formTypeCode?: string;
    fasterProgramCode?: string;
    masterProgramCode?: string;

    constructor(data?: IProgramDTO) {
        if (data) {
            for (const property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }
}

export interface IProgramDTO {
    id: number;
    lobId: number;
    insuranceCompanyId: string;
    programName: string;
    riskTypeId: number;
    url: string;
    effectiveDate?: Date;
    expirationDate?: Date;
    addProcessDate: Date;
    addedBy: number;
    removeProcessDate?: Date;
    removedBy?: string;
    isSurplusLine?: boolean;
    insuranceCompany?: any;
    programStateUsers?: any;
    formTypeCode?: string;
    fasterProgramCode?: string;
    masterProgramCode?: string;
}