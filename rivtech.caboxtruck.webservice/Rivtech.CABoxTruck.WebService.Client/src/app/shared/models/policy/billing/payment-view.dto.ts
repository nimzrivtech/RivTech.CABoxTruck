export class PaymentViewDTO implements IPaymentViewDTO {
  id?: string;
  amount?: number;
  postDate?: Date;
  paymentType: string;
  method: string;
  reference: string;
  comment: string;
  clearDate: Date;
  reversalDate: Date;

  constructor(data?: IPaymentViewDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IPaymentViewDTO {
  id?: string;
  amount?: number;
  postDate?: Date;
  paymentType: string;
  method: string;
  reference: string;
  comment: string;
  clearDate: Date;
  reversalDate: Date;
}