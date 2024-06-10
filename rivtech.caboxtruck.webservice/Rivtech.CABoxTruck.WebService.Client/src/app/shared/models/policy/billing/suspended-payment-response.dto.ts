import { EnumerationDTO } from './common/enumeration.dto';
import { PayerDTO } from './payer.dto';

export class SuspendedPaymentResponseDTO implements ISuspendedPaymentResponseDTO {
  id?: string;
  policyNumber?: string;
  amount?: number;
  receiptDate?: Date;
  payerId?: string;
  sourceId?: string;
  reasonId?: string;
  statusId?: string;
  comment?: string;
  checkRequested?: boolean;
  createdDate?: Date;
  createdById?: string;
  payer?: PayerDTO;
  source?: EnumerationDTO;
  reason?: EnumerationDTO;
  status?: EnumerationDTO;
  createdBy?: EnumerationDTO;

  constructor(data?: ISuspendedPaymentResponseDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface ISuspendedPaymentResponseDTO {
  id?: string;
  policyNumber?: string;
  amount?: number;
  receiptDate?: Date;
  payerId?: string;
  sourceId?: string;
  reasonId?: string;
  statusId?: string;
  comment?: string;
  checkRequested?: boolean;
  createdDate?: Date;
  createdById?: string;
  payer?: PayerDTO;
  source?: EnumerationDTO;
  reason?: EnumerationDTO;
  status?: EnumerationDTO;
  createdBy?: EnumerationDTO;
}