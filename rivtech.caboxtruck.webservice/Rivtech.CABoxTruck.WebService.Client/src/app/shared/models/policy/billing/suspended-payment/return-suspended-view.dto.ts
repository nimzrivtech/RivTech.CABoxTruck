import { ReturnSuspendedViewDetailDTO } from './return-suspended-view-detail.dto';

export class ReturnSuspendedViewDTO implements IReturnSuspendedViewDTO {
  id?: string;
  amount?: number;
  comment?: string;
  payer?: string;
  policyNumber?: string;
  reason?: string;
  receiptDate?: Date;
  status?: string;
  returnDetail?: ReturnSuspendedViewDetailDTO;

  constructor(data?: IReturnSuspendedViewDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IReturnSuspendedViewDTO {
  id?: string;
  amount?: number;
  comment?: string;
  payer?: string;
  policyNumber?: string;
  reason?: string;
  receiptDate?: Date;
  status?: string;
  returnDetail?: ReturnSuspendedViewDetailDTO;
}