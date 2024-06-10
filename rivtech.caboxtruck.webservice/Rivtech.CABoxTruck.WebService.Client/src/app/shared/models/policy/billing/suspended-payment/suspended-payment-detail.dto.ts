export class SuspendedPaymentDetailsDTO implements ISuspendedPaymentDetailsDTO {
  id?: string;
  receiptDate?: string;
  policyNumber?: string;
  amount?: number;
  payer?: string;
  reason?: string;
  status?: string;
  comment?: string;
  statusId?: string;
  suspendedPaymentId?: string;
  returnDetail?: any;

  constructor(data?: SuspendedPaymentDetailsDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface ISuspendedPaymentDetailsDTO {
  id?: string;
  receiptDate?: string;
  policyNumber?: string;
  amount?: number;
  payer?: string;
  reason?: string;
  status?: string;
  comment?: string;
  statusId?: string;
  suspendedPaymentId?: string;
  returnDetail?: any;
}