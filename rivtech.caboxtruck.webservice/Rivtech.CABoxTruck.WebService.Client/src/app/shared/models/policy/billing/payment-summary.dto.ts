export class PaymentSummaryDTO implements IPaymentSummaryDTO {
  effectiveDate?: string;
  instrumentId?: string;
  amount?: number;
  premiumAmount?: number;
  feeAmount?: number;
  taxAmount?: number;
  comment?: string;
  isRecurringPayment?: boolean;
  email?: string;

  constructor(data?: IPaymentSummaryDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IPaymentSummaryDTO {
  effectiveDate?: string;
  instrumentId?: string;
  amount?: number;
  premiumAmount?: number;
  feeAmount?: number;
  taxAmount?: number;
  comment?: string;
  isRecurringPayment?: boolean;
  email?: string;
}