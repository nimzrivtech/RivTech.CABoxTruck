export class ReturnSuspendedViewDetailDTO implements IReturnSuspendedViewDetailDTO {
  id: string;
  payee: string;
  checkNumber: string;
  checkClearDate?: Date;
  checkEscheatDate?: Date;
  checkIssueDate?: Date;
  returnDate?: Date;

  constructor(data?: IReturnSuspendedViewDetailDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IReturnSuspendedViewDetailDTO {
  id: string;
  payee: string;
  checkNumber: string;
  checkClearDate?: Date;
  checkEscheatDate?: Date;
  checkIssueDate?: Date;
  returnDate?: Date;
}