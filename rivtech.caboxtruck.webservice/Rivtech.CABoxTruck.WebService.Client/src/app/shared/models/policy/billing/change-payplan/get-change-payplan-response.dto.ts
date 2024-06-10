import { PayplanPaymentDetailsDTO } from './payplan-payment-details.dto';

export class GetChangePayPlanResponseDTO implements IGetChangePayPlanResponseDTO {
  paymentPlanId: string;
  amountToPay: number;
  details: PayplanPaymentDetailsDTO;

  constructor(data?: IGetChangePayPlanResponseDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IGetChangePayPlanResponseDTO {
  paymentPlanId: string;
  amountToPay: number;
  details: PayplanPaymentDetailsDTO;
}