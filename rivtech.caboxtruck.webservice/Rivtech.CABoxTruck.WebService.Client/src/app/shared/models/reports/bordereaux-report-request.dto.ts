export class BordereauxReportRequestDTO implements IBordereauxReportRequestDTO {
  fromDate?: string;
  toDate?: string;

  constructor(data?: IBordereauxReportRequestDTO) {
      if (data) {
          for (const property in data) {
              if (data.hasOwnProperty(property)) {
                (<any>this)[property] = (<any>data)[property];
              }
          }
      }
  }
}

export interface IBordereauxReportRequestDTO {
  fromDate?: string;
  toDate?: string;
}
