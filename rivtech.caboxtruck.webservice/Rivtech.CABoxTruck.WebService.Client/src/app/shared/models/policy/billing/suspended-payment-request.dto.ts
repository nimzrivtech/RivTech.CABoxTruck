import { PayerDTO } from './payer.dto';

export class SuspendedPaymentRequestDTO implements ISuspendedPaymentRequestDTO {
  receiptDate?: Date;
  amount?: number;
  policyNumber?: string;
  reasonId?: string;
  sourceId?: string;
  payer?: PayerDTO;
  comment?: string;

  constructor(data?: ISuspendedPaymentRequestDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface ISuspendedPaymentRequestDTO {
  receiptDate?: Date;
  amount?: number;
  policyNumber?: string;
  reasonId?: string;
  sourceId?: string;
  payer?: PayerDTO;
  comment?: string;
}