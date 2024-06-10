import { AddressDTO } from '../addressDto';

export class NameAndAddressDTO implements INameAndAddressDTO {
    riskId?: string;
    riskDetailId?: string;
    submissionNumber?: string;
    quoteNumber?: string;
    policyNumber?: string;
    businessName?: string;
    insuredDBA?: string;
    businessAddress?: string;
    city: string;
    state?: string;
    zipCode?: number | undefined;
    cityOperations: string;
    stateOperations: string;
    zipCodeOperations?: number | undefined;
    businessPrincipal?: string;
    email?: string;
    phoneExt?: string;
    addresses?: EntityAddressDTO[] | undefined;
    isRenewal: boolean;
    isFromRenewal: boolean;
    previousPolicyNumber: string;

    fieldVersions: any | undefined;

    constructor(init?: Partial<NameAndAddressDTO>) {
        Object.assign(this, init);
      }
}

export interface INameAndAddressDTO {
    riskId?: string;
    riskDetailId?: string;
    submissionNumber?: string;
    quoteNumber?: string;
    policyNumber?: string;
    businessName?: string;
    insuredDBA?: string;
    businessAddress?: string;
    city: string;
    state?: string;
    zipCode?: number | undefined;
    cityOperations: string;
    stateOperations: string;
    zipCodeOperations?: number | undefined;
    businessPrincipal?: string;
    email?: string;
    phoneExt?: string;

    addresses?: EntityAddressDTO[] | undefined;
    isRenewal: boolean;
    isFromRenewal: boolean;
    previousPolicyNumber: string;

    fieldVersions: any | undefined;
}


export class EntityAddressDTO implements IEntityAddressDTO {
    id?: string;
    entityId?: string;
    addressId?: string;
    addressTypeId?: number | undefined;
    riskDetailId?: number | undefined;
    address?: AddressDTO | undefined;

    contactName?: string;
    email?: string;
    phone?: string;

    constructor(init?: Partial<EntityAddressDTO>) {
        Object.assign(this, init);
      }
}

export interface IEntityAddressDTO {
    id?: string;
    entityId?: string;
    addressId?: string;
    addressTypeId?: number | undefined;
    riskDetailId?: number | undefined;
    address?: AddressDTO | undefined;

    contactName?: string;
    email?: string;
    phone?: string;
}

