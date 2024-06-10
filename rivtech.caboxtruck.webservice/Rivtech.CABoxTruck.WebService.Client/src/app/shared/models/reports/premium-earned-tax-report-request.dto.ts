export class PremiumEarnedTaxReportRequestDTO implements IPremiumEarnedTaxReportRequestDTO {
  fromDate?: string;
  toDate?: string;

  constructor(data?: IPremiumEarnedTaxReportRequestDTO) {
      if (data) {
          for (const property in data) {
              if (data.hasOwnProperty(property)) {
                (<any>this)[property] = (<any>data)[property];
              }
          }
      }
  }
}

export interface IPremiumEarnedTaxReportRequestDTO {
  fromDate?: string;
  toDate?: string;
}
