export class BillingAddressDTO implements IBillingAddressDTO {
  firstName?: string;
  lastName?: string;
  address?: string;
  city?: string;
  zip?: string;
  state?: string;
  checkNumber?: string;

  constructor(data?: IBillingAddressDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IBillingAddressDTO {
  firstName?: string;
  lastName?: string;
  address?: string;
  city?: string;
  zip?: string;
  state?: string;
  checkNumber?: string;
}