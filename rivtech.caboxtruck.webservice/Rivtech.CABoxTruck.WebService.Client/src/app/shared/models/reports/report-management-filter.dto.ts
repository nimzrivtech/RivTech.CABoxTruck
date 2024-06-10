import { Guid } from "guid-typescript";

export class ReportManagementFilterDTO implements IReportManagementFilterDTO {
  sortField?: string;
  thenSortField?: string;
  ascending?: boolean;
  skip?: number;
  pageSize?: number;
  dateMin?: string;
  dateMax?: string;
  reportType?: string;
  reportFileName?: string;
  riskDetailId?: string;

  constructor(data?: IReportManagementFilterDTO) {
      if (data) {
          for (const property in data) {
              if (data.hasOwnProperty(property)) {
                (<any>this)[property] = (<any>data)[property];
              }
          }
      }
  }
}

export interface IReportManagementFilterDTO {
  sortField?: string;
  thenSortField?: string;
  ascending?: boolean;
  skip?: number;
  pageSize?: number;
  dateMin?: string;
  dateMax?: string;
  reportType?: string;
  reportFileName?: string;
}
