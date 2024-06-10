export class PolicyContactsDTO implements IPolicyContactsDTO {
    id?: string;
    name?: string;
    email?: string;
    position?: string;
    isAssignedTo?: boolean;

    constructor(init?: Partial<PolicyContactsDTO>) {
        Object.assign(this, init);
      }
}

export interface IPolicyContactsDTO {
    id?: string;
    name?: string;
    email?: string;
    position?: string;
    isAssignedTo?: boolean;
}
