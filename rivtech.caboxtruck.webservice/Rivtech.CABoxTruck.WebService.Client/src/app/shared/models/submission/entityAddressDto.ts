import { AddressDTO } from './addressDto';

export class EntityAddressDTO implements IEntityAddressDTO {
    id?: string;
    entityId?: string;
    addressId?: string;
    addressTypeId?: number;
    addressType?: string;
    effectiveDate?: Date;
    expirationDate?: Date;
    addProcessDate?: Date;
    addedBy?: string;
    removeProcessDate?: Date | undefined;
    removedBy?: string | undefined;
    address?: AddressDTO;
    riskDetailId?: string | undefined;
    isMainGaragingAddress?: boolean | undefined;

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
    addressTypeId?: number;
    addressType?: string;
    effectiveDate?: Date;
    expirationDate?: Date;
    addProcessDate?: Date;
    addedBy?: string;
    removeProcessDate?: Date | undefined;
    removedBy?: string | undefined;
    address?: AddressDTO;
    riskDetailId?: string;

    contactName?: string;
    email?: string;
    phone?: string;
}