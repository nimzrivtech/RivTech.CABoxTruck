export class BillingSummaryDetailsDTO implements IBillingSummaryDetailsDTO {
    description?: string;
    written?: number;
    billed?: number;
    paid?: number;
    balance?: number;
  
    constructor(data?: IBillingSummaryDetailsDTO) {
      if (data) {
        for (const property in data) {
          if (data.hasOwnProperty(property)) {
            (<any>this)[property] = (<any>data)[property];
          }
        }
      }
    }
  }
  
  export interface IBillingSummaryDetailsDTO {
    description?: string;
    written?: number;
    billed?: number;
    paid?: number;
    balance?: number;
  }