import { ApplicationUserDTO } from './common/application-user.dto';
import { EnumerationDTO } from './common/enumeration.dto';
import { PaymentDetailDTO } from './payment-detail.dto';

export class PaymentDTO implements IPaymentDTO {
  id?: string;
  riskId?: string;
  amount?: number;
  effectiveDate?: Date;
  reference?: string;
  comment?: string;
  createdDate?: string;
  createdById?: string;
  voidOfPaymentId?: string;
  voidedById?: string;
  voidDate?: string;
  instrumentType?: EnumerationDTO;
  createdBy?: ApplicationUserDTO;
  voidedBy?: ApplicationUserDTO;
  paymentDetails?: PaymentDetailDTO[];

  constructor(data?: IPaymentDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IPaymentDTO {
  id?: string;
  riskId?: string;
  amount?: number;
  effectiveDate?: Date;
  reference?: string;
  comment?: string;
  createdDate?: string;
  createdById?: string;
  voidOfPaymentId?: string;
  voidedById?: string;
  voidDate?: string;
  instrumentType?: EnumerationDTO;
  createdBy?: ApplicationUserDTO;
  voidedBy?: ApplicationUserDTO;
  paymentDetails?: PaymentDetailDTO[];
}