export class MedicalExpensesLimitDTO implements IEnvMedicalExpensesLimitDTO {
    id?: number;
    value?: number;
    isActive?: boolean;

    constructor(data?: IEnvMedicalExpensesLimitDTO) {
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
            this.value = _data["value"];
            this.isActive = _data["isActive"];
        }
    }

    static fromJS(data: any): MedicalExpensesLimitDTO {
        data = typeof data === 'object' ? data : {};
        let result = new MedicalExpensesLimitDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["value"] = this.value;
        data["isActive"] = this.isActive;
        return data; 
    }
}

export interface IEnvMedicalExpensesLimitDTO {
    id?: number;
    value?: number;
    isActive?: boolean;
}