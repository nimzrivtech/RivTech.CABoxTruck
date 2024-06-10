
export class EndorsementProcessQueueDTO implements IEndorsementProcessQueueDTO {
    riskId?: string;
    riskDetailId?: string;
    effectiveDate?: Date;
    processType?: string;
    endorsementText?: string;
    policyChanges?: string;

    constructor(init?: Partial<EndorsementProcessQueueDTO>) {
        Object.assign(this, init);
      }
}

export interface IEndorsementProcessQueueDTO {
    riskId?: string;
    riskDetailId?: string;
    effectiveDate?: Date;
    processType?: string;
    endorsementText?: string;
    policyChanges?: string;
}

