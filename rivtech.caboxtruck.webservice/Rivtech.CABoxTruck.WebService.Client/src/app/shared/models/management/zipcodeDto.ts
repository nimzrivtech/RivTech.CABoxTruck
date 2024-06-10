export class ZipCodeDTO implements IZipCodeDTO {
    id?: number;
    zipCode?: number;
    county?: string | undefined;
    city?: string | undefined;
    stateCode?: string | undefined;
    stateFullName?: string | undefined;

    constructor(data?: IZipCodeDTO) {
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
            this.zipCode = _data["zipCode"];
            this.county = _data["county"];
            this.city = _data["city"];
            this.stateCode = _data["stateCode"];
            this.stateFullName = _data["stateFullName"];
        }
    }

    static fromJS(data: any): ZipCodeDTO {
        data = typeof data === 'object' ? data : {};
        let result = new ZipCodeDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["zipCode"] = this.zipCode;
        data["county"] = this.county;
        data["city"] = this.city;
        data["stateCode"] = this.stateCode;
        data["stateFullName"] = this.stateFullName;
        return data; 
    }
}

export interface IZipCodeDTO {
    id?: number;
    zipCode?: number;
    county?: string | undefined;
    city?: string | undefined;
    stateCode?: string | undefined;
    stateFullName?: string | undefined;
}