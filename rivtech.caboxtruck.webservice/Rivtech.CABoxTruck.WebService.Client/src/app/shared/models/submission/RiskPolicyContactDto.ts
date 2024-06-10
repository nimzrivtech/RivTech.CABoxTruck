export class RiskPolicyContactDTO implements IRiskPolicyContactDTO {
    id?: string;
    riskDetailId?: string;
    typeId?: number;
    type?: string;
    firstName?: string;
    lastName?: string;
    fullName?: string;
    email?: string;
    phone?: string;
    address?: string;
    city?: string;
    state?: string;
    zipCode?: number;

    constructor(init?: Partial<RiskPolicyContactDTO>) {
        Object.assign(this, init);
      }
}

export interface IRiskPolicyContactDTO {
    id?: string;
    riskDetailId?: string;
    typeId?: number;
    type?: string;
    firstName?: string;
    lastName?: string;
    fullName?: string;
    email?: string;
    phone?: string;
    address?: string;
    city?: string;
    state?: string;
    zipCode?: number;
}
