export class TransactionTaxDTO implements ITransactionTaxDTO {
    id?: string;
    addDate?: Date;
    description?: string;
    amount?: number;
    voidDate?: Date;

    constructor(data?: ITransactionTaxDTO) {
      if (data) {
        for (const property in data) {
          if (data.hasOwnProperty(property)) {
            (<any>this)[property] = (<any>data)[property];
          }
        }
      }
    }
}

export interface ITransactionTaxDTO {
    id?: string;
    addDate?: Date;
    description?: string;
    amount?: number;
    voidDate?: Date;
}