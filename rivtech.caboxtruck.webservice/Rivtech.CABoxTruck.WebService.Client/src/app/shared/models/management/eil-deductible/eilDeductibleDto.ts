export class EilDeductibleDTO implements IEilDeductibleDTO {
    value?: number;
    id?: number;
    isActive?: boolean;

    constructor(data?: IEilDeductibleDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.value = _data["value"];
            this.id = _data["id"];
            this.isActive = _data["isActive"];
        }
    }

    static fromJS(data: any): EilDeductibleDTO {
        data = typeof data === 'object' ? data : {};
        let result = new EilDeductibleDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["value"] = this.value;
        data["id"] = this.id;
        data["isActive"] = this.isActive;
        return data; 
    }
}

export interface IEilDeductibleDTO {
    value?: number;
    id?: number;
    isActive?: boolean;
}