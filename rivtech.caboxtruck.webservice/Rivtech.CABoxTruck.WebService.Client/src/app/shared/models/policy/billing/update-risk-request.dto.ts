export class UpdateRiskRequestDTO implements IUpdateRiskRequestDTO {
    riskId: string;
    policyNumber: string;

    constructor(data?: IUpdateRiskRequestDTO) {
        if (data) {
            for (const property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }
}

export interface IUpdateRiskRequestDTO {
    riskId: string;
    policyNumber: string;
}