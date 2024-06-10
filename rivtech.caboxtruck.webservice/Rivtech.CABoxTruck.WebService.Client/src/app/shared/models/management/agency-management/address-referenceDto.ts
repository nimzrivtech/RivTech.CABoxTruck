import { AddressDTO } from '../addressDto';

export class AddressReferenceDTO implements IAddressReferenceDTO {
    id?: number;
    addresses?: AddressDTO[] | undefined;

    constructor(data?: IAddressReferenceDTO) {
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
            if (Array.isArray(_data["addresses"])) {
                this.addresses = [] as any;
                for (let item of _data["addresses"])
                    this.addresses!.push(AddressDTO.fromJS(item));
            }
        }
    }

    static fromJS(data: any): AddressReferenceDTO {
        data = typeof data === 'object' ? data : {};
        let result = new AddressReferenceDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        if (Array.isArray(this.addresses)) {
            data["addresses"] = [];
            for (let item of this.addresses)
                data["addresses"].push(item.toJSON());
        }
        return data; 
    }
}

export interface IAddressReferenceDTO {
    id?: number;
    addresses?: AddressDTO[] | undefined;
}