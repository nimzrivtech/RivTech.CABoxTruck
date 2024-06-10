export class AddressTypeDTO implements IAddressTypeDTO {
    id?: number;
    name?: string | undefined;
    isActive?: boolean;

    constructor(data?: IAddressTypeDTO) {
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
            this.name = _data["name"];
            this.isActive = _data["isActive"];
        }
    }

    static fromJS(data: any): AddressTypeDTO {
        data = typeof data === 'object' ? data : {};
        let result = new AddressTypeDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["name"] = this.name;
        data["isActive"] = this.isActive;
        return data; 
    }
}

export interface IAddressTypeDTO {
    id?: number;
    name?: string | undefined;
    isActive?: boolean;
}