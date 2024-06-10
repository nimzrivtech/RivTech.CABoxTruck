export class NAICSCodeDTO implements INAICSCodeDTO {
    id: number;
    name: string;
    createdBy: number;
    createdDate: Date;
    isActive: boolean;

    constructor(data?: INAICSCodeDTO) {
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
            this.createdBy = _data["createdBy"];
            this.createdDate = _data["createdDate"];
            this.isActive = _data["isActive"];
        }
    }

    static fromJS(data: any): NAICSCodeDTO {
        data = typeof data === 'object' ? data : {};
        let result = new NAICSCodeDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["name"] = this.name;
        data["createdBy"] = this.createdBy;
        data["createdDate"] = this.createdDate;
        data["isActive"] = this.isActive;
        return data; 
    }
}

export interface INAICSCodeDTO {
    id: number;
    name: string;
    createdBy: number;
    createdDate: Date;
    isActive: boolean;
}