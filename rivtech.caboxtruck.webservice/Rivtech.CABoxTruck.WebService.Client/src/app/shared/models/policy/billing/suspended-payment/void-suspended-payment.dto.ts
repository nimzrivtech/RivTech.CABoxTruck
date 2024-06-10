export class VoidSuspendedPaymentDTO implements IVoidSuspendedPaymentDTO {
  suspendedPaymentId: string;
  comment: string;

  constructor(data?: IVoidSuspendedPaymentDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IVoidSuspendedPaymentDTO {
  suspendedPaymentId: string;
  comment: string;
}