export class PreBindPaymentConfirmationRequestDTO implements IPreBindPaymentConfirmationRequestDTO {
  insuredName?: string;
  paymentDate?: string;
  paymentMethod?: string;
  paymentMethodShort?: string;
  amount?: number;
  email?: string;

  constructor(data?: PreBindPaymentConfirmationRequestDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IPreBindPaymentConfirmationRequestDTO {
  insuredName?: string;
  paymentDate?: string;
  paymentMethod?: string;
  paymentMethodShort?: string;
  amount?: number;
  email?: string;
}