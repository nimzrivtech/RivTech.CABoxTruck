export class TransferPaymentDTO implements ITransferPaymentDTO {
  fromRiskId?: string;
  toRiskId?: string;
  paymentId?: string;
  comments?: string;
  toPolicyNumber?: string;
  fromPolicyNumber?: string;

  constructor(data?: ITransferPaymentDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface ITransferPaymentDTO {
  fromRiskId?: string;
  toRiskId?: string;
  paymentId?: string;
  comments?: string;
  toPolicyNumber?: string;
  fromPolicyNumber?: string;
}