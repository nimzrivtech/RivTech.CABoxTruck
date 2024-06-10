export class PayplanPaymentDetailsDTO implements IPayplanPaymentDetailsDTO {
  totalAmount: number;
  premiumAmount: number;
  feeAmount: number;
  taxAmount: number;

  constructor(data?: IPayplanPaymentDetailsDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IPayplanPaymentDetailsDTO {
  totalAmount: number;
  premiumAmount: number;
  feeAmount: number;
  taxAmount: number;
}