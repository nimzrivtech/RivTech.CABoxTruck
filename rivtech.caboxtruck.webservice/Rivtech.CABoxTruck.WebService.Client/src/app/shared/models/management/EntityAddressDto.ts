import { AddressDTO } from './addressDto';
import { DatePipe } from '@angular/common';

export class EntityAddressDTO implements IEntityAddressDTO {
    id?: string;
    entityId?: string;
    addressId?: string;
    addressTypeId?: number;
    effectiveDate?: string;
    expirationDate?: string;
    addProcessDate?: Date;
    addedBy?: string;
    removeProcessDate?: Date | undefined;
    removedBy?: string | undefined;
    address?: AddressDTO;

    constructor(data?: IEntityAddressDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
            this.entityId = _data["entityId"];
            this.addressId = _data["addressId"];
            this.addressTypeId = _data["addressTypeId"];
            this.effectiveDate = _data["effectiveDate"] ? new Date(_data["effectiveDate"].toString()) : <any>undefined;
            this.expirationDate = _data["expirationDate"] ? new Date(_data["expirationDate"].toString()) : <any>undefined;
            this.addProcessDate = _data["addProcessDate"] ? new Date(_data["addProcessDate"].toString()) : <any>undefined;
            this.addedBy = _data["addedBy"];
            this.removeProcessDate = _data["removeProcessDate"] ? new Date(_data["removeProcessDate"].toString()) : <any>undefined;
            this.removedBy = _data["removedBy"];
            this.address = _data["address"] ? AddressDTO.fromJS(_data["address"]) : <any>undefined;
        }
    }

    static fromJS(data: any): EntityAddressDTO {
        data = typeof data === 'object' ? data : {};
        let result = new EntityAddressDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["entityId"] = this.entityId;
        data["addressId"] = this.addressId;
        data["addressTypeId"] = this.addressTypeId;
        data["effectiveDate"] = this.effectiveDate ? new Date(this.effectiveDate).toISOString() : <any>undefined; //.toISOString()
        data["expirationDate"] = this.expirationDate ? new Date(this.expirationDate).toISOString() : <any>undefined;
        data["addProcessDate"] = this.addProcessDate ? new Date(this.addProcessDate).toISOString() : <any>undefined;
        data["addedBy"] = this.addedBy;
        data["removeProcessDate"] = this.removeProcessDate ? this.removeProcessDate.toISOString() : <any>undefined;
        data["removedBy"] = this.removedBy;
        data["address"] = this.address ? this.address.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IEntityAddressDTO {
    id?: string;
    entityId?: string;
    addressId?: string;
    addressTypeId?: number;
    effectiveDate?: string;
    expirationDate?: string;
    addProcessDate?: Date;
    addedBy?: string;
    removeProcessDate?: Date | undefined;
    removedBy?: string | undefined;
    address?: AddressDTO;
}