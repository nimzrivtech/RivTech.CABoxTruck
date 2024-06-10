export class SubmissionQuoteReportRequestDTO implements ISubmissionQuoteReportRequestDTO {
  fromDate?: string;
  toDate?: string;

  constructor(data?: ISubmissionQuoteReportRequestDTO) {
      if (data) {
          for (const property in data) {
              if (data.hasOwnProperty(property)) {
                (<any>this)[property] = (<any>data)[property];
              }
          }
      }
  }
}

export interface ISubmissionQuoteReportRequestDTO {
  fromDate?: string;
  toDate?: string;
}
