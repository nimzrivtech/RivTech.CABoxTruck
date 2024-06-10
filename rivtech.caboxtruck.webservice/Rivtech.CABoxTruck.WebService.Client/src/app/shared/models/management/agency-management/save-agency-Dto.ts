import { EntityDTO } from './entityDto';

export class SaveAgencyDTO implements ISaveAgencyDTO {
    id?: string;
    createdBy?: string;
    createdDate?: Date;
    programId?: number;
    entity?: EntityDTO;

    constructor(data?: ISaveAgencyDTO) {
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
            this.programId = _data["programId"];
            this.createdBy = _data["createdBy"];
            this.createdDate = _data["createdDate"] ? new Date(_data["createdDate"].toString()) : <any>undefined;
            this.entity = _data["entity"] ? EntityDTO.fromJS(_data["entity"]) : <any>undefined;
        }
    }

    static fromJS(data: any): SaveAgencyDTO {
        data = typeof data === 'object' ? data : {};
        let result = new SaveAgencyDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["programId"] = this.programId;
        data["createdBy"] = this.createdBy;
        data["createdDate"] = this.createdDate ? this.createdDate.toISOString() : <any>undefined;
        data["entity"] = this.entity ? this.entity.toJSON() : <any>undefined;
        return data; 
    }
}

export interface ISaveAgencyDTO {
    id?: string;
    createdBy?: string;
    createdDate?: Date;
    programId?: number;
    entity?: EntityDTO;
}
