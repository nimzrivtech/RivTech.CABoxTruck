export class ReverseSuspendedPaymentDTO implements IReverseSuspendedPaymentDTO {
  suspendedPaymentId?: string;
  comment?: string;

  constructor(data?: IReverseSuspendedPaymentDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IReverseSuspendedPaymentDTO {
  suspendedPaymentId?: string;
  comment?: string;
}