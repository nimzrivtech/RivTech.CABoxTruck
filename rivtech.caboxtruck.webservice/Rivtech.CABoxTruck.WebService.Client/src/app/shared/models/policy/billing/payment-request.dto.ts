import { BillingDetailDTO } from "./billing-detail.dto";
import { PaymentSummaryDTO } from "./payment-summary.dto";
import { PaymentPortalRequest } from "./payment-portal-request";

export class PaymentRequestDTO implements IPaymentRequestDTO {
  riskId?: string;
  riskBindId?: string;
  paymentSummary?: PaymentSummaryDTO;
  billingDetail?: BillingDetailDTO;
  appId?: string;
  paymentPortalRequest?: PaymentPortalRequest

  constructor(data?: IPaymentRequestDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IPaymentRequestDTO {
  riskId?: string;
  paymentSummary?: PaymentSummaryDTO;
  billingDetail?: BillingDetailDTO;
  appId?: string;
  paymentPortalRequest?: PaymentPortalRequest
}