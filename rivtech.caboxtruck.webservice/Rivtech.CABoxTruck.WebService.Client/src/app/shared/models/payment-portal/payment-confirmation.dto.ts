export class PaymentConfirmationRequestDTO implements IPaymentConfirmationRequestDTO {
  insuredName?: string;
  policyNumber?: string;
  paymentDate?: string;
  paymentMethod?: string;
  amount?: number;
  underwriter?: string;
  email?: string;

  constructor(data?: PaymentConfirmationRequestDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IPaymentConfirmationRequestDTO {
  insuredName?: string;
  policyNumber?: string;
  paymentDate?: string;
  paymentMethod?: string;
  amount?: number;
  underwriter?: string;
  email?: string;
}