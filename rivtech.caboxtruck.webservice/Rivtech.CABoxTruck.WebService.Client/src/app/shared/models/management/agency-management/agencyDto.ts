import { EntityDTO } from './entityDto';

export class AgencyDTO implements IAgencyDTO {
    id?: string;
    entityId?: string;
    createdBy?: string;
    createdDate?: Date;
    entity?: EntityDTO;
    contactName?: string;

    constructor(data?: IAgencyDTO) {
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
            this.createdBy = _data["createdBy"];
            this.createdDate = _data["createdDate"] ? new Date(_data["createdDate"].toString()) : <any>undefined;
            this.entity = _data["entity"] ? EntityDTO.fromJS(_data["entity"]) : <any>undefined;
        }
    }

    static fromJS(data: any): AgencyDTO {
        data = typeof data === 'object' ? data : {};
        let result = new AgencyDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["entityId"] = this.entityId;
        data["createdBy"] = this.createdBy;
        data["createdDate"] = this.createdDate ? this.createdDate.toISOString() : <any>undefined;
        data["entity"] = this.entity ? this.entity.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IAgencyDTO {
    id?: string;
    entityId?: string;
    createdBy?: string;
    createdDate?: Date;
    entity?: EntityDTO;
}
