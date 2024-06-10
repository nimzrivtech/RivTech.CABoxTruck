export class MaritalStatusDTO implements IMaritalStatusDTO {
    description?: string | undefined;
    id?: number;
    isActive?: boolean;

    constructor(data?: IMaritalStatusDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.description = _data["description"];
            this.id = _data["id"];
            this.isActive = _data["isActive"];
        }
    }

    static fromJS(data: any): MaritalStatusDTO {
        data = typeof data === 'object' ? data : {};
        let result = new MaritalStatusDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["description"] = this.description;
        data["id"] = this.id;
        data["isActive"] = this.isActive;
        return data; 
    }
}

export interface IMaritalStatusDTO {
    description?: string | undefined;
    id?: number;
    isActive?: boolean;
}