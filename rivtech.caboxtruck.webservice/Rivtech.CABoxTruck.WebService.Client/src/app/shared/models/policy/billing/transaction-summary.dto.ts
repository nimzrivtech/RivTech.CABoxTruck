export class TransactionSummaryDTO implements ITransactionSummaryDTO {
  premium?: number;
  fees?: number;
  tax?: number;
  total?: number;

  constructor(data?: ITransactionSummaryDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface ITransactionSummaryDTO {
  premium?: number;
  fees?: number;
  tax?: number;
  total?: number;
}