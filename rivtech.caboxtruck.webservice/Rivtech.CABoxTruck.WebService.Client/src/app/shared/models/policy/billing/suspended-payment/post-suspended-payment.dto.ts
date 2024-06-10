export class PostSuspendedPaymentDTO implements IPostSuspendedPaymentDTO {
  suspendedPaymentId: string;
  riskId: string;
  appId?: string;

  constructor(data?: IPostSuspendedPaymentDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IPostSuspendedPaymentDTO {
  suspendedPaymentId: string;
  riskId: string;
  appId?: string;
}