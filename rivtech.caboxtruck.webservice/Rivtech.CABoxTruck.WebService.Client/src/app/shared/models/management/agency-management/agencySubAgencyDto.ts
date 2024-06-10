import { AgencyDTO } from './agencyDto';
import { SubAgencyDTO } from './sub-agencyDto';

export class AgencySubAgencyDTO implements IAgencySubAgencyDTO {
    id?: string;
    agencyId?: string;
    subAgencyId?: string;
    effectiveDate?: string;
    expirationDate?: string;
    addProcessDate?: Date;
    removeProcessDate?: Date | undefined;
    agency?: AgencyDTO;
    subAgency?: SubAgencyDTO;

    constructor(data?: IAgencySubAgencyDTO) {
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
            this.agency = _data["agency"] ? AgencyDTO.fromJS(_data["agency"]) : <any>undefined;
            this.subAgency = _data["subAgency"] ? SubAgencyDTO.fromJS(_data["subAgency"]) : <any>undefined;
        }
    }

    static fromJS(data: any): AgencySubAgencyDTO {
        data = typeof data === 'object' ? data : {};
        let result = new AgencySubAgencyDTO();
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
        data["addProcessDate"] = this.addProcessDate ? this.addProcessDate.toISOString() : <any>undefined;
        data["removeProcessDate"] = this.removeProcessDate ? this.removeProcessDate.toISOString() : <any>undefined;
        data["agency"] = this.agency ? this.agency.toJSON() : <any>undefined;
        data["subAgency"] = this.subAgency ? this.subAgency.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IAgencySubAgencyDTO {
    id?: string;
    agencyId?: string;
    subAgencyId?: string;
    effectiveDate?: string;
    expirationDate?: string;
    addProcessDate?: Date;
    removeProcessDate?: Date | undefined;
    agency?: AgencyDTO;
    subAgency?: SubAgencyDTO;
}