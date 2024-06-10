export class VoidTransactionFeeRequestDTO implements IVoidTransactionFeeRequestDTO {
  id?: string;
  riskId?: string;

  constructor(data?: IVoidTransactionFeeRequestDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IVoidTransactionFeeRequestDTO {
  id?: string;
  riskId?: string;
}