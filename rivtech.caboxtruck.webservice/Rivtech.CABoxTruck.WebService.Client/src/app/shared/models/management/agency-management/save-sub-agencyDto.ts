import { BusinessTypeDTO } from '../business-typeDto';
import { EntityDTO } from './entityDto';

export class SaveSubAgencyDTO implements ISaveSubAgencyDTO {
    id?: string;
    entityId?: string;
    createdBy?: string;
    createdDate?: Date;
    businessType?: BusinessTypeDTO;
    entity?: EntityDTO;

    constructor(data?: ISaveSubAgencyDTO) {
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
            this.businessType = _data["businessType"] ? BusinessTypeDTO.fromJS(_data["businessType"]) : <any>undefined;
            this.entity = _data["entity"] ? EntityDTO.fromJS(_data["entity"]) : <any>undefined;
        }
    }

    static fromJS(data: any): SaveSubAgencyDTO {
        data = typeof data === 'object' ? data : {};
        let result = new SaveSubAgencyDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["entityId"] = this.entityId;
        data["createdBy"] = this.createdBy;
        data["createdDate"] = this.createdDate ? this.createdDate.toISOString() : <any>undefined;
        data["businessType"] = this.businessType ? this.businessType.toJSON() : <any>undefined;
        data["entity"] = this.entity ? this.entity.toJSON() : <any>undefined;
        return data; 
    }
}

export interface ISaveSubAgencyDTO {
    id?: string;
    entityId?: string;
    createdBy?: string;
    createdDate?: Date;
    businessType?: BusinessTypeDTO;
    entity?: EntityDTO;
}