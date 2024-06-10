export class AllowedPaymentRangeDTO implements IAllowedPaymentRangeDTO {
  minimumPaymentAmount?: number;
  maximumPaymentAmount?: number;
  isAccountEnrolled?: boolean;

  constructor(data?: IAllowedPaymentRangeDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IAllowedPaymentRangeDTO {
  minimumPaymentAmount?: number;
  maximumPaymentAmount?: number;
  isAccountEnrolled?: boolean;
}