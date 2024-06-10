export class CopyPaymentRelatedDocDTO implements ICopyPaymentRelatedDocDTO {
  sourcePaymentId?: string;
  destinationPaymentId?: string;
  destinationRiskId?: string;

  constructor(data?: ICopyPaymentRelatedDocDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface ICopyPaymentRelatedDocDTO {
  sourcePaymentId?: string;
  destinationPaymentId?: string;
  destinationRiskId?: string;
}