export class ReturnedSuspendedPaymentListDTO implements IReturnedSuspendedPaymentListDTO {
  suspendedPaymentId?: string;
  returnDate?: Date;
  payee?: string;
  amount?: number;
  checkIssueDate?: Date;
  checkNumber?: string;
  checkClearDate?: Date;
  checkEscheatDate?: Date;

  constructor(data?: ReturnedSuspendedPaymentListDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IReturnedSuspendedPaymentListDTO {
  suspendedPaymentId?: string;
  returnDate?: Date;
  payee?: string;
  amount?: number;
  checkIssueDate?: Date;
  checkNumber?: string;
  checkClearDate?: Date;
  checkEscheatDate?: Date;
}