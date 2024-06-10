export class CreditCardDTO implements ICreditCardDTO {
  cardCode?: Date;
  cardNumber?: string;
  expirationDate?: string;
  creditCardTypeId?: number;

  constructor(data?: ICreditCardDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface ICreditCardDTO {
  cardCode?: Date;
  cardNumber?: string;
  expirationDate?: string;
}