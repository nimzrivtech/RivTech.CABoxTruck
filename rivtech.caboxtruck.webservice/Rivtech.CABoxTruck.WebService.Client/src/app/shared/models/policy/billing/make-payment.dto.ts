export class MakePaymentDTO implements IMakePaymentDTO {
  riskIdData?: string;
  policyNumberData?: string;
  insuredLastName?: string;
  payoffAmount?: number;
  amountDue?: number;
  pastDueAmount?: number;
  isFullPay?: boolean;
  isEnrolledToAutopay?: boolean;

  constructor(data?: IMakePaymentDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IMakePaymentDTO {
  riskIdData?: string;
  policyNumberData?: string;
  insuredLastName?: string;
  payoffAmount?: number;
  amountDue?: number;
  pastDueAmount?: number;
  isFullPay?: boolean;
  isEnrolledToAutopay?: boolean;
}