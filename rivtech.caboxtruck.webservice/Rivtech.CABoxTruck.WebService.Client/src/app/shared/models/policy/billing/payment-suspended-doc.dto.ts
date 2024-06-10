export class PaymentSuspendedDocDTO implements IPaymentSuspendedDocDTO {
  id?: string | undefined;
  riskId?: string | undefined;
  suspendedPaymentId?: string | undefined;
  description?: string | undefined;
  file?: any;
  fileName?: string | undefined;
  filePath: string | undefined;
  isUploaded?: boolean;
  createdBy?: number;
  createdDate?: Date;
  readonly isActive?: boolean;

  constructor(data?: IPaymentSuspendedDocDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IPaymentSuspendedDocDTO {
  id?: string | undefined;
  riskId?: string | undefined;
  suspendedPaymentId?: string | undefined;
  description?: string | undefined;
  file?: any;
  fileName?: string | undefined;
  filePath: string | undefined;
  isUploaded?: boolean;
  createdBy?: number;
  createdDate?: Date;
}
