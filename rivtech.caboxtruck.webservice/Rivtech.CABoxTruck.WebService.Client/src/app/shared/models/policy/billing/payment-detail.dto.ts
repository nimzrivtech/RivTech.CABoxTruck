import { EnumerationDTO } from "./common/enumeration.dto";

export class PaymentDetailDTO implements IPaymentDetailDTO {
  id?: string;
  paymentId?: string;
  amount?: number;
  effectiveDate?: Date;
  amountTypeId?: string;
  amountType?: EnumerationDTO;

  constructor(data?: IPaymentDetailDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IPaymentDetailDTO {
  id?: string;
  paymentId?: string;
  amount?: number;
  effectiveDate?: Date;
  amountTypeId?: string;
  amountType?: EnumerationDTO;
}