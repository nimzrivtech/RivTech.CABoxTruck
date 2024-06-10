import { SaveAgencyDTO } from './save-agency-Dto';
import { SaveSubAgencyDTO } from './save-sub-agencyDto';

export class SaveAgencySubAgencyDTO implements ISaveAgencySubAgencyDTO {
    id?: string;
    agencyId: string;
    subAgencyId: string;
    effectiveDate?: string;
    expirationDate?: string;
    addProcessDate?: Date;
    removeProcessDate?: Date | undefined;
    agency?: SaveAgencyDTO;
    subAgency?: SaveSubAgencyDTO;

    constructor(data?: ISaveAgencySubAgencyDTO) {
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
            this.agencyId = _data["agencyId"];
            this.subAgencyId = _data["subAgencyId"];
            this.effectiveDate = _data["effectiveDate"] ? new Date(_data["effectiveDate"].toString()) : <any>undefined;
            this.expirationDate = _data["expirationDate"] ? new Date(_data["expirationDate"].toString()) : <any>undefined;
            this.addProcessDate = _data["addProcessDate"] ? new Date(_data["addProcessDate"].toString()) : <any>undefined;
            this.removeProcessDate = _data["removeProcessDate"] ? new Date(_data["removeProcessDate"].toString()) : <any>undefined;
            this.agency = _data["agency"] ? SaveAgencyDTO.fromJS(_data["agency"]) : <any>undefined;
            this.subAgency = _data["subAgency"] ? SaveSubAgencyDTO.fromJS(_data["subAgency"]) : <any>undefined;
        }
    }

    static fromJS(data: any): SaveAgencySubAgencyDTO {
        data = typeof data === 'object' ? data : {};
        let result = new SaveAgencySubAgencyDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["agencyId"] = this.agencyId;
        data["subAgencyId"] = this.subAgencyId;
        data["effectiveDate"] = this.effectiveDate ? new Date(this.effectiveDate).toISOString() : <any>undefined;
        data["expirationDate"] = this.expirationDate ? new Date(this.expirationDate).toISOString() : <any>undefined;
        data["addProcessDate"] = this.addProcessDate ? new Date(this.addProcessDate).toISOString() : <any>undefined;
        data["removeProcessDate"] = this.removeProcessDate ? this.removeProcessDate.toISOString() : <any>undefined;
        data["agency"] = this.agency ? this.agency.toJSON() : <any>undefined;
        data["subAgency"] = this.subAgency ? this.subAgency.toJSON() : <any>undefined;
        return data; 
    }
}

export interface ISaveAgencySubAgencyDTO {
    id?: string;
    agencyId?: string;
    subAgencyId?: string;
    effectiveDate?: string;
    expirationDate?: string;
    addProcessDate?: Date;
    removeProcessDate?: Date | undefined;
    agency?: SaveAgencyDTO;
    subAgency?: SaveSubAgencyDTO;
}