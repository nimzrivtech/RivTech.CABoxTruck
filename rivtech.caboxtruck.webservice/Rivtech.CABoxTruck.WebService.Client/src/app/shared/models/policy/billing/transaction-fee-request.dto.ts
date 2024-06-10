export class TransactionFeeRequestDTO implements ITransactionFeeRequestDTO {
  riskId?: string;
  amountSubTypeId?: string;
  amount?: number;
  addDate?: Date;

  constructor(data?: ITransactionFeeRequestDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface ITransactionFeeRequestDTO {
  riskId?: string;
  amountSubTypeId?: string;
  amount?: number;
  addDate?: Date;
}