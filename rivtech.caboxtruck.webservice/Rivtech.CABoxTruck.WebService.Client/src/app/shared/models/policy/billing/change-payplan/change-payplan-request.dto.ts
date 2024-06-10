import { BillingDetailDTO } from '../billing-detail.dto';
import { PayplanPaymentDetailsDTO } from './payplan-payment-details.dto';

export class ChangePayPlanRequestDTO implements IChangePayPlanRequestDTO {
  riskId: string;
  instrumentId?: string;
  newPaymentPlanId: string;
  billingDetail?: BillingDetailDTO;
  paymentDetails: PayplanPaymentDetailsDTO;
  appId?: string;

  constructor(data?: IChangePayPlanRequestDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IChangePayPlanRequestDTO {
  riskId: string;
  instrumentId?: string;
  newPaymentPlanId: string;
  billingDetail?: BillingDetailDTO;
  paymentDetails: PayplanPaymentDetailsDTO;
  appId?: string;
}