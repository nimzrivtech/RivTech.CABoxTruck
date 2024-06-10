export class EnvCoverageServiceTypeDTO implements IEnvCoverageServiceTypeDTO {
    id?: number;
    serviceClass?: number;
    coverageType?: string | undefined;
    description?: string | undefined;
    isActive?: boolean;

    constructor(data?: IEnvCoverageServiceTypeDTO) {
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
            this.serviceClass = _data["serviceClass"];
            this.coverageType = _data["coverageType"];
            this.description = _data["description"];
            this.isActive = _data["isActive"];
        }
    }

    static fromJS(data: any): EnvCoverageServiceTypeDTO {
        data = typeof data === 'object' ? data : {};
        let result = new EnvCoverageServiceTypeDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["serviceClass"] = this.serviceClass;
        data["coverageType"] = this.coverageType;
        data["description"] = this.description;
        data["isActive"] = this.isActive;
        return data; 
    }
}

export interface IEnvCoverageServiceTypeDTO {
    id?: number;
    serviceClass?: number;
    coverageType?: string | undefined;
    description?: string | undefined;
    isActive?: boolean;
}