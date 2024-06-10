export class PaymentDetailsViewDTO implements IPaymentDetailsViewDTO {
  paymentId?: string;
  amount?: number;
  postDate?: Date;
  postedBy?: string;
  premium?: number;
  tax?: number;
  fee?: number;
  paymentMethod: string;
  reference: string;
  reversalType: string;
  reversalDate: Date;
  comments: string;

  constructor(data?: IPaymentDetailsViewDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IPaymentDetailsViewDTO {
  paymentId?: string;
  amount?: number;
  postDate?: Date;
  postedBy?: string;
  premium?: number;
  tax?: number;
  fee?: number;
  paymentMethod: string;
  reference: string;
  reversalType: string;
  reversalDate: Date;
  comments: string;
}