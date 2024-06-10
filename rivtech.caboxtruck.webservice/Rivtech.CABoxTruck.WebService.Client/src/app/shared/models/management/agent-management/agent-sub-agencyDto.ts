export class AgentSubAgencyDTO implements IAgentSubAgencyDTO {
    id?: string;
    agentId?: string;
    subAgencyId?: string;
    effectiveDate?: Date;
    expirationDate?: Date;
    addProcessDate?: Date;
    removeProcessDate?: string | undefined;

    constructor(data?: IAgentSubAgencyDTO) {
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
            this.agentId = _data["agentId"];
            this.subAgencyId = _data["subAgencyId"];
            this.effectiveDate = _data["effectiveDate"] ? new Date(_data["effectiveDate"].toString()) : <any>undefined;
            this.expirationDate = _data["expirationDate"] ? new Date(_data["expirationDate"].toString()) : <any>undefined;
            this.addProcessDate = _data["addProcessDate"] ? new Date(_data["addProcessDate"].toString()) : <any>undefined;
            this.removeProcessDate = _data["removeProcessDate"];
        }
    }

    static fromJS(data: any): AgentSubAgencyDTO {
        data = typeof data === 'object' ? data : {};
        let result = new AgentSubAgencyDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["agentId"] = this.agentId;
        data["subAgencyId"] = this.subAgencyId;
        data["effectiveDate"] = this.effectiveDate ? this.effectiveDate.toISOString() : <any>undefined;
        data["expirationDate"] = this.expirationDate ? this.expirationDate.toISOString() : <any>undefined;
        data["addProcessDate"] = this.addProcessDate ? this.addProcessDate.toISOString() : <any>undefined;
        data["removeProcessDate"] = this.removeProcessDate;
        return data; 
    }
}

export interface IAgentSubAgencyDTO {
    id?: string;
    agentId?: string;
    subAgencyId?: string;
    effectiveDate?: Date;
    expirationDate?: Date;
    addProcessDate?: Date;
    removeProcessDate?: string | undefined;
}