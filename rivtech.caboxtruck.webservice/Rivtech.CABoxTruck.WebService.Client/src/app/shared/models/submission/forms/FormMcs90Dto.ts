
export class FormMcs90DTO implements IFormMcs90DTO {
    id?: string;
    riskDetailId?: string | undefined;
    formId?: string | undefined;

    usDotNumber?: string | undefined;
    dateReceived?: Date | undefined;
    issuedTo?: string | undefined;
    state?: string | undefined;
    datedAt?: string | undefined;
    policyDateDay?: number | undefined;
    policyDateMonth?: string | undefined;
    policyDateYear?: number | undefined;
    policyNumber?: string | undefined;
    effectiveDate?: Date | undefined;
    insuranceCompany?: string | undefined;
    counterSignedBy?: string | undefined;
    accidentLimit?: number | undefined;

    createdBy?: string | undefined;
    createdDate?: Date | undefined;

    constructor(init?: Partial<IFormMcs90DTO>) {
        Object.assign(this, init);
    }
}

export interface IFormMcs90DTO {
    id?: string;
    riskDetailId?: string | undefined;
    formId?: string | undefined;

    usDotNumber?: string | undefined;
    dateReceived?: Date | undefined;
    issuedTo?: string | undefined;
    state?: string | undefined;
    datedAt?: string | undefined;
    policyDateDay?: number | undefined;
    policyDateMonth?: string | undefined;
    policyNumber?: string | undefined;
    policyDateYear?: number | undefined;
    effectiveDate?: Date | undefined;
    insuranceCompany?: string | undefined;
    counterSignedBy?: string | undefined;
    accidentLimit?: number | undefined;

    createdBy?: string | undefined;
    createdDate?: Date | undefined;
}

