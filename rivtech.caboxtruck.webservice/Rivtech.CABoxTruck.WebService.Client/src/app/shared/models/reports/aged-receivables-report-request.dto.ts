export class AgedReceivablesReportRequestDTO implements IAgedReceivablesReportRequestDTO {
    toDate?: string;

    constructor(data?: IAgedReceivablesReportRequestDTO) {
        if (data) {
            for (const property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }
}
  
export interface IAgedReceivablesReportRequestDTO {
    toDate?: string;
}
  