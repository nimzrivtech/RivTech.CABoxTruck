export class TransactionFeeDTO implements ITransactionFeeDTO {
    id?: string;
    addDate?: Date;
    description?: string;
    amount?: number;
    voidDate?: Date;

    constructor(data?: ITransactionFeeDTO) {
      if (data) {
        for (const property in data) {
          if (data.hasOwnProperty(property)) {
            (<any>this)[property] = (<any>data)[property];
          }
        }
      }
    }
}

export interface ITransactionFeeDTO {
    id?: string;
    addDate?: Date;
    description?: string;
    amount?: number;
    voidDate?: Date;
}