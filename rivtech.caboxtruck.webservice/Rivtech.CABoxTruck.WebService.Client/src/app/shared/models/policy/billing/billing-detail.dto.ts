import { BankAccountDTO } from './bank-account.dto';
import { BillingAddressDTO } from './billing-address.dto';
import { CreditCardDTO } from './credit-card.dto';

export class BillingDetailDTO implements IBillingDetailDTO {
  creditCard?: CreditCardDTO;
  billingAddress?: BillingAddressDTO;
  bankAccount?: BankAccountDTO;

  constructor(data?: IBillingDetailDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IBillingDetailDTO {
  creditCard?: CreditCardDTO;
  billingAddress?: BillingAddressDTO;
  bankAccount?: BankAccountDTO;
}