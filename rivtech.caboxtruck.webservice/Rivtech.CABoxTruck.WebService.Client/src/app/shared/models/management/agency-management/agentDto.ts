import { AgencyDTO } from './agencyDto';

export class AgentDTO implements IAgentDTO {
    agentID?: number;
    agencyID?: number;
    subAgencyID?: number;
    userName?: string | undefined;
    emailAddress?: string | undefined;
    isActive?: number;
    createdAt?: Date;
    deletedDate?: Date;
    agency?: AgencyDTO;

    constructor(data?: IAgentDTO) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.agentID = _data["agentID"];
            this.agencyID = _data["agencyID"];
            this.subAgencyID = _data["subAgencyID"];
            this.userName = _data["userName"];
            this.emailAddress = _data["emailAddress"];
            this.isActive = _data["isActive"];
            this.createdAt = _data["createdAt"] ? new Date(_data["createdAt"].toString()) : <any>undefined;
            this.deletedDate = _data["deletedDate"] ? new Date(_data["deletedDate"].toString()) : <any>undefined;
            this.agency = _data["agency"] ? AgencyDTO.fromJS(_data["agency"]) : <any>undefined;
        }
    }

    static fromJS(data: any): AgentDTO {
        data = typeof data === 'object' ? data : {};
        let result = new AgentDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["agentID"] = this.agentID;
        data["agencyID"] = this.agencyID;
        data["subAgencyID"] = this.subAgencyID;
        data["userName"] = this.userName;
        data["emailAddress"] = this.emailAddress;
        data["isActive"] = this.isActive;
        data["createdAt"] = this.createdAt ? this.createdAt.toISOString() : <any>undefined;
        data["deletedDate"] = this.deletedDate ? this.deletedDate.toISOString() : <any>undefined;
        data["agency"] = this.agency ? this.agency.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IAgentDTO {
    agentID?: number;
    agencyID?: number;
    subAgencyID?: number;
    userName?: string | undefined;
    emailAddress?: string | undefined;
    isActive?: number;
    createdAt?: Date;
    deletedDate?: Date;
    agency?: AgencyDTO;
}