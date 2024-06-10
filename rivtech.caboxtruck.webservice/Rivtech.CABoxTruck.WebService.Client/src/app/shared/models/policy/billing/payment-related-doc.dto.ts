export class PaymentRelatedDocDTO implements IPaymentRelatedDocDTO {
    id?: string | undefined;
    riskId?: string | undefined;
    paymentId?: string | undefined;
    description?: string | undefined;
    file?: any;
    fileName?: string | undefined;
    filePath: string | undefined;
    isUploaded?: boolean;
    createdBy?: number;
    createdDate?: Date;
    readonly isActive?: boolean;

    constructor(data?: IPaymentRelatedDocDTO) {
      if (data) {
        for (const property in data) {
          if (data.hasOwnProperty(property)) {
            (<any>this)[property] = (<any>data)[property];
          }
        }
      }
    }
  }

  export interface IPaymentRelatedDocDTO {
    id?: string | undefined;
    riskId?: string | undefined;
    paymentId?: string | undefined;
    description?: string | undefined;
    file?: any;
    fileName?: string | undefined;
    filePath: string | undefined;
    isUploaded?: boolean;
    createdBy?: number;
    createdDate?: Date;
  }
