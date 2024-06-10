export class PreBindPaymentDTO implements IPreBindPaymentDTO {
  id?: string;
  insuredName?: string;
  accessCode?: string;
  payeeName?: string;
  payeeEmail?: string;
  paymentDateTime?: Date;
  paymentMethod?: string;
  paymentAmount?: number;
  confirmationCode?: string;

  constructor(data?: IPreBindPaymentDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IPreBindPaymentDTO {
  id?: string;
  insuredName?: string;
  accessCode?: string;
  payeeName?: string;
  payeeEmail?: string;
  paymentDateTime?: Date;
  paymentMethod?: string;
  paymentAmount?: number;
  confirmationCode?: string;
}