import { SuspendedPayeeRequestDTO } from './suspended-payee-request.dto';

export class ReturnSuspendedPaymentDTO implements IReturnSuspendedPaymentDTO {
  suspendedPaymentId: string;
  payee: SuspendedPayeeRequestDTO;
  comment: string;

  constructor(data?: IReturnSuspendedPaymentDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IReturnSuspendedPaymentDTO {
  suspendedPaymentId: string;
  payee: SuspendedPayeeRequestDTO;
  comment: string;
}