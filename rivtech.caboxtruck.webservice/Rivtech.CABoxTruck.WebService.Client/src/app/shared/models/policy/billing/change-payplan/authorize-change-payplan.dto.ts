import { BillingDetailDTO } from '../billing-detail.dto';
import { PayplanPaymentDetailsDTO } from './payplan-payment-details.dto';

export class AuthorizeChangePayPlanDTO implements IAuthorizeChangePayPlanDTO {
  effectiveDate: string;
  instrumentId: string;
  paymentPlan: string;
  billingDetail: BillingDetailDTO;
  paymentDetails: PayplanPaymentDetailsDTO;
  appId: string;

  constructor(data?: IAuthorizeChangePayPlanDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IAuthorizeChangePayPlanDTO {
  effectiveDate: string;
  instrumentId: string;
  paymentPlan: string;
  billingDetail: BillingDetailDTO;
  paymentDetails: PayplanPaymentDetailsDTO;
  appId: string;
}