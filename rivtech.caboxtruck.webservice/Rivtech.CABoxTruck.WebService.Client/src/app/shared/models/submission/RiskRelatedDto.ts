export class RiskRelatedDTO implements IRiskRelatedDTO {
    id?: string;
    current?: string;
    policyNumber?: string;
    isPolicy?: boolean;

    constructor(init?: Partial<RiskRelatedDTO>) {
        Object.assign(this, init);
      }
}

export interface IRiskRelatedDTO {
    id?: string;
    current?: string;
    policyNumber?: string;
    isPolicy?: boolean;
}
