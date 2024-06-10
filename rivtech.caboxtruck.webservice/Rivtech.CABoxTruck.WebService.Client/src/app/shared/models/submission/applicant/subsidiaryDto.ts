export class SubsidiaryDTO implements ISubsidiaryDTO {
    id: string;
    riskDetailId: string;
    entityId: string;
    name: string;
    relationship: string;
    typeOfBusinessId?: number | undefined;
    numVechSizeComp: string;

    constructor(init?: Partial<SubsidiaryDTO>) {
        Object.assign(this, init);
      }
}

export interface ISubsidiaryDTO {
    id: string;
    riskDetailId: string;
    entityId: string;
    name: string;
    relationship: string;
    typeOfBusinessId?: number | undefined;
    numVechSizeComp: string;
}

